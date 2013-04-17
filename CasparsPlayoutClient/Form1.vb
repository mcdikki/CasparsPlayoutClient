Imports System.Threading

Public Class Form1

    Private sc As ServerController
    Private worker As Thread
    Private nf As Integer = 0

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        logger.debug("Try to connect casparCG server..")
        If IsNothing(sc) Then
            sc = New ServerController()
        End If
        If Not sc.isOpen() Then
            sc.open("casparcg", 5250)
        End If

        Thread.Sleep(1000)

        testPlaylist()
        openListener()

    End Sub

    Private Sub testPlaylist()
        Dim mediaLib As New Library(sc)
        mediaLib.refreshLibrary()

        Dim p1 As IPlaylistItem
        Dim p2 As New PlaylistBlockItem("P2_SEQ", sc)

        'p1.setChannel(1)
        'p2.setChannel(1)
        'p1.setLayer(1)
        'p2.setLayer(1)
        'p1.setParallel(False)
        'p2.setParallel(True)
        'p1.setAutoStart(True)
        'p2.setAutoStart(True)

        'p1.addItem(New PlaylistMovieItem("1", sc, mediaLib.getItem("amb")))
        'p1.addItem(New PlaylistMovieItem("2", sc, mediaLib.getItem("cg1080i50")))
        'p1.addItem(New PlaylistMovieItem("3", sc, mediaLib.getItem("go1080p25")))

        p1 = New PlaylistMovieItem("go1080p25", sc, mediaLib.getItem("go1080p25"), 3, 1)
        p1.setLooping(True)
        p2.addItem(p1)

        p1 = New PlaylistMovieItem("cg1080i50", sc, mediaLib.getItem("cg1080i50"), 1, 2)
        p1.setLooping(True)
        p2.addItem(p1)


        'sc.getPlaylistRoot.addItem(p1)
        sc.getPlaylistRoot.addItem(p2)
        sc.startTicker()
        'openListener()

        p2.start()
        'sc.getPlaylistRoot.start(True)

    End Sub

    Private Sub openListener()
        Dim f2 As Form2
        For i = nf To Integer.Parse(txtListenerCount.Text) - 1
            nf = nf + 1
            f2 = New Form2(sc)
            f2.Text = nf
            f2.Show()
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'sc.toggleTickerActive()
        worker = New Thread(AddressOf doIt)
        worker.Start()
    End Sub

    Private Sub doIt()
        Dim mediaLib As New Library(sc)
        logger.log("Refreshing Library")
        mediaLib.refreshLibrary()
        logger.log("LIBRARY:")
        For Each media As CasparCGMedia In mediaLib.getItems
            logger.log(media.toString)
        Next
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.addLogAction(New consoleLogger(3))
        logger.log("Console Logger startet")
    End Sub

    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        sc.close()
        MyBase.Finalize()
    End Sub

End Class
