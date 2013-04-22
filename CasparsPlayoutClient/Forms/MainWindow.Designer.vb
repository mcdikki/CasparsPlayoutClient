<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWindow
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        RemoveHandler sc.getTicker.frameTick, AddressOf onTick
        sc.close()
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
        Me.layoutPlaylistSplit = New System.Windows.Forms.SplitContainer()
        Me.layoutUpDownSplit = New System.Windows.Forms.SplitContainer()
        Me.layoutCgLib = New System.Windows.Forms.SplitContainer()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.ListView3 = New System.Windows.Forms.ListView()
        CType(Me.layoutPlaylistSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutPlaylistSplit.Panel2.SuspendLayout()
        Me.layoutPlaylistSplit.SuspendLayout()
        CType(Me.layoutUpDownSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutUpDownSplit.Panel1.SuspendLayout()
        Me.layoutUpDownSplit.Panel2.SuspendLayout()
        Me.layoutUpDownSplit.SuspendLayout()
        CType(Me.layoutCgLib, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.layoutCgLib.Panel1.SuspendLayout()
        Me.layoutCgLib.Panel2.SuspendLayout()
        Me.layoutCgLib.SuspendLayout()
        Me.SuspendLayout()
        '
        'layoutPlaylistSplit
        '
        Me.layoutPlaylistSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutPlaylistSplit.Location = New System.Drawing.Point(0, 0)
        Me.layoutPlaylistSplit.Name = "layoutPlaylistSplit"
        Me.layoutPlaylistSplit.Panel1MinSize = 260
        '
        'layoutPlaylistSplit.Panel2
        '
        Me.layoutPlaylistSplit.Panel2.Controls.Add(Me.layoutUpDownSplit)
        Me.layoutPlaylistSplit.Size = New System.Drawing.Size(784, 562)
        Me.layoutPlaylistSplit.SplitterDistance = 260
        Me.layoutPlaylistSplit.TabIndex = 0
        '
        'layoutUpDownSplit
        '
        Me.layoutUpDownSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutUpDownSplit.Location = New System.Drawing.Point(0, 0)
        Me.layoutUpDownSplit.Name = "layoutUpDownSplit"
        Me.layoutUpDownSplit.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'layoutUpDownSplit.Panel1
        '
        Me.layoutUpDownSplit.Panel1.Controls.Add(Me.layoutCgLib)
        '
        'layoutUpDownSplit.Panel2
        '
        Me.layoutUpDownSplit.Panel2.Controls.Add(Me.ListView3)
        Me.layoutUpDownSplit.Size = New System.Drawing.Size(520, 562)
        Me.layoutUpDownSplit.SplitterDistance = 426
        Me.layoutUpDownSplit.TabIndex = 0
        '
        'layoutCgLib
        '
        Me.layoutCgLib.Dock = System.Windows.Forms.DockStyle.Fill
        Me.layoutCgLib.Location = New System.Drawing.Point(0, 0)
        Me.layoutCgLib.Name = "layoutCgLib"
        '
        'layoutCgLib.Panel1
        '
        Me.layoutCgLib.Panel1.Controls.Add(Me.ListView1)
        '
        'layoutCgLib.Panel2
        '
        Me.layoutCgLib.Panel2.Controls.Add(Me.ListView2)
        Me.layoutCgLib.Size = New System.Drawing.Size(520, 426)
        Me.layoutCgLib.SplitterDistance = 375
        Me.layoutCgLib.TabIndex = 0
        '
        'ListView1
        '
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.Location = New System.Drawing.Point(0, 0)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(375, 426)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'ListView2
        '
        Me.ListView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView2.Location = New System.Drawing.Point(0, 0)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(141, 426)
        Me.ListView2.TabIndex = 0
        Me.ListView2.UseCompatibleStateImageBehavior = False
        '
        'ListView3
        '
        Me.ListView3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView3.Location = New System.Drawing.Point(0, 0)
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Size = New System.Drawing.Size(520, 132)
        Me.ListView3.TabIndex = 0
        Me.ListView3.UseCompatibleStateImageBehavior = False
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.layoutPlaylistSplit)
        Me.Name = "MainWindow"
        Me.Text = "MainWindow"
        Me.layoutPlaylistSplit.Panel2.ResumeLayout(False)
        CType(Me.layoutPlaylistSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutPlaylistSplit.ResumeLayout(False)
        Me.layoutUpDownSplit.Panel1.ResumeLayout(False)
        Me.layoutUpDownSplit.Panel2.ResumeLayout(False)
        CType(Me.layoutUpDownSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutUpDownSplit.ResumeLayout(False)
        Me.layoutCgLib.Panel1.ResumeLayout(False)
        Me.layoutCgLib.Panel2.ResumeLayout(False)
        CType(Me.layoutCgLib, System.ComponentModel.ISupportInitialize).EndInit()
        Me.layoutCgLib.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents layoutPlaylistSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents layoutUpDownSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents layoutCgLib As System.Windows.Forms.SplitContainer
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ListView3 As System.Windows.Forms.ListView
End Class
