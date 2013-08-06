'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let know of any changes and improofments you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

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
