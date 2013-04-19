Public Class MediaMonitor

    Private sc As ServerController
    Delegate Sub updateDelegate()

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
            .Add("% gespielt")
        End With
        AddHandler sc.getTicker.frameTick, AddressOf tmUpdater_Tick
        'tmUpdater.Start()
    End Sub

    Private Sub tmUpdater_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmUpdater.Tick
        If lsvPlayingMedia.InvokeRequired Then
            Dim d As New updateDelegate(AddressOf updateView)
            Me.Invoke(d, New Object() {})
        Else
            updateView()
        End If
    End Sub

    Private Sub updateView()
        lsvPlayingMedia.Items.Clear()
        For Each item In sc.getPlaylistRoot.getPlayingChildItems(True, True)
            Dim line As New ListViewItem(item.getName)
            line.Name = item.toString
            With line.SubItems
                .Add(item.getChannel)
                .Add(item.getLayer)
                .Add(item.getDuration)
                .Add(item.getRemaining)
                .Add(item.getPlayed)
            End With
            If lsvPlayingMedia.Items.ContainsKey(item.toString) Then
                lsvPlayingMedia.Items(item.toString).Remove()
            End If
            lsvPlayingMedia.Items.Add(line)
        Next
    End Sub
End Class