Imports System.Threading

Public Class Form2
    Implements IFrameTickListener

    Private sc As ServerController
    Delegate Sub SetTextCallback(ByVal frame As Long, ByVal channel As Integer)

    Public Sub New(ByRef controller As ServerController)
        MyBase.New()
        InitializeComponent()
        Me.sc = controller
        'Monitor.Enter(sc)
        'While Not sc.registerFrameTickListener(Me)
        '    logger.debug("Retry to register...")
        '    Thread.Sleep(500)
        'End While
        'logger.debug("...registered")
        'Monitor.Exit(sc)
    End Sub

    Public Sub sendTick(ByVal frame As Long, ByVal channel As Integer) Implements IFrameTickListener.sendTick
        'logger.debug("got Tick...")
        On Error Resume Next
        If Me.lstFrame.InvokeRequired Then
            'logger.debug("calling delegate...")
            Dim d As New SetTextCallback(AddressOf updateLstFrame)
            Me.Invoke(d, New Object() {frame, channel})
        Else
            'logger.debug("calling update...")
            updateLstFrame(frame, channel)
        End If
    End Sub

    Private Sub updateLstFrame(ByVal frame As Long, ByVal channel As Integer)
        'logger.debug("updating display...")
        If lstFrame.Items.Count >= channel Then
            lstFrame.Items.Item(channel - 1) = frame
        Else
            lstFrame.Items.Add(frame)
        End If
    End Sub

    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        'Monitor.Enter(sc)
        sc.unregisterFrameTickListener(Me)
        'Monitor.Exit(sc)
        MyBase.Finalize()
    End Sub
End Class