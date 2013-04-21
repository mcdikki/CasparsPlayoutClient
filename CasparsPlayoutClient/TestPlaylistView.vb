Public Class TestPlaylistView

    Private playlist As IPlaylistItem
    Private childs As List(Of TestPlaylistView)
    Private fullHeight As Integer
    Private Delegate Sub updateDelegate()

    Private Event changedPlaying()
    Public Event dataChanged()

    Public Sub New(ByRef playlist As IPlaylistItem)
        Me.playlist = playlist
        childs = New List(Of TestPlaylistView)
        InitializeComponent()
        init()
    End Sub

    Public Sub onDataChanged() Handles Me.dataChanged
        If Me.InvokeRequired Then
            Dim d As New updateDelegate(AddressOf Me.setData)
            Me.Invoke(d)
        Else
            setData()
        End If
        For Each child In childs
            child.onDataChanged()
        Next
    End Sub

    Private Sub setData()
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
            Me.pbPlayed.Value = .getPlayed
            RaiseEvent changedPlaying()
        End With
    End Sub

    Private Sub init()
        RaiseEvent dataChanged()

        '' ChildLayout füllen
        If playlist.getItemType > -1 Then
            '' Kein BlockItem, also ChildLayout anders fülen
        Else
            '' BlockItem, schauen ob childs geladen werden können
            For Each item In playlist.getChildItems(False)
                Dim child As New TestPlaylistView(item)
                child.Parent = Me.layoutChild
                child.Show()
                childs.Add(child)
            Next
            Me.Height = Me.layoutChild.ClientSize.Height + Me.layoutHeaderContentSplit.Panel1MinSize
        End If
    End Sub

    Friend Sub atResize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        For Each child In childs
            child.Width = Me.layoutChild.ClientRectangle.Width
        Next
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

    Private Sub cmbToggleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbToggleButton.Click
        If playlist.isPlaying Then
            playlist.abort()
        Else
            playlist.start(True)
        End If
        RaiseEvent changedPlaying()
    End Sub

    Friend Sub onChangedPlayingState() Handles Me.changedPlaying
        If playlist.isPlaying Then
            cmbToggleButton.Text = "o"
            layoutInfos.Enabled = False
            layoutButton.Enabled = False
            layoutChannelLayer.Enabled = False
            layoutName.Enabled = False
        Else
            cmbToggleButton.Text = ">"
            layoutInfos.Enabled = True
            layoutButton.Enabled = True
            layoutChannelLayer.Enabled = True
            layoutName.Enabled = True
        End If
        For Each child In childs
            child.onChangedPlayingState()
        Next
    End Sub

    Private Sub ckbParallel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbParallel.CheckedChanged
        playlist.setParallel(ckbParallel.Checked)
    End Sub

    Private Sub ckbAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbAuto.CheckedChanged
        playlist.setAutoStart(ckbAuto.Checked)
    End Sub

    Private Sub ckbLoop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbLoop.CheckedChanged
        playlist.setLooping(ckbLoop.Checked)
    End Sub

    Private Sub nudLayer_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLayer.ValueChanged
        playlist.setLayer(nudLayer.Value)
    End Sub

    Private Sub nudChannel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudChannel.ValueChanged
        playlist.setChannel(nudChannel.Value)
    End Sub

    Private Sub txtDelay_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDelay.TextChanged
        playlist.setDelay(TimeSpan.Parse(txtDelay.Text).TotalMilliseconds)
    End Sub

    Private Sub txtName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.Leave
        playlist.setName(txtName.Text)
    End Sub

End Class
