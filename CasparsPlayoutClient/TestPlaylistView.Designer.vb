<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestPlaylistView
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
        Me.layoutContentSplit = New System.Windows.Forms.SplitContainer()
        Me.buttonLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.ckbParallel = New System.Windows.Forms.CheckBox()
        Me.ckbAuto = New System.Windows.Forms.CheckBox()
        Me.ckbLoop = New System.Windows.Forms.CheckBox()
        Me.childLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.layoutHeaderTable = New System.Windows.Forms.TableLayoutPanel()
        Me.lblExpand = New System.Windows.Forms.Label()
        Me.cmbToggleButton = New System.Windows.Forms.Button()
        Me.pbPlayed = New System.Windows.Forms.ProgressBar()
        Me.layoutInfos = New System.Windows.Forms.FlowLayoutPanel()
        Me.grbChannelLayer = New System.Windows.Forms.GroupBox()
        Me.chLayerLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.grbDelay = New System.Windows.Forms.GroupBox()
        Me.txtDelay = New System.Windows.Forms.TextBox()
        Me.grbDuration = New System.Windows.Forms.GroupBox()
        Me.txtDuration = New System.Windows.Forms.TextBox()
        Me.grpPosition = New System.Windows.Forms.GroupBox()
        Me.txtPosition = New System.Windows.Forms.TextBox()
        Me.grpRemaining = New System.Windows.Forms.GroupBox()
        Me.txtRemaining = New System.Windows.Forms.TextBox()
        Me.layoutName = New System.Windows.Forms.Panel()
        Me.nudLayer = New System.Windows.Forms.NumericUpDown()
        Me.nudChannel = New System.Windows.Forms.NumericUpDown()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.layoutHeaderContentSplit = New System.Windows.Forms.SplitContainer()
        CType(Me.layoutContentSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutContentSplit.Panel1.SuspendLayout()
        Me.layoutContentSplit.Panel2.SuspendLayout()
        Me.layoutContentSplit.SuspendLayout()
        Me.buttonLayout.SuspendLayout()
        Me.layoutHeaderTable.SuspendLayout()
        Me.layoutInfos.SuspendLayout()
        Me.grbChannelLayer.SuspendLayout()
        Me.grbDelay.SuspendLayout()
        Me.grbDuration.SuspendLayout()
        Me.grpPosition.SuspendLayout()
        Me.grpRemaining.SuspendLayout()
        Me.layoutName.SuspendLayout()
        CType(Me.nudLayer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudChannel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.layoutHeaderContentSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutHeaderContentSplit.Panel1.SuspendLayout()
        Me.layoutHeaderContentSplit.Panel2.SuspendLayout()
        Me.layoutHeaderContentSplit.SuspendLayout()
        Me.SuspendLayout()
        '
        'layoutContentSplit
        '
        Me.layoutContentSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutContentSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.layoutContentSplit.IsSplitterFixed = True
        Me.layoutContentSplit.Location = New System.Drawing.Point(0, 0)
        Me.layoutContentSplit.Name = "layoutContentSplit"
        '
        'layoutContentSplit.Panel1
        '
        Me.layoutContentSplit.Panel1.Controls.Add(Me.buttonLayout)
        Me.layoutContentSplit.Panel1MinSize = 30
        '
        'layoutContentSplit.Panel2
        '
        Me.layoutContentSplit.Panel2.AutoScroll = True
        Me.layoutContentSplit.Panel2.Controls.Add(Me.childLayout)
        Me.layoutContentSplit.Size = New System.Drawing.Size(311, 117)
        Me.layoutContentSplit.SplitterDistance = 30
        Me.layoutContentSplit.TabIndex = 0
        '
        'buttonLayout
        '
        Me.buttonLayout.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buttonLayout.AutoSize = True
        Me.buttonLayout.Controls.Add(Me.ckbParallel)
        Me.buttonLayout.Controls.Add(Me.ckbAuto)
        Me.buttonLayout.Controls.Add(Me.ckbLoop)
        Me.buttonLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.buttonLayout.Location = New System.Drawing.Point(0, 0)
        Me.buttonLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.buttonLayout.MaximumSize = New System.Drawing.Size(35, 0)
        Me.buttonLayout.MinimumSize = New System.Drawing.Size(30, 0)
        Me.buttonLayout.Name = "buttonLayout"
        Me.buttonLayout.Size = New System.Drawing.Size(35, 111)
        Me.buttonLayout.TabIndex = 2
        '
        'ckbParallel
        '
        Me.ckbParallel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbParallel.AutoSize = True
        Me.ckbParallel.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbParallel.Location = New System.Drawing.Point(2, 3)
        Me.ckbParallel.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ckbParallel.Name = "ckbParallel"
        Me.ckbParallel.Size = New System.Drawing.Size(35, 31)
        Me.ckbParallel.TabIndex = 0
        Me.ckbParallel.Text = "Par."
        Me.ckbParallel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbParallel.UseVisualStyleBackColor = True
        '
        'ckbAuto
        '
        Me.ckbAuto.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbAuto.AutoSize = True
        Me.ckbAuto.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbAuto.Location = New System.Drawing.Point(2, 40)
        Me.ckbAuto.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.ckbAuto.Name = "ckbAuto"
        Me.ckbAuto.Size = New System.Drawing.Size(35, 31)
        Me.ckbAuto.TabIndex = 1
        Me.ckbAuto.Text = "Auto" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ckbAuto.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ckbAuto.UseVisualStyleBackColor = True
        '
        'ckbLoop
        '
        Me.ckbLoop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        'childLayout
        '
        Me.childLayout.AutoScroll = True
        Me.childLayout.AutoSize = True
        Me.childLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.childLayout.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.childLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.childLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.childLayout.Location = New System.Drawing.Point(0, 0)
        Me.childLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.childLayout.Name = "childLayout"
        Me.childLayout.Size = New System.Drawing.Size(277, 117)
        Me.childLayout.TabIndex = 6
        Me.childLayout.WrapContents = False
        '
        'layoutHeaderTable
        '
        Me.layoutHeaderTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.layoutHeaderTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutHeaderTable.ColumnCount = 2
        Me.layoutHeaderTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.layoutHeaderTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutHeaderTable.Controls.Add(Me.lblExpand, 0, 2)
        Me.layoutHeaderTable.Controls.Add(Me.cmbToggleButton, 0, 0)
        Me.layoutHeaderTable.Controls.Add(Me.pbPlayed, 1, 1)
        Me.layoutHeaderTable.Controls.Add(Me.layoutInfos, 1, 2)
        Me.layoutHeaderTable.Controls.Add(Me.layoutName, 1, 0)
        Me.layoutHeaderTable.Location = New System.Drawing.Point(0, 0)
        Me.layoutHeaderTable.MinimumSize = New System.Drawing.Size(300, 90)
        Me.layoutHeaderTable.Name = "layoutHeaderTable"
        Me.layoutHeaderTable.RowCount = 3
        Me.layoutHeaderTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.layoutHeaderTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutHeaderTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.layoutHeaderTable.Size = New System.Drawing.Size(311, 90)
        Me.layoutHeaderTable.TabIndex = 0
        '
        'lblExpand
        '
        Me.lblExpand.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExpand.AutoEllipsis = True
        Me.lblExpand.AutoSize = True
        Me.lblExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblExpand.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpand.Location = New System.Drawing.Point(14, 70)
        Me.lblExpand.Name = "lblExpand"
        Me.lblExpand.Size = New System.Drawing.Size(18, 20)
        Me.lblExpand.TabIndex = 17
        Me.lblExpand.Text = "+"
        '
        'cmbToggleButton
        '
        Me.cmbToggleButton.AutoSize = True
        Me.cmbToggleButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmbToggleButton.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbToggleButton.Location = New System.Drawing.Point(2, 3)
        Me.cmbToggleButton.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.cmbToggleButton.Name = "cmbToggleButton"
        Me.layoutHeaderTable.SetRowSpan(Me.cmbToggleButton, 2)
        Me.cmbToggleButton.Size = New System.Drawing.Size(31, 44)
        Me.cmbToggleButton.TabIndex = 16
        Me.cmbToggleButton.Text = ">"
        Me.cmbToggleButton.UseVisualStyleBackColor = True
        '
        'pbPlayed
        '
        Me.pbPlayed.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPlayed.Location = New System.Drawing.Point(37, 33)
        Me.pbPlayed.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.pbPlayed.Name = "pbPlayed"
        Me.pbPlayed.Size = New System.Drawing.Size(272, 14)
        Me.pbPlayed.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbPlayed.TabIndex = 11
        '
        'layoutInfos
        '
        Me.layoutInfos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.layoutInfos.Controls.Add(Me.grbChannelLayer)
        Me.layoutInfos.Controls.Add(Me.grbDelay)
        Me.layoutInfos.Controls.Add(Me.grbDuration)
        Me.layoutInfos.Controls.Add(Me.grpPosition)
        Me.layoutInfos.Controls.Add(Me.grpRemaining)
        Me.layoutInfos.Location = New System.Drawing.Point(38, 53)
        Me.layoutInfos.Name = "layoutInfos"
        Me.layoutInfos.Size = New System.Drawing.Size(270, 34)
        Me.layoutInfos.TabIndex = 12
        '
        'grbChannelLayer
        '
        Me.grbChannelLayer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbChannelLayer.AutoSize = True
        Me.grbChannelLayer.Controls.Add(Me.chLayerLayout)
        Me.grbChannelLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbChannelLayer.Location = New System.Drawing.Point(0, 0)
        Me.grbChannelLayer.Margin = New System.Windows.Forms.Padding(0)
        Me.grbChannelLayer.Name = "grbChannelLayer"
        Me.grbChannelLayer.Padding = New System.Windows.Forms.Padding(0)
        Me.grbChannelLayer.Size = New System.Drawing.Size(0, 35)
        Me.grbChannelLayer.TabIndex = 7
        Me.grbChannelLayer.TabStop = False
        Me.grbChannelLayer.Text = "Channel-Layer"
        '
        'chLayerLayout
        '
        Me.chLayerLayout.AutoSize = True
        Me.chLayerLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.chLayerLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chLayerLayout.Location = New System.Drawing.Point(0, 10)
        Me.chLayerLayout.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.chLayerLayout.Name = "chLayerLayout"
        Me.chLayerLayout.Size = New System.Drawing.Size(0, 25)
        Me.chLayerLayout.TabIndex = 0
        '
        'grbDelay
        '
        Me.grbDelay.Controls.Add(Me.txtDelay)
        Me.grbDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbDelay.Location = New System.Drawing.Point(0, 0)
        Me.grbDelay.Margin = New System.Windows.Forms.Padding(0)
        Me.grbDelay.Name = "grbDelay"
        Me.grbDelay.Padding = New System.Windows.Forms.Padding(1)
        Me.grbDelay.Size = New System.Drawing.Size(65, 35)
        Me.grbDelay.TabIndex = 9
        Me.grbDelay.TabStop = False
        Me.grbDelay.Text = "Delay"
        '
        'txtDelay
        '
        Me.txtDelay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDelay.Location = New System.Drawing.Point(1, 12)
        Me.txtDelay.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtDelay.MaxLength = 11
        Me.txtDelay.Name = "txtDelay"
        Me.txtDelay.Size = New System.Drawing.Size(63, 18)
        Me.txtDelay.TabIndex = 0
        Me.txtDelay.Text = "00:00:00.00"
        Me.txtDelay.WordWrap = False
        '
        'grbDuration
        '
        Me.grbDuration.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbDuration.Controls.Add(Me.txtDuration)
        Me.grbDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbDuration.Location = New System.Drawing.Point(65, 0)
        Me.grbDuration.Margin = New System.Windows.Forms.Padding(0)
        Me.grbDuration.Name = "grbDuration"
        Me.grbDuration.Padding = New System.Windows.Forms.Padding(1)
        Me.grbDuration.Size = New System.Drawing.Size(65, 35)
        Me.grbDuration.TabIndex = 8
        Me.grbDuration.TabStop = False
        Me.grbDuration.Text = "Duration"
        '
        'txtDuration
        '
        Me.txtDuration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDuration.Location = New System.Drawing.Point(1, 12)
        Me.txtDuration.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(63, 18)
        Me.txtDuration.TabIndex = 0
        Me.txtDuration.Text = "00:00:00.00"
        Me.txtDuration.WordWrap = False
        '
        'grpPosition
        '
        Me.grpPosition.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpPosition.Controls.Add(Me.txtPosition)
        Me.grpPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpPosition.Location = New System.Drawing.Point(130, 0)
        Me.grpPosition.Margin = New System.Windows.Forms.Padding(0)
        Me.grpPosition.Name = "grpPosition"
        Me.grpPosition.Padding = New System.Windows.Forms.Padding(1)
        Me.grpPosition.Size = New System.Drawing.Size(65, 35)
        Me.grpPosition.TabIndex = 10
        Me.grpPosition.TabStop = False
        Me.grpPosition.Text = "Position"
        '
        'txtPosition
        '
        Me.txtPosition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPosition.Location = New System.Drawing.Point(1, 12)
        Me.txtPosition.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtPosition.Name = "txtPosition"
        Me.txtPosition.Size = New System.Drawing.Size(63, 18)
        Me.txtPosition.TabIndex = 0
        Me.txtPosition.Text = "00:00:00.00"
        Me.txtPosition.WordWrap = False
        '
        'grpRemaining
        '
        Me.grpRemaining.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRemaining.Controls.Add(Me.txtRemaining)
        Me.grpRemaining.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRemaining.Location = New System.Drawing.Point(195, 0)
        Me.grpRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.grpRemaining.Name = "grpRemaining"
        Me.grpRemaining.Padding = New System.Windows.Forms.Padding(1)
        Me.grpRemaining.Size = New System.Drawing.Size(65, 35)
        Me.grpRemaining.TabIndex = 11
        Me.grpRemaining.TabStop = False
        Me.grpRemaining.Text = "Remaining"
        '
        'txtRemaining
        '
        Me.txtRemaining.Location = New System.Drawing.Point(1, 12)
        Me.txtRemaining.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtRemaining.Name = "txtRemaining"
        Me.txtRemaining.Size = New System.Drawing.Size(65, 18)
        Me.txtRemaining.TabIndex = 0
        Me.txtRemaining.Text = "00:00:00.00"
        Me.txtRemaining.WordWrap = False
        '
        'layoutName
        '
        Me.layoutName.AutoSize = True
        Me.layoutName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutName.Controls.Add(Me.nudLayer)
        Me.layoutName.Controls.Add(Me.nudChannel)
        Me.layoutName.Controls.Add(Me.txtName)
        Me.layoutName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutName.Location = New System.Drawing.Point(38, 3)
        Me.layoutName.MinimumSize = New System.Drawing.Size(70, 27)
        Me.layoutName.Name = "layoutName"
        Me.layoutName.Size = New System.Drawing.Size(270, 27)
        Me.layoutName.TabIndex = 15
        '
        'nudLayer
        '
        Me.nudLayer.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.nudLayer.AutoSize = True
        Me.nudLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudLayer.Location = New System.Drawing.Point(188, 4)
        Me.nudLayer.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.nudLayer.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudLayer.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudLayer.Name = "nudLayer"
        Me.nudLayer.Size = New System.Drawing.Size(41, 20)
        Me.nudLayer.TabIndex = 4
        '
        'nudChannel
        '
        Me.nudChannel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.nudChannel.AutoSize = True
        Me.nudChannel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudChannel.Location = New System.Drawing.Point(230, 4)
        Me.nudChannel.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.nudChannel.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudChannel.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudChannel.Name = "nudChannel"
        Me.nudChannel.Size = New System.Drawing.Size(41, 20)
        Me.nudChannel.TabIndex = 13
        '
        'txtName
        '
        Me.txtName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.txtName.Location = New System.Drawing.Point(2, 3)
        Me.txtName.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(182, 20)
        Me.txtName.TabIndex = 10
        Me.txtName.Text = "Name"
        '
        'layoutHeaderContentSplit
        '
        Me.layoutHeaderContentSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutHeaderContentSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.layoutHeaderContentSplit.IsSplitterFixed = True
        Me.layoutHeaderContentSplit.Location = New System.Drawing.Point(0, 0)
        Me.layoutHeaderContentSplit.MinimumSize = New System.Drawing.Size(300, 95)
        Me.layoutHeaderContentSplit.Name = "layoutHeaderContentSplit"
        Me.layoutHeaderContentSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'layoutHeaderContentSplit.Panel1
        '
        Me.layoutHeaderContentSplit.Panel1.Controls.Add(Me.layoutHeaderTable)
        Me.layoutHeaderContentSplit.Panel1MinSize = 90
        '
        'layoutHeaderContentSplit.Panel2
        '
        Me.layoutHeaderContentSplit.Panel2.Controls.Add(Me.layoutContentSplit)
        Me.layoutHeaderContentSplit.Panel2MinSize = 110
        Me.layoutHeaderContentSplit.Size = New System.Drawing.Size(311, 211)
        Me.layoutHeaderContentSplit.SplitterDistance = 90
        Me.layoutHeaderContentSplit.TabIndex = 0
        '
        'TestPlaylistView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.layoutHeaderContentSplit)
        Me.MinimumSize = New System.Drawing.Size(310, 95)
        Me.Name = "TestPlaylistView"
        Me.Size = New System.Drawing.Size(311, 211)
        Me.layoutContentSplit.Panel1.ResumeLayout(False)
        Me.layoutContentSplit.Panel1.PerformLayout()
        Me.layoutContentSplit.Panel2.ResumeLayout(False)
        Me.layoutContentSplit.Panel2.PerformLayout()
        CType(Me.layoutContentSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutContentSplit.ResumeLayout(False)
        Me.buttonLayout.ResumeLayout(False)
        Me.buttonLayout.PerformLayout()
        Me.layoutHeaderTable.ResumeLayout(False)
        Me.layoutHeaderTable.PerformLayout()
        Me.layoutInfos.ResumeLayout(False)
        Me.layoutInfos.PerformLayout()
        Me.grbChannelLayer.ResumeLayout(False)
        Me.grbChannelLayer.PerformLayout()
        Me.grbDelay.ResumeLayout(False)
        Me.grbDelay.PerformLayout()
        Me.grbDuration.ResumeLayout(False)
        Me.grbDuration.PerformLayout()
        Me.grpPosition.ResumeLayout(False)
        Me.grpPosition.PerformLayout()
        Me.grpRemaining.ResumeLayout(False)
        Me.grpRemaining.PerformLayout()
        Me.layoutName.ResumeLayout(False)
        Me.layoutName.PerformLayout()
        CType(Me.nudLayer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudChannel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutHeaderContentSplit.Panel1.ResumeLayout(False)
        Me.layoutHeaderContentSplit.Panel2.ResumeLayout(False)
        CType(Me.layoutHeaderContentSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutHeaderContentSplit.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents layoutContentSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents buttonLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ckbParallel As System.Windows.Forms.CheckBox
    Friend WithEvents ckbAuto As System.Windows.Forms.CheckBox
    Friend WithEvents ckbLoop As System.Windows.Forms.CheckBox
    Friend WithEvents childLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents layoutHeaderTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblExpand As System.Windows.Forms.Label
    Friend WithEvents cmbToggleButton As System.Windows.Forms.Button
    Friend WithEvents pbPlayed As System.Windows.Forms.ProgressBar
    Friend WithEvents layoutInfos As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents grbChannelLayer As System.Windows.Forms.GroupBox
    Friend WithEvents chLayerLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents grbDelay As System.Windows.Forms.GroupBox
    Friend WithEvents txtDelay As System.Windows.Forms.TextBox
    Friend WithEvents grbDuration As System.Windows.Forms.GroupBox
    Friend WithEvents txtDuration As System.Windows.Forms.TextBox
    Friend WithEvents grpPosition As System.Windows.Forms.GroupBox
    Friend WithEvents txtPosition As System.Windows.Forms.TextBox
    Friend WithEvents grpRemaining As System.Windows.Forms.GroupBox
    Friend WithEvents txtRemaining As System.Windows.Forms.TextBox
    Friend WithEvents layoutName As System.Windows.Forms.Panel
    Friend WithEvents nudLayer As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudChannel As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents layoutHeaderContentSplit As System.Windows.Forms.SplitContainer

End Class
