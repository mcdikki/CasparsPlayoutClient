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
    Private clock As New ToolStripLabel
    Private progress As New ToolStripProgressBar
    Private state As New ToolStripLabel

    Private Sub MainWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Settings.logToConsole Then
            logger.addLogAction(New consoleLogger(My.Settings.loglevel))
        End If
        If My.Settings.logdir.Length = 0 Then My.Settings.logdir = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\log"
        If My.Settings.logToFile Then
            logger.addLogAction(New fileLogger(My.Settings.loglevel, My.Settings.logdir & "\" & My.Settings.logfile, True, False))
        End If
        logger.log("Starting " & My.Application.Info.ProductName & " Version " & My.Application.Info.Version.ToString & " on " & My.Computer.Info.OSFullName)
        sc = New ServerController

        loadConfig()
        mediaLib = New Library(sc)
        AddPlaylist()
        AddLibrary()
        initMenu()
        initInfo()
        layoutUpDownSplit.SplitterDistance = layoutUpDownSplit.Size.Height - layoutUpDownSplit.Panel2MinSize
    End Sub

    Private Sub _onClosing() Handles MyClass.FormClosing
        timer.Stop()
        mediaLib.abortUpdate()
        If Not IsNothing(sc.getTicker) Then RemoveHandler sc.getTicker.frameTick, AddressOf onTick
        If Not IsNothing(sc) And sc.isConnected Then sc.close()
        logger.close()
        If My.Settings.rememberConnection Then
            My.Settings.last_AcmpPort = txtPort.Text
            My.Settings.last_AcmpServer = txtAddress.Text
        End If
        If My.Settings.rememberPlaylist Then My.Settings.last_Playlist = sc.getPlaylistRoot.toXMLString
        saveLayout()
        My.Settings.Save()
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
        If My.Settings.rememberLayout Then restoreLayout()
    End Sub

    Private Sub restoreLayout()
        WindowState = My.Settings.last_WindowState
        If My.Settings.last_WindowState = FormWindowState.Normal Then
            Size = My.Settings.last_WindowSize
            Location = My.Settings.last_WindowLocation
        End If
        'layoutPlaylistSplit.SplitterDistance = Math.Max(My.Settings.last_PlaylistWidth, layoutPlaylistSplit.Panel1MinSize)
        'layoutUpDownSplit.SplitterDistance = Math.Max(My.Settings.last_CGHeight, layoutUpDownSplit.Panel1MinSize)
        'layoutCgLib.SplitterDistance = Math.Max(My.Settings.last_LibraryWidth, layoutCgLib.Panel1MinSize)
        layoutPlaylistSplit.SplitterDistance = My.Settings.last_PlaylistWidth
        layoutUpDownSplit.SplitterDistance = My.Settings.last_CGHeight
        layoutCgLib.SplitterDistance = My.Settings.last_LibraryWidth
    End Sub

    Private Sub saveLayout()
        ' MainWindow
        My.Settings.last_WindowState = WindowState
        My.Settings.last_WindowSize = Size
        My.Settings.last_WindowLocation = Location

        'Controls
        My.Settings.last_PlaylistWidth = layoutPlaylistSplit.SplitterDistance
        My.Settings.last_CGHeight = layoutUpDownSplit.SplitterDistance
        My.Settings.last_LibraryWidth = layoutCgLib.SplitterDistance
    End Sub

    Private Sub AddLibrary()
        libraryView = New LibraryView(mediaLib)
        libraryView.Dock = DockStyle.Fill
        layoutCgLib.Panel2MinSize = libraryView.MinimumSize.Width
        layoutCgLib.SplitterDistance = layoutCgLib.Width - libraryView.MinimumSize.Width - layoutCgLib.SplitterWidth
        layoutCgLib.Panel2.Controls.Add(libraryView)
    End Sub

    Private Sub AddPlaylist()
        playlistView = New PlaylistView(sc.getPlaylistRoot, My.Settings.defaultPlaylistCollapsed)
        playlistView.Dock = DockStyle.Fill
        layoutPlaylistSplit.Panel1MinSize = playlistView.MinimumSize.Width
        playlistView.Parent = layoutPlaylistSplit.Panel1
    End Sub

    Private Sub initMenu()

        Dim m As New MenuStrip()
        Dim fm As New ToolStripMenuItem("&File")
        Dim em As New ToolStripMenuItem("&Extra")
        Dim hm As New ToolStripMenuItem("&?")

        fm.DropDownItems.Add("Load playlist", Nothing, New EventHandler(AddressOf playlistView.loadPlaylist))
        fm.DropDownItems.Add("Save playlist", Nothing, New EventHandler(AddressOf playlistView.savePlaylist))
        fm.DropDownItems.Add(New ToolStripSeparator)
        fm.DropDownItems.Add("Connect/Disconnect", Nothing, New EventHandler(AddressOf connect))
        fm.DropDownItems.Add(New ToolStripSeparator)
        fm.DropDownItems.Add("Exit", Nothing, New EventHandler(AddressOf Me.Close))

        em.DropDownItems.Add("Settings", Nothing, New EventHandler(AddressOf showSettings))

        Dim about As String
        With My.Application.Info
            about = .ProductName & vbNewLine & _
                "Version " & .Version.ToString & vbNewLine & vbNewLine & _
                .Description & vbNewLine & vbNewLine & _
                "by " & .CompanyName & vbNewLine & _
                .Copyright & vbNewLine & vbNewLine & _
                "Loaded assemblys:"
            For Each assembly In .LoadedAssemblies
                about = about & vbNewLine & vbTab & assembly.FullName
            Next
        End With
        hm.DropDownItems.Add("About", Nothing, New EventHandler(Sub() MsgBox(about, MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "About " & My.Application.Info.Title)))

        m.Items.Add(fm)
        m.Items.Add(em)
        m.Items.Add(hm)


        clock.Alignment = ToolStripItemAlignment.Right
        m.Items.Add(clock)

        progress.Alignment = ToolStripItemAlignment.Right
        progress.Maximum = 100
        progress.Minimum = 0
        m.Items.Add(progress)

        state.Alignment = ToolStripItemAlignment.Right
        m.Items.Add(state)

        Me.MainMenuStrip = m
        Me.Controls.Add(m)

    End Sub

    Private Sub showSettings()
        Dim settings As New SettingsWindow
        settings.Show()
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
        AddHandler timer.Elapsed, AddressOf updateMenu
        AddHandler logger.messageReceived, AddressOf updateStatusBar
        AddHandler ssLog.ItemRemoved, Sub() ssLog.Items.Item(0).Text = "Messages: " & ssLog.Items.Count - 1
        AddHandler ssLog.ItemAdded, Sub() ssLog.Items.Item(0).Text = "Messages: " & ssLog.Items.Count - 1
        ssLog.Items.Add("")
        ssLog.Items.Item(0).TextAlign = ContentAlignment.MiddleLeft
        updateClock()
        updateDate()
        updateStatus()
        updateMenu()
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

    Private Sub updateMenu()
        If MainMenuStrip.InvokeRequired Then
            Dim d = New updateDelegate(AddressOf updateMenu)
            MainMenuStrip.Invoke(d)
        Else
            progress.Value = sc.getPlaylistRoot.getPlayed
            clock.Text = Now
            If sc.isConnected Then
                If sc.getPlaylistRoot.isPlaying OrElse sc.getPlaylistRoot.isWaiting Then
                    state.Text = "ON AIR - Playing"
                    state.ForeColor = Color.DarkOrange
                Else
                    state.Text = "ON AIR - Stopped"
                    state.ForeColor = Color.Red
                End If
            Else
                state.Text = "Disconnected"
                state.ForeColor = Color.Lime
            End If
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
                    lblStatus.Text = "ON AIR - Playing"
                Else
                    lblStatus.ForeColor = Color.Red
                    lblStatus.Text = "ON AIR - Stopped"
                End If
            Else
                lblStatus.Text = "Disconnected"
                lblStatus.ForeColor = Color.Lime
            End If
        End If
    End Sub

    Private Sub connect() Handles cmbConnect.Click
        If sc.isConnected Then
            mediaLib.abortUpdate()
            Dim t As New Threading.Thread(AddressOf sc.close)
            t.Start()
            cmbConnect.Enabled = False
            cmbConnect.Text = "disconnecting..."
        Else
            Dim t As New Threading.Thread(Sub() sc.open(txtAddress.Text, Integer.Parse(txtPort.Text)))
            t.Start()
            cmbConnect.Enabled = False
            cmbConnect.Text = "connecting..."
        End If
    End Sub

    Private Sub connected() Handles sc.connected
        If InvokeRequired Then
            Dim d As New updateDelegate(AddressOf connected)
            Invoke(d)
        Else
            For i = sc.getChannels To 1 Step -1
                cbbClearChannel.Text = i
                cbbClearChannel.Items.Add(i)
            Next
            AddHandler sc.getTicker.frameTick, AddressOf onTick
            sc.startTicker()
            'libraryView.cmbRefresh.PerformClick()
            cmbConnect.Text = "Disconnect"
            cmbConnect.Enabled = True
        End If
    End Sub

    Private Sub disconnected() Handles sc.disconnected
        If InvokeRequired Then
            Dim d As New updateDelegate(AddressOf disconnected)
            Invoke(d)
        Else
            Try
                RemoveHandler sc.getTicker.frameTick, AddressOf onTick
            Catch e As Exception
            End Try
            'libraryView.Library.refreshLibrary()
            playlistView.onDataChanged()
            cmbConnect.Text = "Connect"
            cmbConnect.Enabled = True
        End If
    End Sub

    Private Sub clearAll() Handles cmdClearAll.Click
        Dim cmd As New ClearCommand()
        Dim p = CTypeDynamic(cmd.getCommandParameter("channel"), cmd.getCommandParameter("channel").getGenericParameterType)
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