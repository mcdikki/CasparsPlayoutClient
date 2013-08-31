<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsWindow
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.propGridSettings = New System.Windows.Forms.PropertyGrid()
        Me.layoutButtonFlow = New System.Windows.Forms.FlowLayoutPanel()
        Me.cmbOk = New System.Windows.Forms.Button()
        Me.cmbRestoreDefault = New System.Windows.Forms.Button()
        Me.cmbLoad = New System.Windows.Forms.Button()
        Me.layoutButtonFlow.SuspendLayout()
        Me.SuspendLayout()
        '
        'propGridSettings
        '
        Me.propGridSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.propGridSettings.Location = New System.Drawing.Point(8, 8)
        Me.propGridSettings.Name = "propGridSettings"
        Me.propGridSettings.Size = New System.Drawing.Size(642, 316)
        Me.propGridSettings.TabIndex = 1
        '
        'layoutButtonFlow
        '
        Me.layoutButtonFlow.Controls.Add(Me.cmbOk)
        Me.layoutButtonFlow.Controls.Add(Me.cmbRestoreDefault)
        Me.layoutButtonFlow.Controls.Add(Me.cmbLoad)
        Me.layoutButtonFlow.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.layoutButtonFlow.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.layoutButtonFlow.Location = New System.Drawing.Point(0, 327)
        Me.layoutButtonFlow.Name = "layoutButtonFlow"
        Me.layoutButtonFlow.Size = New System.Drawing.Size(656, 28)
        Me.layoutButtonFlow.TabIndex = 3
        '
        'cmbOk
        '
        Me.cmbOk.AutoSize = True
        Me.cmbOk.Location = New System.Drawing.Point(578, 3)
        Me.cmbOk.Name = "cmbOk"
        Me.cmbOk.Size = New System.Drawing.Size(75, 23)
        Me.cmbOk.TabIndex = 3
        Me.cmbOk.Text = "Ok"
        Me.cmbOk.UseVisualStyleBackColor = True
        '
        'cmbRestoreDefault
        '
        Me.cmbRestoreDefault.AutoSize = True
        Me.cmbRestoreDefault.Location = New System.Drawing.Point(477, 3)
        Me.cmbRestoreDefault.Name = "cmbRestoreDefault"
        Me.cmbRestoreDefault.Size = New System.Drawing.Size(95, 23)
        Me.cmbRestoreDefault.TabIndex = 4
        Me.cmbRestoreDefault.Text = "Restore defaults"
        Me.cmbRestoreDefault.UseVisualStyleBackColor = True
        '
        'cmbLoad
        '
        Me.cmbLoad.AutoSize = True
        Me.cmbLoad.Location = New System.Drawing.Point(376, 3)
        Me.cmbLoad.Name = "cmbLoad"
        Me.cmbLoad.Size = New System.Drawing.Size(95, 23)
        Me.cmbLoad.TabIndex = 5
        Me.cmbLoad.Text = "Load last"
        Me.cmbLoad.UseVisualStyleBackColor = True
        '
        'SettingsWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(656, 355)
        Me.Controls.Add(Me.layoutButtonFlow)
        Me.Controls.Add(Me.propGridSettings)
        Me.Name = "SettingsWindow"
        Me.Text = "Settings"
        Me.layoutButtonFlow.ResumeLayout(False)
        Me.layoutButtonFlow.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents propGridSettings As System.Windows.Forms.PropertyGrid
    Friend WithEvents layoutButtonFlow As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbOk As System.Windows.Forms.Button
    Friend WithEvents cmbRestoreDefault As System.Windows.Forms.Button
    Friend WithEvents cmbLoad As System.Windows.Forms.Button
End Class
