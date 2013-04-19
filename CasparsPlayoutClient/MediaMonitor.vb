Public Class MediaMonitor

    Private sc As ServerController

    Public Sub New(ByRef controller As ServerController)
        MyBase.New()
        InitializeComponent()
        Me.sc = controller
    End Sub

    Private Sub MediaMonitor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lsvPlayingMedia.View = View.Details
        With lsvPlayingMedia.Columns
            .Add("Name")
            .Add("Channel")
            .Add("Layer")
            .Add("Laufzeit")
            .Add("Verbleibend")
        End With
        tmUpdater.Start()
    End Sub

    Private Sub tmUpdater_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmUpdater.Tick
        updateView()
    End Sub

    Private Sub updateView()
        'lsvPlayingMedia.Clear()



        'lsvPlayingMedia.Items.Find("Name", False)(0).SubItems.Add("Test")    '   SubItems.Add("Test")
        ''lsvPlayingMedia.Items.Add(header)

        For Each item In sc.getPlaylistRoot.getPlayingChildItems(True, True)
            logger.log(item.getName)
            Dim line As New ListViewItem(item.getName)
            line.Name = item.getName
            With line.SubItems
                .Add(item.getChannel)
                .Add(item.getLayer)
                .Add(item.getDuration)
                .Add(item.getRemaining)
            End With
            If lsvPlayingMedia.Items.ContainsKey(item.getName) Then
                logger.log("remove " & item.getName)
                lsvPlayingMedia.Items(item.getName).Remove()
            End If
            lsvPlayingMedia.Items.Add(line)
        Next
    End Sub
End Class