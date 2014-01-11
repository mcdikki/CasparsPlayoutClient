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

Imports System.Threading
Imports CasparCGNETConnector
Imports logger


Public Class ServerController

    Public readyForUpdate As New Semaphore(1, 1)
    Private WithEvents cmdConnection As CasparCGConnection
    Private WithEvents updateConnection As CasparCGConnection
    Private WithEvents testConnection As CasparCGConnection
    Private tickThread As Thread
    Private serverAddress As String = My.Settings.defaultAcmpServer
    Private serverPort As Integer = My.Settings.deafaultAcmpPort
    Private testChannel As Integer = 2
    Private framesPerTick As Integer = My.Settings.frameTickInterval
    Private channels As Integer
    Private channelFPS() As Integer
    Private opened As Boolean
    Private WithEvents ticker As FrameTicker
    Private updater As AbstractMediaUpdater
    Private playlist As IPlaylistItem ' Die Root Playlist unter die alle anderen kommen
    Private openLock As New Mutex

    Public Event disconnected()
    Public Event connected()

    Public Sub New()
        If My.Settings.playlistdir.Length = 0 Then My.Settings.playlistdir = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\playlist"
        If My.Settings.rememberPlaylist AndAlso My.Settings.last_Playlist.Length > 0 Then
            Dim xmldoc As New MSXML2.DOMDocument
            If xmldoc.loadXML(My.Settings.last_Playlist) Then
                playlist = PlaylistFactory.getPlaylist(xmldoc, Me)
            Else
                playlist = New PlaylistBlockItem("Playlist", Me, 1, 0)
                playlist.setParallel(True)
            End If
        Else
            playlist = New PlaylistBlockItem("Playlist", Me, 1, 0)
            playlist.setParallel(True)
        End If
    End Sub

    Public Function open() As Boolean
        Return open(serverAddress, serverPort)
    End Function

    Public Sub close()
        logger.debug("ServerController.close: Close servercontroller...")
        opened = False

        RemoveHandler testConnection.disconnected, AddressOf Me.handleDisconnect
        RemoveHandler updateConnection.disconnected, AddressOf Me.handleDisconnect
        RemoveHandler cmdConnection.disconnected, AddressOf Me.handleDisconnect

        If Not IsNothing(updater) Then
            updater.stopUpdate()
            updater = Nothing
        End If
        If Not IsNothing(ticker) Then
            ticker.stopTicker()
        End If

        If Not IsNothing(cmdConnection) AndAlso cmdConnection.isConnected Then cmdConnection.close()
        If Not IsNothing(updateConnection) AndAlso updateConnection.isConnected Then updateConnection.close()
        If Not IsNothing(testConnection) AndAlso testConnection.isConnected Then testConnection.close()
        channels = 0
        ReDim channelFPS(0)
        RaiseEvent disconnected()
    End Sub

    Private Sub handleDisconnect(ByRef sender As Object)
        If isOpen() Then
            Me.close()
            If My.Settings.autoReconnect AndAlso openLock.WaitOne(5) Then
                While Not open()
                    Thread.Sleep(My.Settings.checkConnectionInterval)
                End While
                openLock.ReleaseMutex()
            End If
        End If
    End Sub

    Public Function isOpen() As Boolean
        Return opened
    End Function

    Public Function isConnected() As Boolean
        Return Not IsNothing(updateConnection) AndAlso Not _
            IsNothing(cmdConnection) AndAlso Not IsNothing(testConnection) AndAlso _
            updateConnection.isConnected AndAlso _
            cmdConnection.isConnected AndAlso testConnection.isConnected
    End Function

    Public Function open(ByVal serverAddress As String, ByVal severPort As Integer) As Boolean
        Me.serverAddress = serverAddress
        Me.serverPort = serverPort
        cmdConnection = New CasparCGConnection(serverAddress, serverPort)
        updateConnection = New CasparCGConnection(serverAddress, serverPort)
        testConnection = New CasparCGConnection(serverAddress, serverPort)

        cmdConnection.timeout = My.Settings.connectionTimeout
        cmdConnection.disconnectOnTimeout = My.Settings.disconnectAtTimeout
        cmdConnection.reconnectTimeout = My.Settings.reconnectTimout
        cmdConnection.reconnectTries = My.Settings.reconnectTries
        cmdConnection.checkInterval = My.Settings.checkConnectionInterval
        cmdConnection.strictVersionControl = My.Settings.strictVersionControl
        updateConnection.timeout = My.Settings.connectionTimeout
        updateConnection.disconnectOnTimeout = My.Settings.disconnectAtTimeout
        updateConnection.reconnectTimeout = My.Settings.reconnectTimout
        updateConnection.reconnectTries = My.Settings.reconnectTries
        updateConnection.checkInterval = My.Settings.checkConnectionInterval
        updateConnection.strictVersionControl = My.Settings.strictVersionControl
        testConnection.timeout = My.Settings.connectionTimeout
        testConnection.disconnectOnTimeout = My.Settings.disconnectAtTimeout
        testConnection.reconnectTimeout = My.Settings.reconnectTimout
        testConnection.reconnectTries = My.Settings.reconnectTries
        testConnection.checkInterval = My.Settings.checkConnectionInterval
        testConnection.strictVersionControl = My.Settings.strictVersionControl

        AddHandler testConnection.disconnected, AddressOf Me.handleDisconnect
        AddHandler updateConnection.disconnected, AddressOf Me.handleDisconnect
        AddHandler cmdConnection.disconnected, AddressOf Me.handleDisconnect

        If testConnection.connect() AndAlso updateConnection.connect() AndAlso cmdConnection.connect() Then
            opened = True
            ' Channels des Servers bestimmen
            channels = testConnection.getServerChannels
            testChannel = channels
            If channels > 1 Then
                channels = channels - 1
            End If

            ReDim channelFPS(channels)
            For c As Integer = 0 To channels - 1
                channelFPS(c) = getChannelFPS(c + 1)
            Next

            ' Tick  starten
            ticker = New frameTicker(Me, framesPerTick)

            ' updater starten
            updater = MediaUpdaterFactory.getMediaUpdater(updateConnection, playlist, Me)
            updater.startUpdate()

            RaiseEvent connected()
        Else
            RaiseEvent disconnected()
            opened = False
        End If
        Return opened
    End Function

    Public Function getPlaylistRoot() As IPlaylistItem
        Return playlist
    End Function

    Public Sub setPlaylistRoot(ByRef rootPlaylist As IPlaylistItem)
        If Not IsNothing(rootPlaylist) AndAlso rootPlaylist.getItemType.Equals(AbstractPlaylistItem.PlaylistItemTypes.BLOCK) Then
            playlist = rootPlaylist
        End If
    End Sub

    Public Function getCommandConnection() As CasparCGConnection
        Return cmdConnection
    End Function

    Public Function getTestConnection() As CasparCGConnection
        Return testConnection
    End Function

    Public Function getTestChannel() As Integer
        Return testChannel
    End Function

    Public Function getChannels() As Integer
        Return channels
    End Function

    Public Function getPort() As Integer
        Return serverPort
    End Function

    Public Function getServerAddress() As String
        Return serverAddress
    End Function

    ''' <summary>
    ''' Returns the media duration in milliseconds if playing in native fps.
    ''' </summary>
    ''' <param name="media"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getOriginalMediaDuration(ByRef media As AbstractCasparCGMedia) As Long
        Select Case media.getMediaType
            Case AbstractCasparCGMedia.MediaType.COLOR, AbstractCasparCGMedia.MediaType.STILL, AbstractCasparCGMedia.MediaType.TEMPLATE
                '' These mediatyps doesn't have any durations
                Return 0
            Case Else
                If media.containsInfo("file-nb-frames") AndAlso media.containsInfo("fps") AndAlso media.containsInfo("progressive") Then
                    Dim fps As Integer = Single.Parse(media.getInfo("fps").Trim.Substring(0, Math.Min(4, media.getInfo("fps").Trim.Length))) * 100
                    Dim progressive = Boolean.Parse(media.getInfo("progressive"))
                    'If Not progressive Then
                    '    fps = fps / 2
                    'End If
                    Return getFramesToMS(media.getInfo("file-nb-frames"), fps)
                End If
                logger.err("ServerController.getOriginalMediaDuration: Could not get media duration of " & media.getFullName & "(" & media.getMediaType.ToString & ").")
        End Select
        Return 0
    End Function

    ''' <summary>
    ''' Returns the media duration in milliseconds if playing at a given channel.
    ''' </summary>
    ''' <param name="media"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getMediaDuration(ByRef media As AbstractCasparCGMedia, ByVal channel As Integer) As Long

        'Return getOriginalMediaDuration(media)

        ''---> Scheint wohl doch nötig zu sein! CD 07/13
        '' Scheint nicht nötig zu sein da die videos IMMER so lange spielen wie sie sollen
        '' zumindest wenn die metadaten stimmen - aber erst noch ausgiebig testen!
        If isConnected() Then
            Select Case media.getMediaType
                Case AbstractCasparCGMedia.MediaType.COLOR, AbstractCasparCGMedia.MediaType.STILL, AbstractCasparCGMedia.MediaType.TEMPLATE
                    '' These mediatyps doesn't have any durations
                    Return 0
                Case Else
                    If media.getInfos.Count = 0 Then
                        '' no media info is loaded
                        '' load it now
                        'media.parseXML(getMediaInfo(media))
                        media.fillMediaInfo(testConnection, testChannel)
                    End If
                    If media.containsInfo("nb-frames") Then
                        Return getFramesToMS(media.getInfo("nb-frames"), getFPS(channel))
                    End If
                    logger.err("ServerController.getMediaDuration: Could not get media duration of " & media.getFullName & "(" & media.getMediaType.ToString & ").")
            End Select
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Returns whether or not, the given channel is configured at the connected CasparCGServer
    ''' </summary>
    ''' <param name="channel">the channel number to check for</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function containsChannel(ByVal channel As Integer) As Boolean
        Return channel <= channels AndAlso channel > 0
    End Function

    ''' <summary>
    ''' Returns the given number of milliseconds as a formated String "*hh:mm:ss.µµ" where h = hours, m = minutes, s = seconds and µ = milliseconds.
    ''' </summary>
    ''' <param name="milliseconds"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getTimeStringOfMS(ByVal milliseconds As Long) As String
        If Not milliseconds = 0 Then
            Dim neg As Boolean = False
            If milliseconds < 0 Then
                milliseconds = milliseconds * -1
                neg = True
            End If
            Dim h As String = Convert.ToUInt16(Math.Truncate(Math.Abs(milliseconds) / 3600000)).ToString("D2")
            Dim m As String = Convert.ToUInt16(Math.Truncate((Math.Abs(milliseconds) / 60000) Mod 60)).ToString("D2")
            Dim s As String = Convert.ToUInt16(Math.Truncate((Math.Abs(milliseconds) / 1000) Mod 60)).ToString("D2")
            Dim ms As String = Convert.ToUInt16(Math.Truncate(Math.Abs(milliseconds) Mod 1000)).ToString("D2").Substring(0, 2)
            If neg Then
                Return "-" & h & ":" & m & ":" & s & "." & ms
            Else
                Return h & ":" & m & ":" & s & "." & ms
            End If
        Else : Return "00:00:00.00"
        End If
    End Function

    Public Shared Function getMsToFrames(ByVal milliseconds As Long, ByVal fps As Integer) As Long
        If milliseconds = 0 Then
            Return 0
        Else
            Return (milliseconds * fps) / 100000
        End If
    End Function

    ''' <summary>
    ''' Returns the time in milliseconds needed to play the given number of frames at a specified framerate.
    ''' </summary>
    ''' <param name="frames">the number of frames</param>
    ''' <param name="fps">the framerate multiplied by 100 to avoid floating numbers like 59.94.</param> 
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getFramesToMS(ByVal frames As Long, ByVal fps As Integer) As Long
        If fps = 0 Then fps = -100
        Return (frames * 1000) / (fps / 100)
    End Function

    Public Function getFPS(ByVal channel As Integer) As Integer
        If containsChannel(channel) Then
            Return channelFPS(channel - 1)
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Returns the framerate of the spezified channel or 0 if the channel does not exist.
    ''' </summary>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getChannelFPS(ByVal channel As Integer) As Integer
        If isConnected() Then
            If containsChannel(channel) Then
                Dim info = New InfoCommand(channel)
                Dim infoDoc As New MSXML2.DOMDocument
                If infoDoc.loadXML(info.execute(testConnection).getXMLData()) Then
                    If infoDoc.hasChildNodes Then
                        If Not IsNothing(infoDoc.selectSingleNode("channel")) AndAlso Not IsNothing(infoDoc.selectSingleNode("channel").selectSingleNode("video-mode")) Then
                            Select Case infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue
                                Case "PAL"
                                    Return 2500
                                Case "NTSC"
                                    Return 2994
                                Case Else
                                    If infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Contains("i") Then
                                        Return Integer.Parse(infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Substring(infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.IndexOf("i") + 1)) / 2
                                    ElseIf infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Contains("p") Then
                                        Return Integer.Parse(infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Substring(infoDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.IndexOf("p") + 1))
                                    End If
                            End Select
                        End If
                    End If
                Else
                    logger.err("ServerController.getChannelFPS: Could not get channel fps. Error in server response: " & infoDoc.parseError.reason & " @" & vbNewLine & info.getResponse.getServerMessage)
                End If
            Else
                logger.err("ServerController.getChannelFPS: Could not get channel fps for channel " & channel & ". Channel does not exist.")
            End If
        End If
        Return -1
    End Function

    ' ''' <summary>
    ' ''' Returns a Dictionary of all media and templates on the server key by their names.
    ' ''' If withMediaInfo is true, all mediaItems will have filled mediaInfo which is default but need more time.
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function getMediaList() As Dictionary(Of String, AbstractCasparCGMedia)
    '    Dim media As New Dictionary(Of String, AbstractCasparCGMedia)
    '    Dim cmd As AbstractCommand
    '    '' Catch the media list and create the media objects
    '    If isConnected() Then
    '        Dim newMedia As AbstractCasparCGMedia = Nothing
    '        cmd = New ClsCommand()
    '        If cmd.execute(testConnection).isOK Then
    '            For Each line As String In cmd.getResponse.getData.Split(vbCrLf)
    '                line = line.Trim()
    '                If line <> "" AndAlso line.Split(" ").Length > 2 Then
    '                    ' removed .toUpper for the name as the server is allways using uppercases and on some special chars like µ this leads to an error.
    '                    Dim name = line.Substring(1, line.LastIndexOf("""") - 1)
    '                    line = line.Remove(0, line.LastIndexOf("""") + 1)
    '                    line = line.Trim().Replace("""", "").Replace("  ", " ")
    '                    Dim values() = line.Split(" ")
    '                    Select Case values(0)
    '                        Case "MOVIE"
    '                            newMedia = New CasparCGMovie(name)
    '                            'newMedia.setInfo("duration", getTimeStringOfMS(getOriginalMediaDuration(newMedia)))
    '                            ' get Thumbnail
    '                            cmd = New ThumbnailRetrieveCommand(name)
    '                            If cmd.isCompatible(testConnection) AndAlso cmd.execute(testConnection).isOK Then
    '                                newMedia.setBase64Thumb(cmd.getResponse.getData)
    '                            End If
    '                            media.Add(newMedia.getUuid, newMedia)
    '                        Case "AUDIO"
    '                            newMedia = New CasparCGAudio(name)
    '                            media.Add(newMedia.getUuid, newMedia)
    '                        Case "STILL"
    '                            newMedia = New CasparCGStill(name)
    '                            ' get Thumbnail
    '                            cmd = New ThumbnailRetrieveCommand(name)
    '                            If cmd.isCompatible(testConnection) AndAlso cmd.execute(testConnection).isOK Then
    '                                newMedia.setBase64Thumb(cmd.getResponse.getData)
    '                            End If
    '                            media.Add(newMedia.getUuid, newMedia)
    '                    End Select
    '                    If Not IsNothing(newMedia) Then newMedia.fillMediaInfo(getTestConnection, testChannel)
    '                End If
    '            Next
    '        End If

    '        '' Catch the template list and create the template objects
    '        cmd = New TlsCommand()
    '        If cmd.execute(testConnection).isOK Then
    '            For Each line As String In cmd.getResponse.getData.Split(vbCrLf)
    '                line = line.Trim.Replace(vbCr, "").Replace(vbLf, "")
    '                If line <> "" AndAlso line.Split(" ").Length > 2 Then
    '                    Dim name = line.Substring(1, line.LastIndexOf("""") - 1).ToUpper
    '                    newMedia = New CasparCGTemplate(name)
    '                    newMedia.fillMediaInfo(testConnection, testChannel)
    '                    media.Add(newMedia.getUuid, newMedia)
    '                End If
    '            Next
    '        End If
    '    End If
    '    Return media
    'End Function

    Public Shared Function getBase64ToImage(ByVal base64string As String) As System.Drawing.Image
        'Converts the base64 encoded msg to image data
        Return System.Drawing.Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(base64string)))
    End Function

    Public Function getTicker() As frameTicker
        Return ticker
    End Function

    Public Sub stopTicker()
        If Not IsNothing(ticker) Then
            ticker.stopTicker()
        End If
    End Sub

    Public Sub startTicker()
        If Not IsNothing(ticker) Then
            ticker.startTicker()
        End If
    End Sub

    Public Sub toggleTickerActive()
        ticker.toggleTicker()
    End Sub

End Class


Public Class frameTicker
    Private sysTimer As Timers.Timer
    Private frametime As Integer
    Private sc As ServerController

    Public Sub New(ByVal controller As ServerController, Optional ByVal frameInterval As Integer = 1)
        sc = controller

        For i As Integer = 1 To sc.getChannels
            If frametime > 1000 / (sc.getFPS(i) / 100) OrElse frametime < 1 Then frametime = 1000 / (sc.getFPS(i) / 100)
        Next

        If frameInterval > 1 Then
            sysTimer = New Timers.Timer(frametime * frameInterval)
        Else
            sysTimer = New Timers.Timer(frametime)
        End If
        sysTimer.Enabled = False
        AddHandler sysTimer.Elapsed, Sub() RaiseEvent frameTick()
    End Sub

    Public Event frameTick()

    Public Sub startTicker()
        sysTimer.Enabled = True
    End Sub

    Public Sub stopTicker()
        sysTimer.Enabled = False
    End Sub

    Public Sub toggleTicker()
        sysTimer.Enabled = Not sysTimer.Enabled
    End Sub

End Class
