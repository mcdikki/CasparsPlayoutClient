<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWindow
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
        Me.playlistLayout = New System.Windows.Forms.FlowLayoutPanel()
        Me.cmbAddPlaylist = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'playlistLayout
        '
        Me.playlistLayout.AutoScroll = True
        Me.playlistLayout.Location = New System.Drawing.Point(15, 12)
        Me.playlistLayout.Name = "playlistLayout"
        Me.playlistLayout.Size = New System.Drawing.Size(424, 522)
        Me.playlistLayout.TabIndex = 0
        '
        'cmbAddPlaylist
        '
        Me.cmbAddPlaylist.Location = New System.Drawing.Point(452, 12)
        Me.cmbAddPlaylist.Name = "cmbAddPlaylist"
        Me.cmbAddPlaylist.Size = New System.Drawing.Size(287, 63)
        Me.cmbAddPlaylist.TabIndex = 1
        Me.cmbAddPlaylist.Text = "Add Playlist"
        Me.cmbAddPlaylist.UseVisualStyleBackColor = True
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(751, 537)
        Me.Controls.Add(Me.cmbAddPlaylist)
        Me.Controls.Add(Me.playlistLayout)
        Me.Name = "MainWindow"
        Me.Text = "MainWindow"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents playlistLayout As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cmbAddPlaylist As System.Windows.Forms.Button
End Class
