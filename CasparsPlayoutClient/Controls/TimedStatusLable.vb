Public Class TimedStatusLable
    Inherits ToolStripStatusLabel

    Private WithEvents timer As Timers.Timer
    Private Delegate Sub removeDelegate()

    Private Property AutoScaleMode As AutoScaleMode

    Public Sub New(ByVal stayTime As Integer, ByVal text As String)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.Text = text
        Me.ToolTipText = text
        Me.AutoToolTip = True

        timer = New Timers.Timer(stayTime)
        timer.Enabled = True
    End Sub

    Private Sub remove() Handles timer.Elapsed
        timer.Enabled = False
        If Me.Owner.InvokeRequired Then
            Me.Owner.Invoke(New removeDelegate(AddressOf remove))
        Else
            If Me.Owner.Items.Contains(Me) Then Me.Owner.Items.Remove(Me)
        End If
    End Sub

End Class
