Public Class TestPlaylistView

    Private playlist As IPlaylistItem
    Private childs As List(Of TestPlaylistView)
    Private fullHeight As Integer

    Public Sub New(ByRef playlist As IPlaylistItem)
        Me.playlist = playlist
        childs = New List(Of TestPlaylistView)
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
                Dim child As New TestPlaylistView(item)
                child.Dock = DockStyle.Fill
                child.Width = Me.layoutContentSplit.Panel2.Width
                child.Parent = Me.childLayout
                child.Show()
                childs.Add(child)
            Next
        End If
        If childs.Count > 0 Then logger.log("My Content width: " & Me.childLayout.Width & "   childs width: " & childs.Item(0).Width)

    End Sub

    Private Sub layoutHeaderContentSplit_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblExpand.Click, layoutHeaderContentSplit.DoubleClick
        If layoutHeaderContentSplit.Panel2Collapsed Then
            lblExpand.Text = "+"
            Me.Height = fullHeight
        Else
            fullHeight = Me.Height
            Me.Height = layoutHeaderContentSplit.Panel1MinSize
            lblExpand.Text = "-"
        End If
        layoutHeaderContentSplit.Panel2Collapsed = Not layoutHeaderContentSplit.Panel2Collapsed
    End Sub

End Class
