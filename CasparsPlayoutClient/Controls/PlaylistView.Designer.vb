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
        Me.layoutContentSplit = New System.Windows.Forms.SplitContainer()
        Me.layoutChild = New System.Windows.Forms.FlowLayoutPanel()
        Me.layoutButton = New System.Windows.Forms.FlowLayoutPanel()
        Me.grpParallel = New System.Windows.Forms.GroupBox()
        Me.ckbParallel = New System.Windows.Forms.CheckBox()
        Me.grpAuto = New System.Windows.Forms.GroupBox()
        Me.ckbAuto = New System.Windows.Forms.CheckBox()
        Me.grpLoop = New System.Windows.Forms.GroupBox()
        Me.ckbLoop = New System.Windows.Forms.CheckBox()
        Me.grpClear = New System.Windows.Forms.GroupBox()
        Me.ckbClear = New System.Windows.Forms.CheckBox()
        Me.layoutHeaderTable = New System.Windows.Forms.TableLayoutPanel()
        Me.lblExpand = New System.Windows.Forms.Label()
        Me.cmbToggleButton = New System.Windows.Forms.Button()
        Me.pbPlayed = New System.Windows.Forms.ProgressBar()
        Me.layoutInfos = New System.Windows.Forms.FlowLayoutPanel()
        Me.grbChannelLayer = New System.Windows.Forms.GroupBox()
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
        Me.layoutContentSplit.Panel2.SuspendLayout()
        Me.layoutContentSplit.SuspendLayout()
        Me.layoutButton.SuspendLayout()
        Me.grpParallel.SuspendLayout()
        Me.grpAuto.SuspendLayout()
        Me.grpLoop.SuspendLayout()
        Me.grpClear.SuspendLayout()
        Me.layoutHeaderTable.SuspendLayout()
        Me.layoutInfos.SuspendLayout()
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
        Me.layoutContentSplit.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutContentSplit.Name = "layoutContentSplit"
        Me.layoutContentSplit.Panel1MinSize = 0
        '
        'layoutContentSplit.Panel2
        '
        Me.layoutContentSplit.Panel2.AutoScroll = True
        Me.layoutContentSplit.Panel2.Controls.Add(Me.layoutChild)
        Me.layoutContentSplit.Panel2MinSize = 0
        Me.layoutContentSplit.Size = New System.Drawing.Size(370, 81)
        Me.layoutContentSplit.SplitterDistance = 25
        Me.layoutContentSplit.SplitterWidth = 1
        Me.layoutContentSplit.TabIndex = 0
        Me.layoutContentSplit.TabStop = False
        '
        'layoutChild
        '
        Me.layoutChild.AutoSize = True
        Me.layoutChild.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutChild.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.layoutChild.Dock = System.Windows.Forms.DockStyle.Top
        Me.layoutChild.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.layoutChild.Location = New System.Drawing.Point(0, 0)
        Me.layoutChild.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutChild.Name = "layoutChild"
        Me.layoutChild.Size = New System.Drawing.Size(344, 4)
        Me.layoutChild.TabIndex = 6
        Me.layoutChild.WrapContents = False
        '
        'layoutButton
        '
        Me.layoutButton.AutoSize = True
        Me.layoutButton.Controls.Add(Me.grpParallel)
        Me.layoutButton.Controls.Add(Me.grpAuto)
        Me.layoutButton.Controls.Add(Me.grpLoop)
        Me.layoutButton.Controls.Add(Me.grpClear)
        Me.layoutButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.layoutButton.Location = New System.Drawing.Point(246, 0)
        Me.layoutButton.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutButton.Name = "layoutButton"
        Me.layoutButton.Size = New System.Drawing.Size(100, 36)
        Me.layoutButton.TabIndex = 6
        '
        'grpParallel
        '
        Me.grpParallel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpParallel.Controls.Add(Me.ckbParallel)
        Me.grpParallel.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpParallel.Location = New System.Drawing.Point(0, 0)
        Me.grpParallel.Margin = New System.Windows.Forms.Padding(0)
        Me.grpParallel.Name = "grpParallel"
        Me.grpParallel.Padding = New System.Windows.Forms.Padding(0)
        Me.grpParallel.Size = New System.Drawing.Size(25, 36)
        Me.grpParallel.TabIndex = 4
        Me.grpParallel.TabStop = False
        Me.grpParallel.Text = "P"
        '
        'ckbParallel
        '
        Me.ckbParallel.AutoSize = True
        Me.ckbParallel.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbParallel.Location = New System.Drawing.Point(5, 13)
        Me.ckbParallel.Margin = New System.Windows.Forms.Padding(0)
        Me.ckbParallel.Name = "ckbParallel"
        Me.ckbParallel.Size = New System.Drawing.Size(15, 14)
        Me.ckbParallel.TabIndex = 6
        Me.ckbParallel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ckbParallel.UseVisualStyleBackColor = True
        '
        'grpAuto
        '
        Me.grpAuto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAuto.Controls.Add(Me.ckbAuto)
        Me.grpAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpAuto.Location = New System.Drawing.Point(25, 0)
        Me.grpAuto.Margin = New System.Windows.Forms.Padding(0)
        Me.grpAuto.Name = "grpAuto"
        Me.grpAuto.Padding = New System.Windows.Forms.Padding(0)
        Me.grpAuto.Size = New System.Drawing.Size(25, 36)
        Me.grpAuto.TabIndex = 7
        Me.grpAuto.TabStop = False
        Me.grpAuto.Text = "A"
        '
        'ckbAuto
        '
        Me.ckbAuto.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbAuto.AutoSize = True
        Me.ckbAuto.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbAuto.Location = New System.Drawing.Point(5, 13)
        Me.ckbAuto.Margin = New System.Windows.Forms.Padding(0)
        Me.ckbAuto.Name = "ckbAuto"
        Me.ckbAuto.Size = New System.Drawing.Size(15, 14)
        Me.ckbAuto.TabIndex = 7
        Me.ckbAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ckbAuto.UseVisualStyleBackColor = True
        '
        'grpLoop
        '
        Me.grpLoop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpLoop.Controls.Add(Me.ckbLoop)
        Me.grpLoop.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpLoop.Location = New System.Drawing.Point(50, 0)
        Me.grpLoop.Margin = New System.Windows.Forms.Padding(0)
        Me.grpLoop.Name = "grpLoop"
        Me.grpLoop.Padding = New System.Windows.Forms.Padding(0)
        Me.grpLoop.Size = New System.Drawing.Size(25, 36)
        Me.grpLoop.TabIndex = 8
        Me.grpLoop.TabStop = False
        Me.grpLoop.Text = "L"
        '
        'ckbLoop
        '
        Me.ckbLoop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbLoop.AutoSize = True
        Me.ckbLoop.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbLoop.Location = New System.Drawing.Point(5, 13)
        Me.ckbLoop.Margin = New System.Windows.Forms.Padding(0)
        Me.ckbLoop.Name = "ckbLoop"
        Me.ckbLoop.Size = New System.Drawing.Size(15, 14)
        Me.ckbLoop.TabIndex = 8
        Me.ckbLoop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ckbLoop.UseVisualStyleBackColor = True
        '
        'grpClear
        '
        Me.grpClear.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpClear.Controls.Add(Me.ckbClear)
        Me.grpClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpClear.Location = New System.Drawing.Point(75, 0)
        Me.grpClear.Margin = New System.Windows.Forms.Padding(0)
        Me.grpClear.Name = "grpClear"
        Me.grpClear.Padding = New System.Windows.Forms.Padding(0)
        Me.grpClear.Size = New System.Drawing.Size(25, 36)
        Me.grpClear.TabIndex = 9
        Me.grpClear.TabStop = False
        Me.grpClear.Text = "C"
        '
        'ckbClear
        '
        Me.ckbClear.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ckbClear.AutoSize = True
        Me.ckbClear.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ckbClear.Location = New System.Drawing.Point(5, 13)
        Me.ckbClear.Margin = New System.Windows.Forms.Padding(0)
        Me.ckbClear.Name = "ckbClear"
        Me.ckbClear.Size = New System.Drawing.Size(15, 14)
        Me.ckbClear.TabIndex = 9
        Me.ckbClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ckbClear.UseVisualStyleBackColor = True
        '
        'layoutHeaderTable
        '
        Me.layoutHeaderTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.layoutHeaderTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.layoutHeaderTable.ColumnCount = 2
        Me.layoutHeaderTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.layoutHeaderTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutHeaderTable.Controls.Add(Me.lblExpand, 0, 2)
        Me.layoutHeaderTable.Controls.Add(Me.cmbToggleButton, 0, 0)
        Me.layoutHeaderTable.Controls.Add(Me.pbPlayed, 1, 1)
        Me.layoutHeaderTable.Controls.Add(Me.layoutInfos, 1, 2)
        Me.layoutHeaderTable.Controls.Add(Me.layoutName, 1, 0)
        Me.layoutHeaderTable.Location = New System.Drawing.Point(0, 0)
        Me.layoutHeaderTable.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutHeaderTable.MinimumSize = New System.Drawing.Size(260, 65)
        Me.layoutHeaderTable.Name = "layoutHeaderTable"
        Me.layoutHeaderTable.RowCount = 3
        Me.layoutHeaderTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.layoutHeaderTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.layoutHeaderTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.layoutHeaderTable.Size = New System.Drawing.Size(370, 65)
        Me.layoutHeaderTable.TabIndex = 1
        '
        'lblExpand
        '
        Me.lblExpand.AutoEllipsis = True
        Me.lblExpand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblExpand.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpand.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblExpand.Location = New System.Drawing.Point(0, 35)
        Me.lblExpand.Margin = New System.Windows.Forms.Padding(0)
        Me.lblExpand.Name = "lblExpand"
        Me.lblExpand.Size = New System.Drawing.Size(24, 30)
        Me.lblExpand.TabIndex = 9
        Me.lblExpand.Text = "+"
        Me.lblExpand.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'cmbToggleButton
        '
        Me.cmbToggleButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmbToggleButton.FlatAppearance.BorderSize = 0
        Me.cmbToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbToggleButton.Location = New System.Drawing.Point(0, 0)
        Me.cmbToggleButton.Margin = New System.Windows.Forms.Padding(0)
        Me.cmbToggleButton.Name = "cmbToggleButton"
        Me.cmbToggleButton.Padding = New System.Windows.Forms.Padding(1)
        Me.layoutHeaderTable.SetRowSpan(Me.cmbToggleButton, 2)
        Me.cmbToggleButton.Size = New System.Drawing.Size(24, 25)
        Me.cmbToggleButton.TabIndex = 0
        Me.cmbToggleButton.UseVisualStyleBackColor = True
        '
        'pbPlayed
        '
        Me.pbPlayed.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPlayed.Location = New System.Drawing.Point(24, 25)
        Me.pbPlayed.Margin = New System.Windows.Forms.Padding(0)
        Me.pbPlayed.Name = "pbPlayed"
        Me.pbPlayed.Size = New System.Drawing.Size(346, 10)
        Me.pbPlayed.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbPlayed.TabIndex = 11
        '
        'layoutInfos
        '
        Me.layoutInfos.Controls.Add(Me.grbChannelLayer)
        Me.layoutInfos.Controls.Add(Me.grbDelay)
        Me.layoutInfos.Controls.Add(Me.grbDuration)
        Me.layoutInfos.Controls.Add(Me.grpPosition)
        Me.layoutInfos.Controls.Add(Me.grpRemaining)
        Me.layoutInfos.Controls.Add(Me.layoutButton)
        Me.layoutInfos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutInfos.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.layoutInfos.Location = New System.Drawing.Point(24, 35)
        Me.layoutInfos.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutInfos.Name = "layoutInfos"
        Me.layoutInfos.Size = New System.Drawing.Size(346, 30)
        Me.layoutInfos.TabIndex = 5
        '
        'grbChannelLayer
        '
        Me.grbChannelLayer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbChannelLayer.AutoSize = True
        Me.grbChannelLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbChannelLayer.Location = New System.Drawing.Point(0, 0)
        Me.grbChannelLayer.Margin = New System.Windows.Forms.Padding(0)
        Me.grbChannelLayer.Name = "grbChannelLayer"
        Me.grbChannelLayer.Padding = New System.Windows.Forms.Padding(0)
        Me.grbChannelLayer.Size = New System.Drawing.Size(0, 36)
        Me.grbChannelLayer.TabIndex = 7
        Me.grbChannelLayer.TabStop = False
        Me.grbChannelLayer.Text = "Channel-Layer"
        '
        'grbDelay
        '
        Me.grbDelay.Controls.Add(Me.txtDelay)
        Me.grbDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbDelay.Location = New System.Drawing.Point(0, 0)
        Me.grbDelay.Margin = New System.Windows.Forms.Padding(0)
        Me.grbDelay.Name = "grbDelay"
        Me.grbDelay.Padding = New System.Windows.Forms.Padding(0)
        Me.grbDelay.Size = New System.Drawing.Size(60, 35)
        Me.grbDelay.TabIndex = 0
        Me.grbDelay.TabStop = False
        Me.grbDelay.Text = "Delay"
        '
        'txtDelay
        '
        Me.txtDelay.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDelay.Location = New System.Drawing.Point(0, 11)
        Me.txtDelay.Margin = New System.Windows.Forms.Padding(0)
        Me.txtDelay.MaxLength = 11
        Me.txtDelay.Name = "txtDelay"
        Me.txtDelay.Size = New System.Drawing.Size(60, 18)
        Me.txtDelay.TabIndex = 4
        Me.txtDelay.Text = "00:00:00.00"
        Me.txtDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDelay.WordWrap = False
        '
        'grbDuration
        '
        Me.grbDuration.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grbDuration.Controls.Add(Me.txtDuration)
        Me.grbDuration.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbDuration.Location = New System.Drawing.Point(60, 0)
        Me.grbDuration.Margin = New System.Windows.Forms.Padding(0)
        Me.grbDuration.Name = "grbDuration"
        Me.grbDuration.Padding = New System.Windows.Forms.Padding(0)
        Me.grbDuration.Size = New System.Drawing.Size(60, 36)
        Me.grbDuration.TabIndex = 1
        Me.grbDuration.TabStop = False
        Me.grbDuration.Text = "Duration"
        '
        'txtDuration
        '
        Me.txtDuration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDuration.Location = New System.Drawing.Point(0, 11)
        Me.txtDuration.Margin = New System.Windows.Forms.Padding(0)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(60, 18)
        Me.txtDuration.TabIndex = 5
        Me.txtDuration.Text = "+00:00:00.00"
        Me.txtDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDuration.WordWrap = False
        '
        'grpPosition
        '
        Me.grpPosition.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpPosition.Controls.Add(Me.txtPosition)
        Me.grpPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpPosition.Location = New System.Drawing.Point(120, 0)
        Me.grpPosition.Margin = New System.Windows.Forms.Padding(0)
        Me.grpPosition.Name = "grpPosition"
        Me.grpPosition.Padding = New System.Windows.Forms.Padding(0)
        Me.grpPosition.Size = New System.Drawing.Size(60, 36)
        Me.grpPosition.TabIndex = 2
        Me.grpPosition.TabStop = False
        Me.grpPosition.Text = "Position"
        '
        'txtPosition
        '
        Me.txtPosition.BackColor = System.Drawing.Color.White
        Me.txtPosition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtPosition.Location = New System.Drawing.Point(0, 11)
        Me.txtPosition.Margin = New System.Windows.Forms.Padding(0)
        Me.txtPosition.Name = "txtPosition"
        Me.txtPosition.ReadOnly = True
        Me.txtPosition.Size = New System.Drawing.Size(60, 18)
        Me.txtPosition.TabIndex = 0
        Me.txtPosition.TabStop = False
        Me.txtPosition.Text = "+00:00:00.00"
        Me.txtPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPosition.WordWrap = False
        '
        'grpRemaining
        '
        Me.grpRemaining.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRemaining.Controls.Add(Me.txtRemaining)
        Me.grpRemaining.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRemaining.Location = New System.Drawing.Point(180, 0)
        Me.grpRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.grpRemaining.Name = "grpRemaining"
        Me.grpRemaining.Padding = New System.Windows.Forms.Padding(0)
        Me.grpRemaining.Size = New System.Drawing.Size(66, 36)
        Me.grpRemaining.TabIndex = 3
        Me.grpRemaining.TabStop = False
        Me.grpRemaining.Text = "Remaining"
        '
        'txtRemaining
        '
        Me.txtRemaining.BackColor = System.Drawing.Color.White
        Me.txtRemaining.Location = New System.Drawing.Point(1, 11)
        Me.txtRemaining.Margin = New System.Windows.Forms.Padding(0)
        Me.txtRemaining.Name = "txtRemaining"
        Me.txtRemaining.ReadOnly = True
        Me.txtRemaining.Size = New System.Drawing.Size(60, 18)
        Me.txtRemaining.TabIndex = 0
        Me.txtRemaining.TabStop = False
        Me.txtRemaining.Text = "+00:00:00.00"
        Me.txtRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
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
        Me.layoutName.Location = New System.Drawing.Point(24, 0)
        Me.layoutName.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutName.MinimumSize = New System.Drawing.Size(70, 27)
        Me.layoutName.Name = "layoutName"
        Me.layoutName.Size = New System.Drawing.Size(346, 27)
        Me.layoutName.TabIndex = 4
        '
        'nudLayer
        '
        Me.nudLayer.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.nudLayer.AutoSize = True
        Me.nudLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudLayer.Location = New System.Drawing.Point(303, 3)
        Me.nudLayer.Margin = New System.Windows.Forms.Padding(0)
        Me.nudLayer.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudLayer.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudLayer.Name = "nudLayer"
        Me.nudLayer.Size = New System.Drawing.Size(41, 20)
        Me.nudLayer.TabIndex = 3
        '
        'nudChannel
        '
        Me.nudChannel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.nudChannel.AutoSize = True
        Me.nudChannel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudChannel.Location = New System.Drawing.Point(261, 3)
        Me.nudChannel.Margin = New System.Windows.Forms.Padding(0)
        Me.nudChannel.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudChannel.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudChannel.Name = "nudChannel"
        Me.nudChannel.Size = New System.Drawing.Size(41, 20)
        Me.nudChannel.TabIndex = 2
        Me.nudChannel.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtName
        '
        Me.txtName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.txtName.Location = New System.Drawing.Point(2, 3)
        Me.txtName.Margin = New System.Windows.Forms.Padding(0)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(258, 20)
        Me.txtName.TabIndex = 1
        Me.txtName.Text = "Name"
        '
        'layoutHeaderContentSplit
        '
        Me.layoutHeaderContentSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutHeaderContentSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.layoutHeaderContentSplit.IsSplitterFixed = True
        Me.layoutHeaderContentSplit.Location = New System.Drawing.Point(0, 0)
        Me.layoutHeaderContentSplit.Margin = New System.Windows.Forms.Padding(0)
        Me.layoutHeaderContentSplit.MinimumSize = New System.Drawing.Size(260, 65)
        Me.layoutHeaderContentSplit.Name = "layoutHeaderContentSplit"
        Me.layoutHeaderContentSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'layoutHeaderContentSplit.Panel1
        '
        Me.layoutHeaderContentSplit.Panel1.Controls.Add(Me.layoutHeaderTable)
        Me.layoutHeaderContentSplit.Panel1MinSize = 65
        '
        'layoutHeaderContentSplit.Panel2
        '
        Me.layoutHeaderContentSplit.Panel2.Controls.Add(Me.layoutContentSplit)
        Me.layoutHeaderContentSplit.Panel2MinSize = 0
        Me.layoutHeaderContentSplit.Size = New System.Drawing.Size(370, 150)
        Me.layoutHeaderContentSplit.SplitterDistance = 65
        Me.layoutHeaderContentSplit.TabIndex = 2
        Me.layoutHeaderContentSplit.TabStop = False
        '
        'PlaylistView
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.layoutHeaderContentSplit)
        Me.MinimumSize = New System.Drawing.Size(370, 65)
        Me.Name = "PlaylistView"
        Me.Size = New System.Drawing.Size(370, 150)
        Me.layoutContentSplit.Panel2.ResumeLayout(False)
        Me.layoutContentSplit.Panel2.PerformLayout()
        CType(Me.layoutContentSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutContentSplit.ResumeLayout(False)
        Me.layoutButton.ResumeLayout(False)
        Me.grpParallel.ResumeLayout(False)
        Me.grpParallel.PerformLayout()
        Me.grpAuto.ResumeLayout(False)
        Me.grpAuto.PerformLayout()
        Me.grpLoop.ResumeLayout(False)
        Me.grpLoop.PerformLayout()
        Me.grpClear.ResumeLayout(False)
        Me.grpClear.PerformLayout()
        Me.layoutHeaderTable.ResumeLayout(False)
        Me.layoutHeaderTable.PerformLayout()
        Me.layoutInfos.ResumeLayout(False)
        Me.layoutInfos.PerformLayout()
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
    Friend WithEvents layoutButton As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ckbParallel As System.Windows.Forms.CheckBox
    Friend WithEvents ckbAuto As System.Windows.Forms.CheckBox
    Friend WithEvents ckbLoop As System.Windows.Forms.CheckBox
    Friend WithEvents layoutChild As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents layoutHeaderTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblExpand As System.Windows.Forms.Label
    Friend WithEvents cmbToggleButton As System.Windows.Forms.Button
    Friend WithEvents pbPlayed As System.Windows.Forms.ProgressBar
    Friend WithEvents layoutInfos As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents grbChannelLayer As System.Windows.Forms.GroupBox
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
    Friend WithEvents grbDelay As System.Windows.Forms.GroupBox
    Friend WithEvents txtDelay As System.Windows.Forms.TextBox
    Friend WithEvents grpParallel As System.Windows.Forms.GroupBox
    Friend WithEvents grpAuto As System.Windows.Forms.GroupBox
    Friend WithEvents grpLoop As System.Windows.Forms.GroupBox
    Friend WithEvents grpClear As System.Windows.Forms.GroupBox
    Friend WithEvents ckbClear As System.Windows.Forms.CheckBox

End Class
