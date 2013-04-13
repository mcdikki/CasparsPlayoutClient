'Imports System.Threading

Public Class Form2

    Private sc As ServerController
    Private maxOffset(20) As Integer
    Delegate Sub SetTextCallback(ByVal frame As Long, ByVal channel As Integer)

    Public Sub New(ByRef controller As ServerController)
        MyBase.New()
        InitializeComponent()
        Me.sc = controller
        AddHandler sc.getTicker.frameTick, AddressOf receiveTick
    End Sub

    Public Sub receiveTick(ByVal sender As Object, ByVal e As frameTickEventArgs)
        On Error Resume Next
        If Me.lstFrame.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf updateLstFrame)
            Me.Invoke(d, New Object() {e.frame, e.channel})
        Else
            updateLstFrame(e.frame, e.channel)
        End If
    End Sub

    Private Sub updateLstFrame(ByVal frame As Long, ByVal channel As Integer)
        If lstFrame.Items.Count >= channel Then
            If frame - lstFrame.Items.Item(channel - 1) > maxOffset(channel - 1) Then maxOffset(channel - 1) = frame - lstFrame.Items.Item(channel - 1)
            lstOffset.Items.Item(channel - 1) = (frame - lstFrame.Items(channel - 1) & " : " & maxOffset(channel - 1))
            lstFrame.Items.Item(channel - 1) = frame
        Else
            lstFrame.Items.Add(frame)
            lstOffset.Items.Add(0 & " : " & maxOffset(channel - 1))
        End If
    End Sub

    Private Sub Form2_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        RemoveHandler sc.getTicker.frameTick, AddressOf receiveTick
        MyBase.Finalize()
    End Sub
End Class