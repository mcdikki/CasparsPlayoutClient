Public Class Form1
    Dim con As CasparCGConnection
    Dim sc As ServerController

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        logger.debug("Try to connect casparCG server..")
        con = New CasparCGConnection("localhost", 5250)
        sc = New ServerController()
        sc.open()
        logger.debug("Connected to server")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        logger.log(sc.getFPS(2))
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.removeLogAction(logger.stdLogAction)
        logger.addLogAction(New consoleLogger(4))
        logger.log("Console Logger startet")
    End Sub
End Class
