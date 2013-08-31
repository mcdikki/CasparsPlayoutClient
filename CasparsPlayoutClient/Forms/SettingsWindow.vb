Public Class SettingsWindow

    Private sw = New SettingsWapper

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        propGridSettings.SelectedObject = sw
    End Sub

    Private Sub cmbOk_Click(sender As Object, e As EventArgs) Handles cmbOk.Click
        My.Settings.Save()
        MsgBox("You need to restart the program for the new settings to take effect.", MsgBoxStyle.OkOnly)
        Me.Hide()
    End Sub

    Private Sub cmbLoad_Click(sender As Object, e As EventArgs) Handles cmbLoad.Click
        My.Settings.Reload()
        propGridSettings.SelectedObject = sw
    End Sub

    Private Sub cmbRestoreDefault_Click(sender As Object, e As EventArgs) Handles cmbRestoreDefault.Click
        My.Settings.Reset()
        propGridSettings.SelectedObject = sw
    End Sub
End Class