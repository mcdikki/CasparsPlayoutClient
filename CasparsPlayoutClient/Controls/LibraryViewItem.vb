Public Class LibraryViewItem

    Public Property MediaItem As CasparCGMedia

    Public Sub New(ByVal mediaItem As CasparCGMedia)
        Me.MediaItem = mediaItem
        InitializeComponent()

        init()
    End Sub

    Private Sub init()
        If Not IsNothing(MediaItem) Then
            Name = MediaItem.getFullName
            toolTip.SetToolTip(Me, MediaItem.getFullName)
            toolTip.SetToolTip(Me.lblName, MediaItem.getFullName)
            toolTip.SetToolTip(Me.cmbAdd, "Add " & MediaItem.getFullName & " to playlist")
            lblName.Text = MediaItem.getName
            lblType.Text = MediaItem.getMediaType.ToString
            If MediaItem.containsInfo("Duration") Then
                lblDuration.Text = MediaItem.getInfo("Duration")
            Else
                lblDuration.Visible = False
            End If
        End If
    End Sub

End Class
