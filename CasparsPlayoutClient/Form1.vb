Public Class Form1
    Dim con As CasparCGConnection
    Dim sc As ServerController

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        logger.debug("Try to connect casparCG server..")
        con = New CasparCGConnection("localhost", 5250)
        'con.connect()
        sc = New ServerController()
        sc.open()
        'logger.debug("Connected to server")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim mediaLib As New Library(sc)
        logger.log("Refreshing Library")
        mediaLib.refreshLibrary()
        logger.log("LIBRARY:")
        For Each media As CasparCGMedia In mediaLib.getCasparCGMedia
            logger.log(media.getFullName & "(" & media.getMediaType.ToString & ")")
        Next

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.addLogAction(New consoleLogger(4))
        logger.log("Console Logger startet")
    End Sub
End Class
