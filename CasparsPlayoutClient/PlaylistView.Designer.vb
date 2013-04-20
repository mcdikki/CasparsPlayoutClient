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
        Me.nameLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.layoutName1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.childLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.cmbToggleButton = New System.Windows.Forms.Button()
        Me.buttonLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.ckbLoop = New System.Windows.Forms.CheckBox()
        Me.ckbAuto = New System.Windows.Forms.CheckBox()
        Me.ckbParallel = New System.Windows.Forms.CheckBox()
        Me.infoLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.grpRemaining = New System.Windows.Forms.GroupBox()
        Me.txtRemaining = New System.Windows.Forms.TextBox()
        Me.grpPosition = New System.Windows.Forms.GroupBox()
        Me.txtPosition = New System.Windows.Forms.TextBox()
        Me.grbDuration = New System.Windows.Forms.GroupBox()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.grbDelay = New System.Windows.Forms.GroupBox()
        Me.txtDelay = New System.Windows.Forms.TextBox()
        Me.grbChannelLayer = New System.Windows.Forms.GroupBox()
        Me.chLayerLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.nudLayer = New System.Windows.Forms.NumericUpDown()
        Me.nudChannel = New System.Windows.Forms.NumericUpDown()
        Me.TableLayoutMain = New System.Windows.Forms.TableLayoutPanel()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.pbPlayed = New System.Windows.Forms.ProgressBar()
        Me.lblExpand = New System.Windows.Forms.Label()
        Me.nameLayout.SuspendLayout()
        Me.layoutName1.SuspendLayout()
        Me.buttonLayout.SuspendLayout()
        Me.infoLayout.SuspendLayout()
        Me.grpRemaining.SuspendLayout()
        Me.grpPosition.SuspendLayout()
        Me.grbDuration.SuspendLayout()
        Me.grbDelay.SuspendLayout()
        Me.grbChannelLayer.SuspendLayout()
        Me.chLayerLayout.SuspendLayout()
        CType(Me.nudLayer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'nameLayout
        '
        Me.nameLayout.AutoSize = True
        Me.nameLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.nameLayout.Controls.Add(Me.layoutName1)
        Me.nameLayout.Controls.Add(Me.pbPlayed)
        Me.nameLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nameLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.nameLayout.Location = New System.Drawing.Point(49, 3)
        Me.nameLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.nameLayout.Name = "nameLayout"
        Me.nameLayout.Size = New System.Drawing.Size(317, 47)
        Me.nameLayout.TabIndex = 4
        Me.nameLayout.WrapContents = False
        '
        'layoutName1
        '
        Me.layoutName1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutName1.Controls.Add(Me.txtName)
        Me.layoutName1.Controls.Add(Me.lblExpand)
        Me.layoutName1.Location = New System.Drawing.Point(3, 3)
        Me.layoutName1.Name = "layoutName1"
        Me.layoutName1.Size = New System.Drawing.Size(311, 20)
        Me.layoutName1.TabIndex = 7
        Me.layoutName1.WrapContents = False
        '
        'childLayout
        '
        Me.childLayout.AutoScroll = True
        Me.childLayout.AutoSize = True
        Me.childLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.childLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.childLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.childLayout.Location = New System.Drawing.Point(49, 109)
        Me.childLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.childLayout.Name = "childLayout"
        Me.childLayout.Size = New System.Drawing.Size(317, 116)
        Me.childLayout.TabIndex = 5
        Me.childLayout.WrapContents = False
        '
        'cmbToggleButton
        '
        Me.cmbToggleButton.AutoSize = True
        Me.cmbToggleButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmbToggleButton.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbToggleButton.Location = New System.Drawing.Point(2, 3)
        Me.cmbToggleButton.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.cmbToggleButton.Name = "cmbToggleButton"
        Me.cmbToggleButton.Size = New System.Drawing.Size(43, 47)
        Me.cmbToggleButton.TabIndex = 2
        Me.cmbToggleButton.Text = ">"
        Me.cmbToggleButton.UseVisualStyleBackColor = True
        '
        'buttonLayout
        '
        Me.buttonLayout.AutoSize = True
        Me.buttonLayout.Controls.Add(Me.ckbParallel)
        Me.buttonLayout.Controls.Add(Me.ckbAuto)
        Me.buttonLayout.Controls.Add(Me.ckbLoop)
        Me.buttonLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.buttonLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.buttonLayout.Location = New System.Drawing.Point(2, 109)
        Me.buttonLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.buttonLayout.Name = "buttonLayout"
        Me.buttonLayout.Size = New System.Drawing.Size(43, 116)
        Me.buttonLayout.TabIndex = 1
        '
        'ckbLoop
        '
        Me.ckbLoop.AutoSize = True
        Me.ckbLoop.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbLoop.Location = New System.Drawing.Point(2, 77)
        Me.ckbLoop.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ckbLoop.Name = "ckbLoop"
        Me.ckbLoop.Size = New System.Drawing.Size(35, 31)
        Me.ckbLoop.TabIndex = 2
        Me.ckbLoop.Text = "Loop"
        Me.ckbLoop.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbLoop.UseVisualStyleBackColor = True
        '
        'ckbAuto
        '
        Me.ckbAuto.AutoSize = True
        Me.ckbAuto.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbAuto.Location = New System.Drawing.Point(2, 40)
        Me.ckbAuto.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ckbAuto.Name = "ckbAuto"
        Me.ckbAuto.Size = New System.Drawing.Size(33, 31)
        Me.ckbAuto.TabIndex = 1
        Me.ckbAuto.Text = "Auto" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ckbAuto.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbAuto.UseVisualStyleBackColor = True
        '
        'ckbParallel
        '
        Me.ckbParallel.AutoSize = True
        Me.ckbParallel.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbParallel.Location = New System.Drawing.Point(2, 3)
        Me.ckbParallel.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ckbParallel.Name = "ckbParallel"
        Me.ckbParallel.Size = New System.Drawing.Size(30, 31)
        Me.ckbParallel.TabIndex = 0
        Me.ckbParallel.Text = "Par."
        Me.ckbParallel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbParallel.UseVisualStyleBackColor = True
        '
        'infoLayout
        '
        Me.infoLayout.AutoSize = True
        Me.infoLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.infoLayout.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutMain.SetColumnSpan(Me.infoLayout, 2)
        Me.infoLayout.Controls.Add(Me.grbChannelLayer)
        Me.infoLayout.Controls.Add(Me.grbDelay)
        Me.infoLayout.Controls.Add(Me.grbDuration)
        Me.infoLayout.Controls.Add(Me.grpPosition)
        Me.infoLayout.Controls.Add(Me.grpRemaining)
        Me.infoLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.infoLayout.Location = New System.Drawing.Point(2, 56)
        Me.infoLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.infoLayout.Name = "infoLayout"
        Me.infoLayout.Size = New System.Drawing.Size(364, 47)
        Me.infoLayout.TabIndex = 0
        Me.infoLayout.WrapContents = False
        '
        'grpRemaining
        '
        Me.grpRemaining.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grpRemaining.Controls.Add(Me.txtRemaining)
        Me.grpRemaining.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpRemaining.Location = New System.Drawing.Point(296, 1)
        Me.grpRemaining.Margin = New System.Windows.Forms.Padding(1)
        Me.grpRemaining.Name = "grpRemaining"
        Me.grpRemaining.Padding = New System.Windows.Forms.Padding(1)
        Me.grpRemaining.Size = New System.Drawing.Size(65, 41)
        Me.grpRemaining.TabIndex = 6
        Me.grpRemaining.TabStop = False
        Me.grpRemaining.Text = "Remaining"
        '
        'txtRemaining
        '
        Me.txtRemaining.Location = New System.Drawing.Point(1, 13)
        Me.txtRemaining.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtRemaining.Name = "txtRemaining"
        Me.txtRemaining.Size = New System.Drawing.Size(65, 20)
        Me.txtRemaining.TabIndex = 0
        Me.txtRemaining.Text = "00:00:00.00"
        Me.txtRemaining.WordWrap = False
        '
        'grpPosition
        '
        Me.grpPosition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grpPosition.Controls.Add(Me.txtPosition)
        Me.grpPosition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpPosition.Location = New System.Drawing.Point(229, 1)
        Me.grpPosition.Margin = New System.Windows.Forms.Padding(1)
        Me.grpPosition.Name = "grpPosition"
        Me.grpPosition.Padding = New System.Windows.Forms.Padding(1)
        Me.grpPosition.Size = New System.Drawing.Size(65, 41)
        Me.grpPosition.TabIndex = 5
        Me.grpPosition.TabStop = False
        Me.grpPosition.Text = "Position"
        '
        'txtPosition
        '
        Me.txtPosition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPosition.Location = New System.Drawing.Point(1, 14)
        Me.txtPosition.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtPosition.Name = "txtPosition"
        Me.txtPosition.Size = New System.Drawing.Size(63, 20)
        Me.txtPosition.TabIndex = 0
        Me.txtPosition.Text = "00:00:00.00"
        Me.txtPosition.WordWrap = False
        '
        'grbDuration
        '
        Me.grbDuration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grbDuration.Controls.Add(Me.txtDuration)
        Me.grbDuration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grbDuration.Location = New System.Drawing.Point(162, 1)
        Me.grbDuration.Margin = New System.Windows.Forms.Padding(1)
        Me.grbDuration.Name = "grbDuration"
        Me.grbDuration.Padding = New System.Windows.Forms.Padding(1)
        Me.grbDuration.Size = New System.Drawing.Size(65, 41)
        Me.grbDuration.TabIndex = 1
        Me.grbDuration.TabStop = False
        Me.grbDuration.Text = "Duration"
        '
        'txtDuration
        '
        Me.txtDuration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDuration.Location = New System.Drawing.Point(1, 14)
        Me.txtDuration.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(63, 20)
        Me.txtDuration.TabIndex = 0
        Me.txtDuration.Text = "00:00:00.00"
        Me.txtDuration.WordWrap = False
        '
        'grbDelay
        '
        Me.grbDelay.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grbDelay.Controls.Add(Me.txtDelay)
        Me.grbDelay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grbDelay.Location = New System.Drawing.Point(95, 1)
        Me.grbDelay.Margin = New System.Windows.Forms.Padding(1)
        Me.grbDelay.Name = "grbDelay"
        Me.grbDelay.Padding = New System.Windows.Forms.Padding(1)
        Me.grbDelay.Size = New System.Drawing.Size(65, 41)
        Me.grbDelay.TabIndex = 4
        Me.grbDelay.TabStop = False
        Me.grbDelay.Text = "Delay"
        '
        'txtDelay
        '
        Me.txtDelay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDelay.Location = New System.Drawing.Point(1, 14)
        Me.txtDelay.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtDelay.MaxLength = 11
        Me.txtDelay.Name = "txtDelay"
        Me.txtDelay.Size = New System.Drawing.Size(63, 20)
        Me.txtDelay.TabIndex = 0
        Me.txtDelay.Text = "00:00:00.00"
        Me.txtDelay.WordWrap = False
        '
        'grbChannelLayer
        '
        Me.grbChannelLayer.AutoSize = True
        Me.grbChannelLayer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grbChannelLayer.Controls.Add(Me.chLayerLayout)
        Me.grbChannelLayer.Dock = System.Windows.Forms.DockStyle.Fill
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
        'chLayerLayout
        '
        Me.chLayerLayout.AutoSize = True
        Me.chLayerLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chLayerLayout.Controls.Add(Me.nudChannel)
        Me.chLayerLayout.Controls.Add(Me.nudLayer)
        Me.chLayerLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chLayerLayout.Location = New System.Drawing.Point(1, 14)
        Me.chLayerLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.chLayerLayout.Name = "chLayerLayout"
        Me.chLayerLayout.Size = New System.Drawing.Size(90, 26)
        Me.chLayerLayout.TabIndex = 0
        '
        'nudLayer
        '
        Me.nudLayer.AutoSize = True
        Me.nudLayer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nudLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudLayer.Location = New System.Drawing.Point(47, 3)
        Me.nudLayer.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.nudLayer.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudLayer.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudLayer.Name = "nudLayer"
        Me.nudLayer.Size = New System.Drawing.Size(41, 20)
        Me.nudLayer.TabIndex = 4
        '
        'nudChannel
        '
        Me.nudChannel.AutoSize = True
        Me.nudChannel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nudChannel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudChannel.Location = New System.Drawing.Point(2, 3)
        Me.nudChannel.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.nudChannel.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudChannel.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudChannel.Name = "nudChannel"
        Me.nudChannel.Size = New System.Drawing.Size(41, 20)
        Me.nudChannel.TabIndex = 3
        '
        'TableLayoutMain
        '
        Me.TableLayoutMain.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutMain.ColumnCount = 2
        Me.TableLayoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47.0!))
        Me.TableLayoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutMain.Controls.Add(Me.infoLayout, 0, 1)
        Me.TableLayoutMain.Controls.Add(Me.buttonLayout, 0, 2)
        Me.TableLayoutMain.Controls.Add(Me.cmbToggleButton, 0, 0)
        Me.TableLayoutMain.Controls.Add(Me.childLayout, 1, 2)
        Me.TableLayoutMain.Controls.Add(Me.nameLayout, 1, 0)
        Me.TableLayoutMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutMain.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.TableLayoutMain.Name = "TableLayoutMain"
        Me.TableLayoutMain.RowCount = 3
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53.0!))
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53.0!))
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutMain.Size = New System.Drawing.Size(368, 228)
        Me.TableLayoutMain.TabIndex = 0
        '
        'txtName
        '
        Me.txtName.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.txtName.Location = New System.Drawing.Point(2, 3)
        Me.txtName.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(286, 20)
        Me.txtName.TabIndex = 8
        Me.txtName.Text = "Name"
        '
        'pbPlayed
        '
        Me.pbPlayed.Dock = System.Windows.Forms.DockStyle.Left
        Me.pbPlayed.Location = New System.Drawing.Point(2, 29)
        Me.pbPlayed.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.pbPlayed.Name = "pbPlayed"
        Me.pbPlayed.Size = New System.Drawing.Size(312, 14)
        Me.pbPlayed.TabIndex = 8
        '
        'lblExpand
        '
        Me.lblExpand.AutoEllipsis = True
        Me.lblExpand.AutoSize = True
        Me.lblExpand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblExpand.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblExpand.Location = New System.Drawing.Point(293, 0)
        Me.lblExpand.Name = "lblExpand"
        Me.lblExpand.Size = New System.Drawing.Size(15, 15)
        Me.lblExpand.TabIndex = 9
        Me.lblExpand.Text = "+"
        '
        'PlaylistView
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.ListItem
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.TableLayoutMain)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.Name = "PlaylistView"
        Me.Size = New System.Drawing.Size(370, 231)
        Me.nameLayout.ResumeLayout(False)
        Me.layoutName1.ResumeLayout(False)
        Me.layoutName1.PerformLayout()
        Me.buttonLayout.ResumeLayout(False)
        Me.buttonLayout.PerformLayout()
        Me.infoLayout.ResumeLayout(False)
        Me.infoLayout.PerformLayout()
        Me.grpRemaining.ResumeLayout(False)
        Me.grpRemaining.PerformLayout()
        Me.grpPosition.ResumeLayout(False)
        Me.grpPosition.PerformLayout()
        Me.grbDuration.ResumeLayout(False)
        Me.grbDuration.PerformLayout()
        Me.grbDelay.ResumeLayout(False)
        Me.grbDelay.PerformLayout()
        Me.grbChannelLayer.ResumeLayout(False)
        Me.grbChannelLayer.PerformLayout()
        Me.chLayerLayout.ResumeLayout(False)
        Me.chLayerLayout.PerformLayout()
        CType(Me.nudLayer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudChannel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutMain.ResumeLayout(False)
        Me.TableLayoutMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents nameLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents layoutName1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents childLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbToggleButton As System.Windows.Forms.Button
    Friend WithEvents buttonLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ckbParallel As System.Windows.Forms.CheckBox
    Friend WithEvents ckbAuto As System.Windows.Forms.CheckBox
    Friend WithEvents ckbLoop As System.Windows.Forms.CheckBox
    Friend WithEvents infoLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents TableLayoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents grbChannelLayer As System.Windows.Forms.GroupBox
    Friend WithEvents chLayerLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents nudChannel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLayer As System.Windows.Forms.NumericUpDown
    Friend WithEvents grbDelay As System.Windows.Forms.GroupBox
    Friend WithEvents txtDelay As System.Windows.Forms.TextBox
    Friend WithEvents grbDuration As System.Windows.Forms.GroupBox
    Friend WithEvents txtDuration As System.Windows.Forms.TextBox
    Friend WithEvents grpPosition As System.Windows.Forms.GroupBox
    Friend WithEvents txtPosition As System.Windows.Forms.TextBox
    Friend WithEvents grpRemaining As System.Windows.Forms.GroupBox
    Friend WithEvents txtRemaining As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents pbPlayed As System.Windows.Forms.ProgressBar
    Friend WithEvents lblExpand As System.Windows.Forms.Label

End Class
