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
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports CasparCGNETConnector
Imports logger

Public Class MainWindow

    Private WithEvents sc As ServerController
    Private mediaLib As Library
    Private WithEvents playlistView As PlaylistView
    Private WithEvents libraryView As LibraryView
    Private Delegate Sub updateDelegate()
    Private Delegate Sub updateStatusDelegate(ByVal message As message)
    Private timer As Timers.Timer
    Private logShowTime As Integer = My.Settings.logShowTime
    Private settings As New SettingsWindow()

    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Settings.logToConsole Then
            logger.addLogAction(New consoleLogger(My.Settings.loglevel))
        End If
        If My.Settings.logdir.Length = 0 Then My.Settings.logdir = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\log"
        If My.Settings.logToFile Then
            logger.addLogAction(New fileLogger(My.Settings.loglevel, My.Settings.logdir & "\" & My.Settings.logfile, True, False))
        End If
        sc = New ServerController

        loadConfig()
        mediaLib = New Library(sc)
        AddPlaylist()
        AddLibrary()
        initInfo()
        initMenu()
        layoutUpDownSplit.SplitterDistance = layoutUpDownSplit.Size.Height - layoutUpDownSplit.Panel2MinSize
    End Sub

    Private Sub loadConfig()
        ' Server and port
        If My.Settings.last_AcmpServer.Length > 0 Then
            Me.txtAddress.Text = My.Settings.last_AcmpServer
        Else
            Me.txtAddress.Text = sc.getServerAddress
        End If
        If My.Settings.last_AcmpPort.Length > 0 Then
            Me.txtPort.Text = My.Settings.last_AcmpPort
        Else
            Me.txtPort.Text = sc.getPort
        End If

        ' playlist

        ' library
    End Sub

    Private Sub AddLibrary()
        libraryView = New LibraryView(mediaLib)
        libraryView.Dock = DockStyle.Fill
        layoutCgLib.Panel2MinSize = libraryView.MinimumSize.Width
        layoutCgLib.SplitterDistance = layoutCgLib.Width - libraryView.MinimumSize.Width - layoutCgLib.SplitterWidth
        layoutCgLib.Panel2.Controls.Add(libraryView)
    End Sub

    Private Sub AddPlaylist()
        playlistView = New PlaylistView(sc.getPlaylistRoot)
        playlistView.Dock = DockStyle.Fill
        layoutPlaylistSplit.Panel1MinSize = playlistView.MinimumSize.Width
        playlistView.Parent = layoutPlaylistSplit.Panel1
    End Sub

    Private Sub connect() Handles cmbConnect.Click
        If Not sc.isConnected Then
            If sc.open(txtAddress.Text, Integer.Parse(txtPort.Text)) Then
                cmbConnect.Enabled = False
                For i = sc.getChannels To 1 Step -1
                    cbbClearChannel.Text = i
                    cbbClearChannel.Items.Add(i)
                Next
                AddHandler sc.getTicker.frameTick, AddressOf onTick
                sc.startTicker()
                libraryView.cmbRefresh.PerformClick()
                cmbDisconnect.Enabled = True
            Else
                cmbConnect.Enabled = True
                cmbDisconnect.Enabled = False
                'logger.warn("Could not connect to " & txtAddress.Text & ":" & txtPort.Text)
                'MsgBox("Error: Could not connect to " & txtAddress.Text & ":" & txtPort.Text, vbCritical + vbOKOnly, "Error - not connected")
            End If
        Else
            MsgBox("Allready connected")
        End If
    End Sub

    Private Sub initMenu()

        Dim m As New MenuStrip()
        Dim fm As New ToolStripMenuItem("File")
        Dim em As New ToolStripMenuItem("Extra")

        fm.DropDownItems.Add("Load playlist", Nothing, New EventHandler(AddressOf playlistView.loadPlaylist))
        fm.DropDownItems.Add("Save playlist", Nothing, New EventHandler(AddressOf playlistView.savePlaylist))
        fm.DropDownItems.Add(New ToolStripSeparator)
        fm.DropDownItems.Add("Exit", Nothing, New EventHandler(AddressOf Me.Close))

        em.DropDownItems.Add("Settings", Nothing, New EventHandler(Sub() settings.Show()))

        m.Items.Add(fm)
        m.Items.Add(em)

        Me.MainMenuStrip = m
        Me.Controls.Add(m)

    End Sub

    Private Sub initInfo()
        timer = New Timers.Timer(100)
        timer.Enabled = True
        ssLog.AllowMerge = False
        ssLog.CanOverflow = True
        ssLog.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow
        ssLog.ShowItemToolTips = True

        AddHandler timer.Elapsed, AddressOf updateClock
        AddHandler timer.Elapsed, AddressOf updateDate
        AddHandler timer.Elapsed, AddressOf updateStatus
        AddHandler logger.messageReceived, AddressOf updateStatusBar
        AddHandler ssLog.ItemRemoved, Sub() ssLog.Items.Item(0).Text = "Messages: " & ssLog.Items.Count - 1
        AddHandler ssLog.ItemAdded, Sub() ssLog.Items.Item(0).Text = "Messages: " & ssLog.Items.Count - 1
        ssLog.Items.Add("")
        ssLog.Items.Item(0).TextAlign = ContentAlignment.MiddleLeft
        updateClock()
        updateDate()
        updateStatus()
    End Sub

    Private Sub updateStatusBar(ByVal msg As message)
        If msg.getLevel < loglevels.debug Then
            If Me.ssLog.InvokeRequired Then
                Dim d As New updateStatusDelegate(AddressOf updateStatusBar)
                Me.ssLog.Invoke(d, msg)
            Else

                Dim item As New TimedStatusLable(logShowTime, msg.getMessage)
                item.BorderSides = ToolStripStatusLabelBorderSides.Top
                item.BorderStyle = Border3DStyle.Flat
                item.TextAlign = ContentAlignment.MiddleLeft
                ssLog.Items.Add(item)
            End If
        End If
    End Sub

    Private Sub ssLog_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ssLog.ItemClicked
        If ssLog.Items.IndexOf(e.ClickedItem) > 0 Then MsgBox(e.ClickedItem.Text)
    End Sub

    Private Sub updateClock()
        If lblClock.InvokeRequired Then
            Dim d = New updateDelegate(AddressOf updateClock)
            lblClock.Invoke(d)
        Else
            lblClock.Text = Now.TimeOfDay.ToString.Substring(0, 8)
        End If
    End Sub

    Private Sub updateDate()
        If lblDate.InvokeRequired Then
            Dim d = New updateDelegate(AddressOf updateDate)
            lblDate.Invoke(d)
        Else
            lblDate.Text = Now.Date.ToShortDateString
        End If
    End Sub

    Private Sub updateStatus()
        If lblStatus.InvokeRequired Then
            Dim d = New updateDelegate(AddressOf updateStatus)
            lblStatus.Invoke(d)
        Else
            If sc.isConnected Then
                If sc.getPlaylistRoot.isPlaying OrElse sc.getPlaylistRoot.isWaiting Then
                    lblStatus.ForeColor = Color.DarkOrange
                    lblStatus.Text = "Running"
                Else
                    lblStatus.ForeColor = Color.Lime
                    lblStatus.Text = "Idle"
                End If
            Else
                lblStatus.Text = "Stopped"
                lblStatus.ForeColor = Color.Red
            End If
        End If
    End Sub

    Private Sub disconnect() Handles cmbDisconnect.Click
        If sc.isConnected Then
            libraryView.Library.abortUpdate()
            cmbDisconnect.Enabled = False
            sc.close()
            RemoveHandler sc.getTicker.frameTick, AddressOf onTick
            libraryView.Library.refreshLibrary()
            playlistView.onDataChanged()
            cmbConnect.Enabled = True
        End If
    End Sub

    Private Sub disconnected() Handles sc.disconnected
        cmbDisconnect.Enabled = False
        Try
            RemoveHandler sc.getTicker.frameTick, AddressOf onTick
        Catch e As Exception
        End Try
        libraryView.Library.refreshLibrary()
        playlistView.onDataChanged()
        cmbConnect.Enabled = True
    End Sub

    Private Sub clearAll() Handles cmdClearAll.Click
        Dim cmd As New ClearCommand()
        Dim p = CTypeDynamic(cmd.getParameter("channel"), cmd.getParameter("channel").getGenericParameterType)
        For i = 1 To sc.getChannels
            p.setValue(i)
            cmd.execute(sc.getCommandConnection)
        Next
    End Sub

    Private Sub clearChannel() Handles cmbClearChannel.Click
        If cbbClearChannel.Text.Length > 0 AndAlso IsNumeric(cbbClearChannel.Text) AndAlso sc.containsChannel(Integer.Parse(cbbClearChannel.Text)) Then
            Dim cmd As New ClearCommand(Integer.Parse(cbbClearChannel.Text))
            cmd.execute(sc.getCommandConnection)
        End If
    End Sub

    Private Sub clearLayer() Handles cmbClearLayer.Click
        If cbbClearChannel.Text.Length > 0 AndAlso IsNumeric(cbbClearChannel.Text) AndAlso sc.containsChannel(Integer.Parse(cbbClearChannel.Text)) Then
            Dim cmd As New ClearCommand(Integer.Parse(cbbClearChannel.Text), nudLayerClear.Value)
            cmd.execute(sc.getCommandConnection)
        End If
    End Sub

    Private Sub onTick()
        playlistView.onDataChanged()
    End Sub

End Class