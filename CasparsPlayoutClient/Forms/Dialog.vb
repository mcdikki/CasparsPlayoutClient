Imports System.Windows.Forms

Public Class Dialog

    Public Sub New(Message As String)
        MyBase.New()
        InitializeComponent()
        lblText.Text = Message
        Me.Name = "Dialog"
    End Sub

    Public Sub New(Message As String, Header As String)
        MyBase.New()
        InitializeComponent()
        Me.Header.Text = Header
        lblText.Text = Message
        Me.Name = Header
    End Sub

    Public Sub New(Message As String, header As String, Image As Image)
        MyBase.New()
        InitializeComponent()
        Me.Header.Text = header
        Me.Name = header
        lblText.Text = Message
        'lblText.Image = Image
        Me.Image.Image = Image
        Me.Image.SizeMode = PictureBoxSizeMode.Zoom
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub lblText_resized(sender As Object, e As EventArgs) Handles lblText.SizeChanged
        Me.Size = New Size(Me.MinimumSize.Width + lblText.Size.Width, Me.MinimumSize.Height + lblText.Height - OK_Button.Height)
    End Sub
End Class
