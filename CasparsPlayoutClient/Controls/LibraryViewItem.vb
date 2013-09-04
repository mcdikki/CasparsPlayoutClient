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

Public Class LibraryViewItem

    Public Property MediaItem As AbstractCasparCGMedia
    Private cMenu As ContextMenuStrip
    Private Delegate Sub refreshDataDelegate()

    Public Sub New(ByVal mediaItem As AbstractCasparCGMedia)
        Me.MediaItem = mediaItem
        InitializeComponent()
        init()
        initMenu()
    End Sub

    Private Sub init()
        If Not IsNothing(MediaItem) Then
            Name = MediaItem.getFullName
            toolTip.SetToolTip(Me, MediaItem.getFullName)
            toolTip.SetToolTip(Me.lblName, MediaItem.getFullName)
            If MediaItem.getBase64Thumb.Length > 0 Then
                picThumb.Image = ServerController.getBase64ToImage(MediaItem.getBase64Thumb)
            End If
            lblName.Text = MediaItem.getName
            lblType.Text = MediaItem.getMediaType.ToString
            'If MediaItem.containsInfo("Duration") Then
            '    lblDuration.Text = MediaItem.getInfo("Duration")
            'Else
            '    lblDuration.Visible = False
            'End If
            lblDuration.Text = ServerController.getTimeStringOfMS(ServerController.getOriginalMediaDuration(MediaItem))


            'set type icon
            Select Case MediaItem.getMediaType
                Case AbstractCasparCGMedia.MediaType.MOVIE
                    lblType.Image = Image.FromFile("img/media-button-movie.gif")
                Case AbstractCasparCGMedia.MediaType.STILL
                    lblDuration.Visible = False
                    lblType.Image = Image.FromFile("img/media-button-still.gif")
                Case AbstractCasparCGMedia.MediaType.AUDIO
                    lblType.Image = Image.FromFile("img/media-button-audio.gif")
                Case AbstractCasparCGMedia.MediaType.TEMPLATE
                    lblDuration.Visible = False
                    lblType.Image = Image.FromFile("img/media-button-template.gif")
            End Select
        End If
    End Sub

    Private Sub initMenu()
        '' Add ContexMenu
        cMenu = New ContextMenuStrip
        cMenu.Items.Add(New ToolStripMenuItem("Save as XML", Nothing, Sub() saveXml()))
        Me.ContextMenuStrip = cMenu
    End Sub

    Private Sub lblExpand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblExpand.Click
        Dim metadata As String = "" '"Metadata"
        For Each info In MediaItem.getInfos.Keys
            metadata = metadata & info & ": " & MediaItem.getInfo(info) & vbNewLine
        Next

        If MediaItem.getBase64Thumb.Length > 0 Then
            Dim d As New Dialog(metadata, "Metadata for " & MediaItem.getName, ServerController.getBase64ToImage(MediaItem.getBase64Thumb))
            d.ShowDialog()
        Else
            Dim d As New Dialog(metadata, "Metadata for " & MediaItem.getName)
            d.ShowDialog()
        End If
    End Sub

    Public Sub saveXml()
        MediaItem.toXml.save(MediaItem.getName & "(" & MediaItem.getMediaType.ToString & ").xml")
        logger.log("LibraryViewItem.saveXml: Media '" & MediaItem.getName & "' successfully saved.")
    End Sub

    Public Sub refreshData()
        If InvokeRequired Then
            Invoke(New refreshDataDelegate(AddressOf refreshData))
        Else
            If Not IsNothing(MediaItem) Then
                If MediaItem.getBase64Thumb.Length > 0 Then
                    picThumb.Image = ServerController.getBase64ToImage(MediaItem.getBase64Thumb)
                End If

                lblDuration.Text = ServerController.getTimeStringOfMS(ServerController.getOriginalMediaDuration(MediaItem))
            End If
        End If
    End Sub

    '' DragDrop verarbeiten

    Private MouseIsDown As Boolean = False
    Private Sub handleMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown, layoutHeaderInfoPanel.MouseDown, layoutHeaderTable.MouseDown, lblDuration.MouseDown, lblExpand.MouseDown, lblName.MouseDown, lblType.MouseDown
        ' Set a flag to show that the mouse is down. 
        MouseIsDown = True
    End Sub
    Private Sub handleMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove, layoutHeaderInfoPanel.MouseMove, layoutHeaderTable.MouseMove, lblDuration.MouseMove, lblExpand.MouseMove, lblName.MouseMove, lblType.MouseMove
        If MouseIsDown Then
            ' Initiate dragging. 
            DoDragDrop(MediaItem, DragDropEffects.Copy)
        End If
        MouseIsDown = False
    End Sub

End Class
