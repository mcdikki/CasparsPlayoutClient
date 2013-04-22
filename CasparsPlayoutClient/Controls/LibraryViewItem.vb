Public Class LibraryViewItem

    Public Property MediaItem As CasparCGMedia

    Public Sub New(ByVal mediaItem As CasparCGMedia)
        Me.MediaItem = mediaItem
        InitializeComponent()
        Me.Name = mediaItem.getFullName
        Me.toolTip.SetToolTip(Me, mediaItem.getFullName)
        Me.toolTip.SetToolTip(Me.lblName, mediaItem.getFullName)
        Me.toolTip.SetToolTip(Me.cmbAdd, "Add " & mediaItem.getFullName & " to playlist")
        init()
    End Sub

    Private Sub init()
        If Not IsNothing(MediaItem) Then
            lblName.Text = MediaItem.getName
            lblType.Text = MediaItem.getMediaType.ToString
            lblDuration.Text = "00:00:00.00"
        End If
    End Sub

End Class
