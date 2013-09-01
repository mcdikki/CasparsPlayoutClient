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
        Me.Close()
    End Sub

    Private Sub cmbLoad_Click(sender As Object, e As EventArgs) Handles cmbLoad.Click
        My.Settings.Reload()
        propGridSettings.SelectedObject = sw
    End Sub

    Private Sub cmbRestoreDefault_Click(sender As Object, e As EventArgs) Handles cmbRestoreDefault.Click
        My.Settings.Reset()
        propGridSettings.SelectedObject = sw
    End Sub

    Private Sub cmbUpgrade_Click(sender As Object, e As EventArgs) Handles cmbUpgrade.Click
        If MsgBox("Do you really want to import settings from a older version of Caspar's Playout Client? This will OVERWRITE all current settings.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Upgrade older settings") = MsgBoxResult.Ok Then
            My.Settings.Upgrade()
        End If
    End Sub
End Class