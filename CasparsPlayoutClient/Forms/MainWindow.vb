Public Class MainWindow

    Private sc As ServerController
    Dim WithEvents playlistView As PlaylistView

    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        sc = New ServerController()
        sc.open("casparcg", 5250)
        AddPlaylist()
        sc.startTicker()
    End Sub

    Private Sub AddPlaylist()

        Dim mediaLib As New Library(sc)
        mediaLib.refreshLibrary()

        Dim p1 As IPlaylistItem
        Dim pp As New PlaylistBlockItem("Paralelle Playlist", sc)
        Dim ps As New PlaylistBlockItem("Seq. Playlist", sc)

        pp.setParallel(True)
        'p2.setAutoStart(True)

        p1 = New PlaylistMovieItem("1. AMB 2-1", sc, mediaLib.getItem("amb").clone, 2, 1)
        ps.addItem(p1)
        p1 = New PlaylistMovieItem("2. cg1080i50 2-1", sc, mediaLib.getItem("cg1080i50").clone, 2, 1)
        ps.addItem(p1)
        p1 = New PlaylistMovieItem("3. AMB 2-1", sc, mediaLib.getItem("amb").clone, 2, 1)
        ps.addItem(p1)

        p1 = New PlaylistMovieItem("P. go1080p25 3-1", sc, mediaLib.getItem("go1080p25").clone, 3, 1)
        'p1.setLooping(True)
        pp.addItem(p1)

        p1 = New PlaylistMovieItem("P. cg1080i50 1-2", sc, mediaLib.getItem("cg1080i50").clone, 1, 2)
        'p1.setLooping(True)
        pp.addItem(p1)


        sc.getPlaylistRoot.addItem(ps)
        sc.getPlaylistRoot.addItem(pp)


        playlistView = New PlaylistView(sc.getPlaylistRoot)
        playlistview.Dock = DockStyle.Fill
        playlistView.Parent = layoutPlaylistSplit.Panel1

        AddHandler sc.getTicker.frameTick, AddressOf onTick
    End Sub

    Private Sub onTick(ByVal sender As Object, ByVal e As frameTickEventArgs)
        playlistView.onDataChanged()
    End Sub
End Class