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

Public Class PlaylistView

    Private isInit As Boolean = False
    Private playlist As IPlaylistItem
    Private childs As List(Of PlaylistView)
    Private startCompact As Boolean
    Private Delegate Sub updateDelegate()
    Private cMenu As ContextMenuStrip
    Private nowarn As Integer = 5000
    Private warn As Integer = 1000
    Private lastWidth As Integer = 0
    Private lastHeight As Integer = 0

    Private updateItems As New Threading.Semaphore(1, 1)

    Private Event changedPlaying()
    Public Event dataChanged()
    Public Event changedHeight(ByVal heightDif As Integer)

    Public Sub New(ByRef playlist As IPlaylistItem, Optional ByVal startCompact As Boolean = False)
        Me.playlist = playlist
        Me.startCompact = startCompact
        childs = New List(Of PlaylistView)
        InitializeComponent()
        isInit = True
        Dim imgList As New ImageList
        imgList.ImageSize = New Size(24, 24)
        imgList.ColorDepth = 32
        imgList.Images.Add(Image.FromFile("img/Play-Green-Button-icon.png"))
        imgList.Images.Add(Image.FromFile("img/Stop-Red-Button-icon.png"))
        imgList.Images.Add(Image.FromFile("img/Play-Blue-Button-icon.png"))
        cmbToggleButton.ImageList = imgList  

        imgList = New ImageList
        imgList.ImageSize = New Size(13, 12)
        imgList.Images.Add(Image.FromFile("img/media-button-block.gif"))
        imgList.Images.Add(Image.FromFile("img/media-button-movie.gif"))
        imgList.Images.Add(Image.FromFile("img/media-button-still.gif"))
        imgList.Images.Add(Image.FromFile("img/media-button-audio.gif"))
        imgList.Images.Add(Image.FromFile("img/media-button-template.gif"))
        imgList.Images.Add(Image.FromFile("img/media-button-cmd.gif"))
        lblExpand.ImageList = imgList

        init()
        initMenu()
    End Sub

    Private Sub init()
        RaiseEvent dataChanged()

        layoutHeaderContentSplit_DoubleClick(Nothing, Nothing)

        '' ChildLayout füllen
        Select Case playlist.getItemType
            Case AbstractPlaylistItem.PlaylistItemTypes.MOVIE
                ' set default behaviour and view
                lblExpand.ImageIndex = 1
                ckbAuto.Checked = True
                ckbParallel.Enabled = False
                AddHandler txtDuration.GotFocus, AddressOf txtDuration_GotFocus
                AddHandler txtDuration.LostFocus, AddressOf txtDuration_LostFocus

                ' load thumbnail
                Dim thumb As New PictureBox()
                If playlist.getMedia.getBase64Thumb.Length > 0 Then
                    thumb.Image = ServerController.getBase64ToImage(playlist.getMedia.getBase64Thumb)
                End If
                thumb.Dock = DockStyle.Fill
                thumb.SizeMode = PictureBoxSizeMode.AutoSize
                thumb.Parent = Me.layoutChild
                thumb.Show()
            Case AbstractPlaylistItem.PlaylistItemTypes.AUDIO
                ' set default behaviour and view
                lblExpand.ImageIndex = 3
                ckbAuto.Checked = True
                ckbParallel.Enabled = False
            Case AbstractPlaylistItem.PlaylistItemTypes.STILL
                ' set default behaviour and view
                lblExpand.ImageIndex = 2
                ckbAuto.Checked = True
                ckbParallel.Enabled = False
                ckbLoop.Enabled = False
                AddHandler txtDuration.GotFocus, AddressOf txtDuration_GotFocus
                AddHandler txtDuration.LostFocus, AddressOf txtDuration_LostFocus

                ' load thumbnail
                Dim thumb As New PictureBox()
                If playlist.getMedia.getBase64Thumb.Length > 0 Then
                    thumb.Image = ServerController.getBase64ToImage(playlist.getMedia.getBase64Thumb)
                End If
                thumb.Dock = DockStyle.Fill
                thumb.SizeMode = PictureBoxSizeMode.AutoSize
                thumb.Parent = Me.layoutChild
                thumb.Show()

            Case AbstractPlaylistItem.PlaylistItemTypes.TEMPLATE
                ' set default behaviour and view
                lblExpand.ImageIndex = 4
            Case AbstractPlaylistItem.PlaylistItemTypes.COMMAND
                ' set default behaviour and view
                lblExpand.ImageIndex = 5
                txtDuration.ReadOnly = True
                txtDuration.BackColor = Color.White
                txtDuration.TabStop = False
            Case AbstractPlaylistItem.PlaylistItemTypes.BLOCK
                ' set default behaviour and view
                lblExpand.ImageIndex = 0
                txtDuration.ReadOnly = True
                txtDuration.BackColor = Color.White
                txtDuration.TabStop = False

                '' BlockItem, schauen ob childs geladen werden können
                For Each item In playlist.getChildItems(False)
                    addChild(item)
                Next
        End Select
        layoutHeaderContentSplit_DoubleClick(Nothing, Nothing)
        If startCompact Then layoutHeaderContentSplit_DoubleClick(Nothing, Nothing)

        AddHandler playlist.changed, Sub() onDataChanged()
    End Sub

    Private Sub initMenu()
        '' Add ContexMenu
        cMenu = New ContextMenuStrip


        cMenu.Items.Add(New ToolStripMenuItem("Save Playlist", Nothing, Sub() playlist.toXML.save(playlist.getName & "(PLAYLIST).xml")))
        cMenu.Items.Add(New ToolStripMenuItem("Load Playlist", Nothing, Sub() loadPlaylist()))
        cMenu.Items.Add(New ToolStripSeparator)
        cMenu.Items.Add(New ToolStripMenuItem("Add Playlist(s) from file", Nothing, Sub() addPlaylist()))
        cMenu.Items.Add(New ToolStripMenuItem("Add Block", Nothing, New EventHandler(AddressOf addBlockItem)))
        Dim sMenu = New ToolStripMenuItem("Add Command")
        sMenu.Name = "Add Command Menu"
        sMenu.Text = "Add Command"
        sMenu.DropDownItems.Add(New ToolStripMenuItem("Add ClearCommand", Nothing, New EventHandler(Sub() addCommandItem(CasparCGCommandFactory.Command.ClearCommand))))
        sMenu.DropDownItems.Add(New ToolStripMenuItem("Add StopCommand", Nothing, New EventHandler(Sub() addCommandItem(CasparCGCommandFactory.Command.StopCommand))))
        cMenu.Items.Add(sMenu)
        cMenu.Items.Add(New ToolStripSeparator)
        cMenu.Items.Add(New ToolStripMenuItem("Remove item", Nothing, New EventHandler(AddressOf removeItem)))

        Me.ContextMenuStrip = cMenu
    End Sub

    Private Sub loadPlaylist()
        Dim fd As New OpenFileDialog()
        fd.DefaultExt = "xml"
        fd.Filter = "Xml Dateien|*.xml"
        fd.CheckFileExists = True
        fd.Multiselect = False
        fd.ShowDialog()

        Dim domDoc As New MSXML2.DOMDocument
        If domDoc.load(fd.FileName) Then
            If domDoc.firstChild.nodeName.Equals("playlist") Then
                Dim pl = PlaylistFactory.getPlaylist(domDoc, playlist.getController)
                If Not IsNothing(pl) Then
                    If IsNothing(playlist.getParent) Then
                        If pl.getItemType.Equals(AbstractPlaylistItem.PlaylistItemTypes.BLOCK) Then
                            playlist.loadXML(domDoc)
                            layoutChild.Controls.Clear()
                            childs.Clear()
                            init()
                        Else
                            logger.warn("PlaylistView.loadPlaylist: Unable to load playlist from xml. The root playlist can only be replaced by a Block type playlist. Try add playlist instead.")
                        End If
                    Else
                        ' change playlist and create new playlist view
                        playlist.insertChildAt(pl, playlist)
                        playlist.getParent.removeChild(playlist)

                        Dim child As New PlaylistView(pl, startCompact)

                        Me.Parent.Controls.Add(child)
                        Me.Parent.Controls.SetChildIndex(child, Me.Parent.Controls.IndexOf(child))
                        Me.Parent.Controls.Remove(Me)
                    End If
                    RaiseEvent dataChanged()
                    atResize(Me, Nothing)
                End If
            Else
                logger.warn("PlaylistView.loadPlaylist: Unable to load playlist from xml file " & fd.FileName & ". Xml definition is not valid.")
            End If
        Else
            logger.warn("PlaylistView.loadPlaylist: Unable to xml file " & fd.FileName)
        End If
    End Sub

    Public Sub addPlaylist()
        Dim fd As New OpenFileDialog()
        fd.DefaultExt = "xml"
        fd.Filter = "Xml Dateien|*.xml"
        fd.CheckFileExists = True
        fd.Multiselect = True
        fd.ShowDialog()

        Dim domDoc As New MSXML2.DOMDocument
        For Each f In fd.FileNames
            If domDoc.load(f) Then
                If domDoc.firstChild.nodeName.Equals("playlist") Then
                    Dim pl = PlaylistFactory.getPlaylist(domDoc, playlist.getController)
                    If Not IsNothing(pl) Then
                        playlist.addItem(pl)
                        addChild(pl)
                    End If
                Else
                    logger.warn("PlaylistView.loadPlaylist: Unable to load playlist from xml file " & fd.FileName & ". Xml definition is not valid.")
                End If
            Else
                logger.warn("PlaylistView.loadPlaylist: Unable to xml file " & fd.FileName)
            End If
        Next
    End Sub

    Public Sub onDataChanged() Handles Me.dataChanged
        If Me.InvokeRequired Then
            Dim d As New updateDelegate(AddressOf Me.onDataChanged)
            Me.Invoke(d)
        Else
            setData()
            '' if we have to wait more than a half frame (25fps), we'll drop that update
            If updateItems.WaitOne(20) Then
                For Each child In childs
                    child.onDataChanged()
                Next
                updateItems.Release()
            End If
        End If
    End Sub

    Private Sub setData()
        '' Werte eintragen
        With playlist

            If Not txtName.Focused Then Me.txtName.Text = .getName
            Me.nudChannel.Value = Math.Max(.getChannel, 0)
            Me.nudLayer.Value = Math.Max(.getLayer, -1)
            If Not txtDuration.Focused Then Me.txtDuration.Text = ServerController.getTimeStringOfMS(.getDuration)
            If Not txtDelay.Focused Then Me.txtDelay.Text = ServerController.getTimeStringOfMS(.getDelay)
            Me.ckbAuto.Checked = .isAutoStarting
            Me.ckbParallel.Checked = .isParallel
            Me.ckbLoop.Checked = .isLooping

            If playlist.getController.isOpen Then
                Me.txtPosition.Text = ServerController.getTimeStringOfMS(.getPosition)
                Me.txtRemaining.Text = ServerController.getTimeStringOfMS(.getRemaining)
                Select Case .getRemaining
                    Case Is < 1
                        txtRemaining.BackColor = Color.White
                    Case Is < warn
                        txtRemaining.BackColor = Color.Red
                    Case Is < nowarn
                        txtRemaining.BackColor = Color.Yellow
                    Case Else
                        txtRemaining.BackColor = Color.White
                End Select
                Me.pbPlayed.Value = .getPlayed
            Else
                Me.txtPosition.Text = ServerController.getTimeStringOfMS(0)
                Me.txtRemaining.Text = ServerController.getTimeStringOfMS(.getDuration)
                txtRemaining.BackColor = Color.White
                Me.pbPlayed.Value = 0
            End If
        End With
        RaiseEvent changedPlaying()
    End Sub

    Private Sub addChild(ByRef childList As IPlaylistItem)
        Dim child As New PlaylistView(childList, startCompact)
        child.Parent = Me.layoutChild
        child.Show()
        updateItems.WaitOne()
        childs.Add(child)
        updateItems.Release()
        AddHandler child.changedHeight, AddressOf atChildChangedHeight
        atResize(Me, Nothing)
    End Sub

    Private Sub atChildChangedHeight(ByVal heightDif As Integer)
        If Not IsNothing(playlist.getParent) Then
            Me.layoutChild.Height = Me.layoutChild.Height + heightDif
            Me.Height = Me.Height + heightDif
        End If
    End Sub

    Friend Sub atResize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        If Not Me.Height = lastHeight AndAlso Not IsNothing(playlist.getParent) Then
            RaiseEvent changedHeight(Me.Height - lastHeight)
            lastHeight = Me.Height
        End If

        If Not Me.layoutChild.ClientRectangle.Width = lastWidth Then
            lastWidth = Me.layoutChild.ClientRectangle.Width
            updateItems.WaitOne()
            For Each child In childs
                ' Let child use the whole width
                child.Width = Me.layoutChild.ClientRectangle.Width
            Next
            updateItems.Release()
        End If
    End Sub

    Private Sub layoutHeaderContentSplit_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblExpand.Click, layoutHeaderContentSplit.DoubleClick
        If layoutHeaderContentSplit.Panel2Collapsed Then
            lblExpand.Text = "-"
            If layoutChild.HasChildren Then
                Me.Height = layoutHeaderContentSplit.Panel1.Height + layoutChild.Height + (2 * layoutHeaderContentSplit.Panel2.Padding.Vertical) + (2 * layoutHeaderContentSplit.Panel2.Margin.Vertical) + 10
            Else
                Me.Height = layoutHeaderContentSplit.Panel1.Height + layoutHeaderContentSplit.Panel2MinSize + (2 * layoutHeaderContentSplit.Panel2.Padding.Vertical) + (2 * layoutHeaderContentSplit.Panel2.Margin.Vertical) '+ 10
            End If
        Else
            Me.Height = layoutHeaderContentSplit.Panel1MinSize
            lblExpand.Text = "+"
        End If
        layoutHeaderContentSplit.Panel2Collapsed = Not layoutHeaderContentSplit.Panel2Collapsed
    End Sub

    Private Sub cmbToggleButton_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmbToggleButton.Click
        If playlist.getController.isConnected Then
            If ModifierKeys = Keys.Control Then
                logger.debug("GUI PlaylistView: HardStop requested at " & Me.txtName.Text)
                playlist.abort()
            ElseIf playlist.isWaiting Then
                playlist.playNextItem()
            ElseIf playlist.isPlaying Then
                playlist.halt()
            Else
                ' Damit nicht gewartet wird falls der button manuel betätigt wurde aber auto nicht gesetzt ist
                If playlist.getController.containsChannel(playlist.getChannel) OrElse Not playlist.isPlayable Then
                    If playlist.isAutoStarting Then
                        playlist.start()
                    Else
                        playlist.playNextItem()
                    End If
                Else
                    MsgBox("Error, unknown channel.")
                End If
            End If
            RaiseEvent changedPlaying()
        End If
    End Sub

    Friend Sub onChangedPlayingState() Handles Me.changedPlaying
        If playlist.isPlaying And Not playlist.isWaiting Then
            txtName.BackColor = Color.Orange
            layoutContentSplit.Panel1.BackColor = Color.Orange
            cmbToggleButton.ImageIndex = 1
            layoutButton.Enabled = False
            nudChannel.Enabled = False
            nudLayer.Enabled = False
            txtName.ReadOnly = True
            If Not IsNothing(Me.ContextMenuStrip) Then Me.ContextMenuStrip.Enabled = False
        ElseIf playlist.isWaiting Then
            txtName.BackColor = Color.LightBlue
            layoutContentSplit.Panel1.BackColor = Color.LightBlue
            cmbToggleButton.ImageIndex = 2
            layoutButton.Enabled = False
            nudChannel.Enabled = False
            nudLayer.Enabled = False
            txtName.ReadOnly = True
            If Not IsNothing(Me.ContextMenuStrip) Then Me.ContextMenuStrip.Enabled = False
        Else
            txtName.BackColor = Color.LightGreen
            layoutContentSplit.Panel1.BackColor = Color.LightGreen
            cmbToggleButton.ImageIndex = 0
            layoutButton.Enabled = True
            nudChannel.Enabled = True
            nudLayer.Enabled = True
            txtName.ReadOnly = False
            If Not IsNothing(Me.ContextMenuStrip) Then Me.ContextMenuStrip.Enabled = True
        End If
    End Sub

    Private Sub ckbParallel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbParallel.CheckedChanged
        If isInit Then
            playlist.setParallel(ckbParallel.Checked)
        End If
    End Sub

    Private Sub ckbAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbAuto.CheckedChanged
        If isInit Then
            playlist.setAutoStart(ckbAuto.Checked)
        End If
    End Sub

    Private Sub ckbLoop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbLoop.CheckedChanged
        If isInit Then
            playlist.setLooping(ckbLoop.Checked)
        End If
    End Sub

    Private Sub nudLayer_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLayer.ValueChanged
        ' Test no check, just show wrong
        'If isInit Then
        If nudLayer.Value < 0 Then
            nudLayer.BackColor = Color.Red
        Else
            nudLayer.BackColor = Color.White
        End If
        playlist.setLayer(nudLayer.Value)
        'End If

        'If isInit Then
        '    If nudLayer.Value < 0 Then
        '        nudLayer.BackColor = Color.Red
        '        playlist.setLayer(-1)
        '    Else
        '        nudLayer.BackColor = Color.White
        '        playlist.setLayer(nudLayer.Value)
        '    End If
        'End If
    End Sub

    Private Sub nudChannel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudChannel.ValueChanged

        If nudChannel.Value < 1 OrElse (playlist.getController.isOpen AndAlso nudChannel.Value > playlist.getController.getChannels) Then
            nudChannel.BackColor = Color.Red
        Else
            nudChannel.BackColor = Color.White
        End If
        playlist.setChannel(nudChannel.Value)

        'If isInit Then
        '    If nudChannel.Value < 1 OrElse Not playlist.getController.containsChannel(nudChannel.Value) Then
        '        nudChannel.BackColor = Color.Red
        '        playlist.setChannel(-1)
        '    Else
        '        nudChannel.BackColor = Color.White
        '        playlist.setChannel(nudChannel.Value)
        '    End If
        'End If
    End Sub

    Private Sub txtDelay_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDelay.LostFocus
        playlist.setDelay(Long.Parse(txtDelay.Text))
        txtDelay.Text = ServerController.getTimeStringOfMS(playlist.getDelay)
    End Sub

    Private Sub txtDelay_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDelay.GotFocus
        txtDelay.Text = playlist.getDelay
    End Sub

    ' Handles ENTER to leave the textbox
    Private Sub txtBox_Ok(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtDelay.KeyDown, txtDuration.KeyDown, txtName.KeyDown, nudLayer.KeyDown, nudChannel.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.SelectNextControl(sender, True, True, True, True)
        End If
    End Sub

    Private Sub txtName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.LostFocus
        playlist.setName(txtName.Text)
    End Sub

    Private Sub txtDuration_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        playlist.setDuration(Long.Parse(txtDuration.Text))
        txtDuration.Text = ServerController.getTimeStringOfMS(playlist.getDuration)
    End Sub

    Private Sub txtDuration_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        txtDuration.Text = playlist.getDuration
    End Sub

    ''
    '' Hinzufügen / entfernen von Blöcken
    ''

    Public Sub addBlockItem()
        If playlist.getItemType = AbstractPlaylistItem.PlaylistItemTypes.BLOCK Then
            Dim bi As New PlaylistBlockItem("BlockItem", playlist.getController)
            playlist.addItem(bi)
            addChild(bi)
        End If
    End Sub

    Public Sub addCommandItem(ByVal command As CasparCGCommandFactory.Command)
        If playlist.getItemType = AbstractPlaylistItem.PlaylistItemTypes.BLOCK Then
            Dim ci As New PlaylistCommandItem(command.ToString, playlist.getController, CasparCGCommandFactory.getCommand(command))
            ci.setAutoStart(True)
            playlist.addItem(ci)
            addChild(ci)
        End If
    End Sub

    Public Sub removeItem()
        If Not IsNothing(playlist.getParent) Then
            playlist.getParent.removeChild(playlist)
            Me.Parent.Controls.Remove(Me)
        Else
            Me.layoutChild.Controls.Clear()
            childs.Clear()
            For Each child In playlist.getChildItems()
                playlist.removeChild(child)
            Next
        End If
    End Sub


    ''
    '' DragDrop verarbeiten
    '
    Private Overloads Sub handleDragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles Me.DragDrop
        If e.Data.GetDataPresent("CasparCGNETConnector.CasparCGMovie") Then
            ''
            '' Neue MediaItems einfügen
            ''
            Dim media As AbstractCasparCGMedia = e.Data.GetData("CasparCGNETConnector.CasparCGMovie")
            Dim child As IPlaylistItem
            child = New PlaylistMovieItem(media.getFullName, playlist.getController, media.clone)
            playlist.addItem(child)
            addChild(child)
        ElseIf e.Data.GetDataPresent("CasparCGNETConnector.CasparCGTemplate") Then
            ''
            '' Neue MediaItems einfügen
            ''
            Dim media As AbstractCasparCGMedia = e.Data.GetData("CasparCGNETConnector.CasparCGTemplate")
            Dim child As IPlaylistItem
            child = New PlaylistTemplateItem(media.getFullName, playlist.getController, media.clone)
            'child = New PlaylistBlockItem("not implemented yet", playlist.getController)
            'addChild(child)
        ElseIf e.Data.GetDataPresent("CasparCGNETConnector.CasparCGStill") Then
            ''
            '' Neue MediaItems einfügen
            ''
            Dim media As AbstractCasparCGMedia = e.Data.GetData("CasparCGNETConnector.CasparCGStill")
            Dim child As IPlaylistItem
            child = New PlaylistStillItem(media.getFullName, playlist.getController, media.clone)
            playlist.addItem(child)
            addChild(child)
        ElseIf e.Data.GetDataPresent("CasparCGNETConnector.CasparCGAudio") Then
            ''
            '' Neue MediaItems einfügen
            ''
            Dim media As AbstractCasparCGMedia = e.Data.GetData("CasparCGNETConnector.CasparCGAudio")
            Dim child As IPlaylistItem
            child = New PlaylistAudioItem(media.getFullName, playlist.getController, media.clone)
            'child = New PlaylistBlockItem("not implemented yet", playlist.getController)
            'playlist.addItem(child)
            'addChild(child)
        ElseIf e.Data.GetDataPresent("CasparsPlayoutClient.PlaylistView") Then
            ''
            '' PlaylistItems verschieben
            ''
            Dim item As PlaylistView = e.Data.GetData("CasparsPlayoutClient.PlaylistView")
            If IsNothing(item.playlist.getParent) Then
                logger.warn("Playlist " + item.playlist.getName + " has no parent assigned.")
            Else
                If item.Equals(Me) Then
                    logger.warn("Playlist " + item.playlist.getName + " Can't move or add me to myself.")
                Else
                    If IsNothing(playlist.getParent) OrElse ModifierKeys = Keys.Control Then
                        ' oder, wenn es auf den Freiraum der neuen liste 
                        ' Playlist von seiner alten liste lösen
                        item.playlist.getParent.removeChild(item.playlist)

                        playlist.addItem(item.playlist)
                        item.Parent = Me.layoutChild
                    Else
                        ' und an den platz dieser Playlist in dem Vater einfügen
                        playlist.getParent.insertChildAt(item.playlist, playlist)

                        'jetzt noch die Controls entsprechend verschieben.
                        Dim p As Control = Me.Parent
                        Dim oldItemP As Control = item.Parent
                        Dim meIndex = p.Controls.GetChildIndex(Me)
                        Dim oldItemIndex = item.Parent.Controls.GetChildIndex(item)

                        p.Controls.Add(item)
                        p.Controls.SetChildIndex(item, meIndex)
                        If p.Controls.GetChildIndex(Me) < meIndex AndAlso oldItemP.Equals(p) AndAlso oldItemIndex < meIndex AndAlso Not meIndex = 1 Then
                            ' wrong, we want it under the new  item
                            p.Controls.SetChildIndex(item, p.Controls.GetChildIndex(Me))
                        End If

                    End If
                End If
            End If
        End If
    End Sub

    Private Overloads Sub handleDragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles Me.DragEnter
        ' Check the format of the data being dropped. 
        If playlist.getItemType = AbstractPlaylistItem.PlaylistItemTypes.BLOCK AndAlso (e.Data.GetDataPresent("CasparCGNETConnector.CasparCGMovie")) OrElse e.Data.GetDataPresent("CasparCGNETConnector.CasparCGStill") Then 'OrElse e.Data.GetDataPresent("CasparCGNETConnector.CasparCGTemplate") OrElse e.Data.GetDataPresent("CasparCGNETConnector.CasparCGAudio") Then
            ' Display the copy cursor. 
            e.Effect = DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent("CasparsPlayoutClient.PlaylistView") Then
            If ModifierKeys = Keys.Control Then
                If playlist.getItemType = AbstractPlaylistItem.PlaylistItemTypes.BLOCK Then
                    e.Effect = DragDropEffects.Copy
                Else
                    e.Effect = DragDropEffects.None
                End If
            Else
                e.Effect = DragDropEffects.Move
            End If
        Else
            ' Display the no-drop cursor. 
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private MouseIsDown As Boolean = False
    Private Sub handleMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, layoutButton.MouseDown, layoutContentSplit.MouseDown, layoutInfos.MouseDown, layoutName.MouseDown, layoutHeaderTable.MouseDown
        ' Set a flag to show that the mouse is down. 
        MouseIsDown = True
    End Sub
    Private Sub handleMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove, layoutButton.MouseMove, layoutContentSplit.MouseMove, layoutInfos.MouseMove, layoutName.MouseMove, layoutHeaderTable.MouseMove
        If MouseIsDown Then
            ' Initiate dragging. 
            If ModifierKeys = Keys.Control Then
                DoDragDrop(Me, DragDropEffects.Copy)
            Else
                DoDragDrop(Me, DragDropEffects.Move)
            End If

        End If
        MouseIsDown = False
    End Sub

End Class
