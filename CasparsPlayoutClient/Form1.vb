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
        Threading.Thread.Sleep(2000)
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

    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        sc.close()
        MyBase.Finalize()
    End Sub
End Class
