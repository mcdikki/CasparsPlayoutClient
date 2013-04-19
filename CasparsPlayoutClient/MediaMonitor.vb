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
        For Each item In sc.getPlaylistRoot.getPlayingChildItems(True, True)
            logger.log(item.toString)
            Dim line As New ListViewItem(item.getName)
            line.Name = item.toString
            With line.SubItems
                .Add(item.getChannel)
                .Add(item.getLayer)
                .Add(item.getDuration)
                .Add(item.getRemaining)
            End With
            If lsvPlayingMedia.Items.ContainsKey(item.toString) Then
                logger.log("remove " & item.toString)
                lsvPlayingMedia.Items(item.toString).Remove()
            End If
            lsvPlayingMedia.Items.Add(line)
        Next
    End Sub
End Class