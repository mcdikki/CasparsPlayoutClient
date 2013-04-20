<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlaylistView
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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
        Me.TableLayoutMain = New System.Windows.Forms.TableLayoutPanel()
        Me.infoLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.buttonLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.cmbToggleButton = New System.Windows.Forms.Button()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.nameLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.pbPlayed = New System.Windows.Forms.ProgressBar()
        Me.nudChannel = New System.Windows.Forms.NumericUpDown()
        Me.nudLayer = New System.Windows.Forms.NumericUpDown()
        Me.grbChannelLayer = New System.Windows.Forms.GroupBox()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.grbDuration = New System.Windows.Forms.GroupBox()
        Me.grbDelay = New System.Windows.Forms.GroupBox()
        Me.txtDelay = New System.Windows.Forms.TextBox()
        Me.grpPosition = New System.Windows.Forms.GroupBox()
        Me.txtPosition = New System.Windows.Forms.TextBox()
        Me.grpRemaining = New System.Windows.Forms.GroupBox()
        Me.txtRemaining = New System.Windows.Forms.TextBox()
        Me.childLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.ckbParallel = New System.Windows.Forms.CheckBox()
        Me.ckbAuto = New System.Windows.Forms.CheckBox()
        Me.ckbLoop = New System.Windows.Forms.CheckBox()
        Me.TableLayoutMain.SuspendLayout()
        Me.infoLayout.SuspendLayout()
        Me.buttonLayout.SuspendLayout()
        Me.nameLayout.SuspendLayout()
        CType(Me.nudChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLayer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbChannelLayer.SuspendLayout()
        Me.grbDuration.SuspendLayout()
        Me.grbDelay.SuspendLayout()
        Me.grpPosition.SuspendLayout()
        Me.grpRemaining.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutMain
        '
        Me.TableLayoutMain.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutMain.ColumnCount = 2
        Me.TableLayoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.35294!))
        Me.TableLayoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.64706!))
        Me.TableLayoutMain.Controls.Add(Me.infoLayout, 0, 1)
        Me.TableLayoutMain.Controls.Add(Me.buttonLayout, 0, 2)
        Me.TableLayoutMain.Controls.Add(Me.cmbToggleButton, 0, 0)
        Me.TableLayoutMain.Controls.Add(Me.nameLayout, 1, 0)
        Me.TableLayoutMain.Controls.Add(Me.childLayout, 1, 2)
        Me.TableLayoutMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutMain.Name = "TableLayoutMain"
        Me.TableLayoutMain.RowCount = 3
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54.0!))
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.72289!))
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 69.27711!))
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutMain.Size = New System.Drawing.Size(489, 225)
        Me.TableLayoutMain.TabIndex = 0
        '
        'infoLayout
        '
        Me.infoLayout.AutoSize = True
        Me.infoLayout.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutMain.SetColumnSpan(Me.infoLayout, 2)
        Me.infoLayout.Controls.Add(Me.grbChannelLayer)
        Me.infoLayout.Controls.Add(Me.grbDelay)
        Me.infoLayout.Controls.Add(Me.grbDuration)
        Me.infoLayout.Controls.Add(Me.grpPosition)
        Me.infoLayout.Controls.Add(Me.grpRemaining)
        Me.infoLayout.Location = New System.Drawing.Point(3, 57)
        Me.infoLayout.Name = "infoLayout"
        Me.infoLayout.Size = New System.Drawing.Size(406, 44)
        Me.infoLayout.TabIndex = 0
        '
        'buttonLayout
        '
        Me.buttonLayout.Controls.Add(Me.ckbParallel)
        Me.buttonLayout.Controls.Add(Me.ckbAuto)
        Me.buttonLayout.Controls.Add(Me.ckbLoop)
        Me.buttonLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.buttonLayout.Location = New System.Drawing.Point(3, 109)
        Me.buttonLayout.Name = "buttonLayout"
        Me.buttonLayout.Size = New System.Drawing.Size(44, 113)
        Me.buttonLayout.TabIndex = 1
        '
        'cmbToggleButton
        '
        Me.cmbToggleButton.AutoSize = True
        Me.cmbToggleButton.Location = New System.Drawing.Point(3, 3)
        Me.cmbToggleButton.Name = "cmbToggleButton"
        Me.cmbToggleButton.Size = New System.Drawing.Size(38, 48)
        Me.cmbToggleButton.TabIndex = 2
        Me.cmbToggleButton.Text = ">"
        Me.cmbToggleButton.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.txtName.Location = New System.Drawing.Point(3, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(375, 20)
        Me.txtName.TabIndex = 3
        Me.txtName.Text = "Name"
        '
        'nameLayout
        '
        Me.nameLayout.Controls.Add(Me.txtName)
        Me.nameLayout.Controls.Add(Me.pbPlayed)
        Me.nameLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.nameLayout.Location = New System.Drawing.Point(53, 3)
        Me.nameLayout.Name = "nameLayout"
        Me.nameLayout.Size = New System.Drawing.Size(432, 48)
        Me.nameLayout.TabIndex = 4
        '
        'pbPlayed
        '
        Me.pbPlayed.Location = New System.Drawing.Point(3, 29)
        Me.pbPlayed.Name = "pbPlayed"
        Me.pbPlayed.Size = New System.Drawing.Size(375, 15)
        Me.pbPlayed.TabIndex = 4
        '
        'nudChannel
        '
        Me.nudChannel.Location = New System.Drawing.Point(6, 19)
        Me.nudChannel.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudChannel.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudChannel.Name = "nudChannel"
        Me.nudChannel.Size = New System.Drawing.Size(37, 20)
        Me.nudChannel.TabIndex = 1
        '
        'nudLayer
        '
        Me.nudLayer.Location = New System.Drawing.Point(49, 19)
        Me.nudLayer.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudLayer.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudLayer.Name = "nudLayer"
        Me.nudLayer.Size = New System.Drawing.Size(37, 20)
        Me.nudLayer.TabIndex = 2
        '
        'grbChannelLayer
        '
        Me.grbChannelLayer.Controls.Add(Me.nudChannel)
        Me.grbChannelLayer.Controls.Add(Me.nudLayer)
        Me.grbChannelLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbChannelLayer.Location = New System.Drawing.Point(1, 1)
        Me.grbChannelLayer.Margin = New System.Windows.Forms.Padding(1)
        Me.grbChannelLayer.Name = "grbChannelLayer"
        Me.grbChannelLayer.Padding = New System.Windows.Forms.Padding(1)
        Me.grbChannelLayer.Size = New System.Drawing.Size(92, 41)
        Me.grbChannelLayer.TabIndex = 0
        Me.grbChannelLayer.TabStop = False
        Me.grbChannelLayer.Text = "Channel-Layer"
        '
        'txtDuration
        '
        Me.txtDuration.Location = New System.Drawing.Point(6, 17)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(63, 20)
        Me.txtDuration.TabIndex = 0
        Me.txtDuration.Text = "00:00:00.00"
        '
        'grbDuration
        '
        Me.grbDuration.Controls.Add(Me.txtDuration)
        Me.grbDuration.Location = New System.Drawing.Point(173, 1)
        Me.grbDuration.Margin = New System.Windows.Forms.Padding(1)
        Me.grbDuration.Name = "grbDuration"
        Me.grbDuration.Padding = New System.Windows.Forms.Padding(1)
        Me.grbDuration.Size = New System.Drawing.Size(76, 42)
        Me.grbDuration.TabIndex = 1
        Me.grbDuration.TabStop = False
        Me.grbDuration.Text = "Duration"
        '
        'grbDelay
        '
        Me.grbDelay.Controls.Add(Me.txtDelay)
        Me.grbDelay.Location = New System.Drawing.Point(95, 1)
        Me.grbDelay.Margin = New System.Windows.Forms.Padding(1)
        Me.grbDelay.Name = "grbDelay"
        Me.grbDelay.Padding = New System.Windows.Forms.Padding(1)
        Me.grbDelay.Size = New System.Drawing.Size(76, 42)
        Me.grbDelay.TabIndex = 4
        Me.grbDelay.TabStop = False
        Me.grbDelay.Text = "Delay"
        '
        'txtDelay
        '
        Me.txtDelay.Location = New System.Drawing.Point(6, 17)
        Me.txtDelay.Name = "txtDelay"
        Me.txtDelay.Size = New System.Drawing.Size(63, 20)
        Me.txtDelay.TabIndex = 0
        Me.txtDelay.Text = "00:00:00.00"
        '
        'grpPosition
        '
        Me.grpPosition.Controls.Add(Me.txtPosition)
        Me.grpPosition.Location = New System.Drawing.Point(251, 1)
        Me.grpPosition.Margin = New System.Windows.Forms.Padding(1)
        Me.grpPosition.Name = "grpPosition"
        Me.grpPosition.Padding = New System.Windows.Forms.Padding(1)
        Me.grpPosition.Size = New System.Drawing.Size(76, 42)
        Me.grpPosition.TabIndex = 5
        Me.grpPosition.TabStop = False
        Me.grpPosition.Text = "Position"
        '
        'txtPosition
        '
        Me.txtPosition.Location = New System.Drawing.Point(6, 17)
        Me.txtPosition.Name = "txtPosition"
        Me.txtPosition.Size = New System.Drawing.Size(63, 20)
        Me.txtPosition.TabIndex = 0
        Me.txtPosition.Text = "00:00:00.00"
        '
        'grpRemaining
        '
        Me.grpRemaining.Controls.Add(Me.txtRemaining)
        Me.grpRemaining.Location = New System.Drawing.Point(329, 1)
        Me.grpRemaining.Margin = New System.Windows.Forms.Padding(1)
        Me.grpRemaining.Name = "grpRemaining"
        Me.grpRemaining.Padding = New System.Windows.Forms.Padding(1)
        Me.grpRemaining.Size = New System.Drawing.Size(76, 42)
        Me.grpRemaining.TabIndex = 6
        Me.grpRemaining.TabStop = False
        Me.grpRemaining.Text = "Remaining"
        '
        'txtRemaining
        '
        Me.txtRemaining.Location = New System.Drawing.Point(6, 17)
        Me.txtRemaining.Name = "txtRemaining"
        Me.txtRemaining.Size = New System.Drawing.Size(63, 20)
        Me.txtRemaining.TabIndex = 0
        Me.txtRemaining.Text = "00:00:00.00"
        '
        'childLayout
        '
        Me.childLayout.AutoSize = True
        Me.childLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.childLayout.Location = New System.Drawing.Point(53, 109)
        Me.childLayout.Name = "childLayout"
        Me.childLayout.Size = New System.Drawing.Size(0, 0)
        Me.childLayout.TabIndex = 5
        '
        'ckbParallel
        '
        Me.ckbParallel.AutoSize = True
        Me.ckbParallel.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbParallel.Location = New System.Drawing.Point(3, 3)
        Me.ckbParallel.Name = "ckbParallel"
        Me.ckbParallel.Size = New System.Drawing.Size(30, 31)
        Me.ckbParallel.TabIndex = 0
        Me.ckbParallel.Text = "Par."
        Me.ckbParallel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbParallel.UseVisualStyleBackColor = True
        '
        'ckbAuto
        '
        Me.ckbAuto.AutoSize = True
        Me.ckbAuto.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbAuto.Location = New System.Drawing.Point(3, 40)
        Me.ckbAuto.Name = "ckbAuto"
        Me.ckbAuto.Size = New System.Drawing.Size(33, 31)
        Me.ckbAuto.TabIndex = 1
        Me.ckbAuto.Text = "Auto" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ckbAuto.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbAuto.UseVisualStyleBackColor = True
        '
        'ckbLoop
        '
        Me.ckbLoop.AutoSize = True
        Me.ckbLoop.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbLoop.Location = New System.Drawing.Point(3, 77)
        Me.ckbLoop.Name = "ckbLoop"
        Me.ckbLoop.Size = New System.Drawing.Size(35, 31)
        Me.ckbLoop.TabIndex = 2
        Me.ckbLoop.Text = "Loop"
        Me.ckbLoop.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbLoop.UseVisualStyleBackColor = True
        '
        'PlaylistView
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.ListItem
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.TableLayoutMain)
        Me.Name = "PlaylistView"
        Me.Size = New System.Drawing.Size(492, 228)
        Me.TableLayoutMain.ResumeLayout(False)
        Me.TableLayoutMain.PerformLayout()
        Me.infoLayout.ResumeLayout(False)
        Me.buttonLayout.ResumeLayout(False)
        Me.buttonLayout.PerformLayout()
        Me.nameLayout.ResumeLayout(False)
        Me.nameLayout.PerformLayout()
        CType(Me.nudChannel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLayer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbChannelLayer.ResumeLayout(False)
        Me.grbDuration.ResumeLayout(False)
        Me.grbDuration.PerformLayout()
        Me.grbDelay.ResumeLayout(False)
        Me.grbDelay.PerformLayout()
        Me.grpPosition.ResumeLayout(False)
        Me.grpPosition.PerformLayout()
        Me.grpRemaining.ResumeLayout(False)
        Me.grpRemaining.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents infoLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents nudChannel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLayer As System.Windows.Forms.NumericUpDown
    Friend WithEvents buttonLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbToggleButton As System.Windows.Forms.Button
    Friend WithEvents nameLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents pbPlayed As System.Windows.Forms.ProgressBar
    Friend WithEvents grbChannelLayer As System.Windows.Forms.GroupBox
    Friend WithEvents grbDelay As System.Windows.Forms.GroupBox
    Friend WithEvents txtDelay As System.Windows.Forms.TextBox
    Friend WithEvents grbDuration As System.Windows.Forms.GroupBox
    Friend WithEvents txtDuration As System.Windows.Forms.TextBox
    Friend WithEvents grpPosition As System.Windows.Forms.GroupBox
    Friend WithEvents txtPosition As System.Windows.Forms.TextBox
    Friend WithEvents grpRemaining As System.Windows.Forms.GroupBox
    Friend WithEvents txtRemaining As System.Windows.Forms.TextBox
    Friend WithEvents childLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ckbParallel As System.Windows.Forms.CheckBox
    Friend WithEvents ckbAuto As System.Windows.Forms.CheckBox
    Friend WithEvents ckbLoop As System.Windows.Forms.CheckBox

End Class
