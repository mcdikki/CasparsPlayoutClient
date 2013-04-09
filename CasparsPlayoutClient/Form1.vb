Public Class Form1
    Dim sc As ServerController

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        logger.debug("Try to connect casparCG server..")
        sc = New ServerController()
        sc.open()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim mediaLib As New Library(sc)
        logger.log("Refreshing Library")
        mediaLib.refreshLibrary()
        logger.log("LIBRARY:")
        For Each media As CasparCGMedia In mediaLib.getItems
            logger.log(media.getFullName & " (" & media.getMediaType.ToString & ")")
        Next

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.addLogAction(New consoleLogger(4))
        logger.log("Console Logger startet")
    End Sub
End Class
