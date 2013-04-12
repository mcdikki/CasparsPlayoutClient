Imports System.Threading

Public Class Form1

    Private sc As ServerController
    Private worker As Thread
    Private listener As New List(Of placeholder)

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        logger.debug("Try to connect casparCG server..")
        If IsNothing(sc) Then
            sc = New ServerController()
        End If
        Dim f2 As New Form2(sc)
        'For i = 0 To 250
        '    listener.Add(New placeholder)
        '    sc.registerFrameTickListener(listener.Item(i))
        'Next
        sc.registerFrameTickListener(f2)
        f2.Show()
        If Not sc.isOpen() Then
            sc.open()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For Each l In listener
            sc.unregisterFrameTickListener(l)
        Next
        listener.Clear()
        'worker = New Thread(AddressOf doIt)
        'worker.Start()
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

Public Class placeholder
    Implements IFrameTickListener
    Public Sub sendTick(ByVal frame As Long, ByVal channel As Integer) Implements IFrameTickListener.sendTick
        logger.debug("Channel " & channel & ": " & frame)
    End Sub
End Class