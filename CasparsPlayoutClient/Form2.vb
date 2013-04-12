Public Class Form2
    Implements IFrameTickListener

    Private sc As ServerController
    Delegate Sub SetTextCallback(ByVal frame As Long, ByVal channel As Integer)

    Public Sub New(ByRef controller As ServerController)
        MyBase.New()
        InitializeComponent()
        Me.sc = controller
        sc.registerFrameTickListener(Me)
    End Sub

    Public Sub sendTick(ByVal frame As Long, ByVal channel As Integer) Implements IFrameTickListener.sendTick
        On Error Resume Next
        If Me.lstFrame.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf updateLstFrame)
            Me.Invoke(d, New Object() {frame, channel})
        Else
            updateLstFrame(frame, channel)
        End If
    End Sub

    Private Sub updateLstFrame(ByVal frame As Long, ByVal channel As Integer)
        If lstFrame.Items.Count >= channel Then
            lstFrame.Items.Item(channel - 1) = frame
        Else
            lstFrame.Items.Add(frame)
        End If
    End Sub

    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        sc.unregisterFrameTickListener(Me)
        MyBase.Finalize()
    End Sub
End Class