Public Class PlaylistView

    Private playlist As IPlaylistItem

    Public Sub New(ByRef playlist As IPlaylistItem)
        Me.playlist = playlist
        InitializeComponent()
        init()
    End Sub

    Private Sub init()
        '' Werte eintragen
        With playlist
            Me.txtName.Text = .getName
            Me.nudChannel.Value = .getChannel
            Me.nudLayer.Value = .getLayer
            Me.txtPosition.Text = .getPosition
            Me.txtDuration.Text = .getDuration
            Me.txtDelay.Text = .getDelay
            Me.ckbAuto.Checked = .isAutoStarting
            Me.ckbParallel.Checked = .isParallel
            Me.ckbLoop.Checked = .isLooping
            If .isPlaying Then
                cmbToggleButton.Text = "o"
            Else
                cmbToggleButton.Text = ">"
            End If
        End With

        '' ChildLayout füllen
        If playlist.getItemType > -1 Then
            '' Kein BlockItem, also ChildLayout anders fülen
        Else
            '' BlockItem, schauen ob childs geladen werden können
            For Each item In playlist.getChildItems(False)
                Dim child As New PlaylistView(item)
                child.Parent = Me.childLayout
                child.Show()
            Next
        End If

    End Sub

End Class
