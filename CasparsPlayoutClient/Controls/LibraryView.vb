'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports CasparCGNETConnector
Imports logger

Public Class LibraryView

    Public WithEvents Library As Library
    Private Delegate Sub updateDelagete()
    Private Delegate Sub updateItemDelegate(ByRef item As AbstractCasparCGMedia)
    Private cMenu As ContextMenuStrip
    Private items As Dictionary(Of String, LibraryViewItem)
    Private updateBrake As New Threading.Semaphore(1, 1)
    Private Const brakeTime = 5
    Private WithEvents brakeTimer As New Timers.Timer(brakeTime)
    Private isBrakeActive As Boolean = False
    Private itemsLock As New Threading.Semaphore(1, 1)

    Public Sub New()
        items = New Dictionary(Of String, LibraryViewItem)
        InitializeComponent()
        cmbRefresh.Image = Image.FromFile("img/refresh-icon.png")
        pbProgress.Image = Image.FromFile("img/refresh-icon-ani.gif")
        initMenu()
        If My.Settings.libdir.Length = 0 Then My.Settings.libdir = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\library"
    End Sub

    Public Sub New(ByVal library As Library)
        items = New Dictionary(Of String, LibraryViewItem)
        Me.Library = library
        InitializeComponent()
        cmbRefresh.Image = Image.FromFile("img/refresh-icon.png")
        pbProgress.Image = Image.FromFile("img/refresh-icon-ani.gif")
        initMenu()
        refreshList()
        ' load last lib if pos.
        If My.Settings.libdir.Length = 0 Then My.Settings.libdir = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\library"
        If My.Settings.rememberLibrary AndAlso My.Settings.last_Library.Length > 0 Then
            Dim xmlDoc As New MSXML2.DOMDocument()
            If xmlDoc.loadXML(My.Settings.last_Library) Then loadXml(xmlDoc)
        End If
    End Sub

    '
    '' Hilfsmethoden
    '

    Private Sub initMenu()
        '' Add ContexMenu
        cMenu = New ContextMenuStrip
        cMenu.Items.Add(New ToolStripMenuItem("Load from XML", Nothing, Sub() loadXml()))
        cMenu.Items.Add(New ToolStripMenuItem("Save library to xml", Nothing, Sub() saveXmlLib()))
        cMenu.Items.Add(New ToolStripSeparator)
        cMenu.Items.Add(New ToolStripMenuItem("Clear library", Nothing, AddressOf clearLibrary))
        Me.ContextMenuStrip = cMenu
    End Sub

    Public Sub clearLibrary()
        Library.clear()
        layoutItemsFlow.Controls.Clear()
        items.Clear()
    End Sub

    Public Sub saveXmlLib()
        If Library.getItems.Count > 0 Then
            Dim domDoc As New MSXML2.DOMDocument
            Dim pnode = domDoc.createElement("library")

            For Each m In Library.getItems
                pnode.appendChild(m.toXml.firstChild)
            Next

            domDoc.appendChild(pnode)
            domDoc.save(My.Settings.libdir & "\" & "MediaLibrary.xml")

            logger.log("LibraryView.saveXmlLib: Media library successfully saved.")
        Else
            logger.warn("LibraryView.saveXmlLib: Library is empty. Nothing to save.")
        End If
    End Sub

    Public Function getXmlLib() As String
        Dim domDoc As New MSXML2.DOMDocument
        Dim pnode = domDoc.createElement("library")

        For Each m In Library.getItems
            pnode.appendChild(m.toXml.firstChild)
        Next

        domDoc.appendChild(pnode)
        Return domDoc.xml
    End Function

    Public Sub loadXml()
        Dim fd As New OpenFileDialog()
        fd.DefaultExt = "xml"
        fd.Filter = "Xml Dateien|*.xml"
        fd.CheckFileExists = True
        fd.Multiselect = True
        fd.InitialDirectory = My.Settings.libdir
        fd.ShowDialog()

        For Each f In fd.FileNames
            loadXml(f)
        Next
        applyFilter()
    End Sub

    Public Sub loadXml(ByVal fileName As String)
        Dim domDoc As New MSXML2.DOMDocument
        If My.Computer.FileSystem.FileExists(fileName) AndAlso domDoc.load(fileName) Then
            loadXml(domDoc)
        Else
            logger.err("LibraryView.loadXml: Unable to parse media file '" & fileName & "'. Not a valid xml file.")
        End If
    End Sub

    Public Sub loadXml(ByRef xmlDoc As MSXML2.DOMDocument)
        Dim media As AbstractCasparCGMedia
        If xmlDoc.hasChildNodes Then
            If xmlDoc.firstChild.nodeName.Equals("library") Then
                ' load whole lib
                For Each m As MSXML2.IXMLDOMNode In xmlDoc.firstChild.selectNodes("media")
                    media = CasparCGMediaFactory.createMedia(m.xml)
                    If Not IsNothing(media) Then
                        Library.addItem(media)
                        updateMediaItem(media)
                        logger.log("LibraryView.loadXml: Successfully loaded " & media.getName)
                    End If
                Next
            ElseIf xmlDoc.firstChild.nodeName.Equals("media") Then
                ' single media
                media = CasparCGMediaFactory.createMedia(xmlDoc.xml)
                If Not IsNothing(media) Then
                    Library.addItem(media)
                    updateMediaItem(media)
                    logger.log("LibraryView.loadXml: Successfully loaded " & media.getName)
                End If
            Else
                logger.warn("LibraryView.loadXml: Unable to load media. Not a valid media definition.")
            End If
        Else
            logger.err("LibraryView.loadXml: Unable to load media. Empty definition.")
        End If
        applyFilter()
    End Sub


    Private Sub applyFilter() Handles ckbAudio.CheckedChanged, ckbMovie.CheckedChanged, ckbStill.CheckedChanged, ckbTemplate.CheckedChanged, txtFilter.TextChanged
        layoutTypeFilterFlow.Enabled = False
        If itemsLock.WaitOne Then
            Dim targetItems As New List(Of LibraryViewItem)
            targetItems.AddRange(items.Values)
            Try
                itemsLock.Release()
            Catch ex As Exception
            End Try

            layoutItemsFlow.SuspendLayout()
            For Each item In targetItems
                Application.DoEvents()
                applyFilterToItem(item)
            Next
            layoutItemsFlow.ResumeLayout()
        End If
        layoutTypeFilterFlow.Enabled = True
    End Sub

    Private Sub applyFilterToItem(ByRef item As LibraryViewItem)
        Select Case item.MediaItem.getMediaType
            Case AbstractCasparCGMedia.MediaType.AUDIO
                item.Visible = ckbAudio.Checked
            Case AbstractCasparCGMedia.MediaType.MOVIE
                item.Visible = ckbMovie.Checked
            Case AbstractCasparCGMedia.MediaType.STILL
                item.Visible = ckbStill.Checked
            Case AbstractCasparCGMedia.MediaType.TEMPLATE
                item.Visible = ckbTemplate.Checked
        End Select

        If Not (txtFilter.Text.Length > 0 AndAlso item.MediaItem.getFullName.ToUpper Like txtFilter.Text.ToUpper & "*" OrElse item.MediaItem.getName.ToUpper Like txtFilter.Text.ToUpper & "*") Then
            item.Visible = False
        End If
    End Sub

    Private Sub refreshList() Handles Library.updated
        If InvokeRequired Then
            Invoke(New updateDelagete(AddressOf refreshList))
        Else
            If My.Settings.rememberLibrary AndAlso Library.getItems.Count > 0 Then My.Settings.last_Library = getXmlLib()
        End If
    End Sub

    ''' <summary>
    ''' Adds a CasparCGMedia to the LibraryView
    ''' </summary>
    ''' <param name="mediaItem"></param>
    ''' <remarks></remarks>
    Private Sub addMediaItem(ByRef mediaItem As AbstractCasparCGMedia)
        If Not items.ContainsKey(mediaItem.getUuid) Then
            Dim libItem As New LibraryViewItem(mediaItem)
            libItem.Name = mediaItem.getUuid
            applyFilterToItem(libItem)
            Application.DoEvents()
            If itemsLock.WaitOne() Then
                items.Add(mediaItem.getUuid, libItem)
                Try
                    itemsLock.Release()
                Catch ex As Exception
                End Try
            End If
            Application.DoEvents()
            If layoutItemsFlow.Controls.Count < 300 Then
                layoutItemsFlow.Controls.Add(libItem)
                libItem.Width = libItem.Parent.ClientRectangle.Width - libItem.Parent.Margin.Horizontal
            Else
                logger.warn("LibraryView.addMediaItem: Can't add more MediaItems.")
            End If
        End If
    End Sub

    Private Sub updateMediaItem(ByRef mediaItem As AbstractCasparCGMedia) Handles Library.itemUpdated
        If InvokeRequired Then
            Application.DoEvents()
            If updateBrake.WaitOne() Then
                Invoke(New updateItemDelegate(AddressOf updateMediaItem), {mediaItem})
                Try
                    updateBrake.Release()
                Catch e As Exception
                End Try
            End If
        Else
            Application.DoEvents()
            If items.ContainsKey(mediaItem.getUuid) Then
                'update or add item
                items.Item(mediaItem.getUuid).refreshData()
                applyFilterToItem(items.Item(mediaItem.getUuid))
            Else
                addMediaItem(mediaItem)
            End If
        End If
    End Sub

    Private Sub updateBrakePause() Handles brakeTimer.Elapsed
        If Not isBrakeActive AndAlso updateBrake.WaitOne(brakeTime) Then
        Else
            Try
                updateBrake.Release()
            Catch e As Exception
            End Try
            isBrakeActive = False
        End If
    End Sub

    Private Sub startBrake()
        brakeTimer.Start()
    End Sub

    Private Sub stopBrake()
        brakeTimer.Stop()
        If isBrakeActive Then updateBrakePause()
    End Sub


    '
    '' Ereignis verarbeitung
    '
    Private Sub cmbRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRefresh.Click
        If Not IsNothing(Library) Then
            cmbRefresh.Visible = False
            pbProgress.Visible = True
            Library.refreshLibrary()
            'startBrake()
        End If
    End Sub

    Private Sub pbProgress_click(ByVal sender As Object, ByVal e As EventArgs) Handles pbProgress.Click
        If Not IsNothing(Library) Then
            Library.abortUpdate()
        End If
    End Sub

    Private Sub LibraryView_ClientSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ClientSizeChanged, layoutItemsFlow.ClientSizeChanged
        For Each item As Control In layoutItemsFlow.Controls
            item.Width = item.Parent.ClientRectangle.Width - item.Parent.Margin.Horizontal
        Next
    End Sub

    Private Sub onUpdate() Handles Library.updatedAborted, Library.updated
        If InvokeRequired Then
            Invoke(New updateDelagete(AddressOf onUpdate))
        Else
            pbProgress.Visible = False
            cmbRefresh.Visible = True
            'stopBrake()
        End If
    End Sub

End Class
