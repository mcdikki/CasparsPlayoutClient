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

Public Class PlaylistTemplateItem
    Inherits AbstractPlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGTemplate
    Private flashlayer As Integer = 10

    Private timer As System.Timers.Timer
    Private stopWatch As New Stopwatch()

    Private loaded As Boolean = False
    Private showing As Boolean = False

    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByVal template As CasparCGTemplate, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal flashlayer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.TEMPLATE, controller, channel, layer, duration)

        media = template
        timer = New Timers.Timer()
        timer.Enabled = False
        AddHandler timer.Elapsed, Sub() playNextItem()
    End Sub

    Public Overrides Sub halt()
        timer.Enabled = False
        stopWatch.Stop()
        waiting = False
        ' TODO: halt template
        Dim cmd As New CgStopCommand(getChannel, getLayer, flashlayer)
        cmd.execute(getController().getCommandConnection)
        stoppedPlaying()
    End Sub

    Public Overrides Sub stoppedPlaying()
        timer.Enabled = False
        timer.Dispose()
        timer = New Timers.Timer()
        stopWatch.Reset()
        playing = False
        showing = False
        _paused = False
        raiseStopped(Me)
        logger.log("PlaylistTemplateItem.stoppedPlaying: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " stopped playing.")
    End Sub

    Public Overrides Sub abort()
        playing = False
        showing = False
        _paused = False
        waiting = False
        timer.Enabled = False
        timer.Dispose()
        timer = New Timers.Timer()
        stopWatch.Reset()
        Dim cmd As New CgRemoveCommand(getChannel, getLayer, flashlayer)
        cmd.execute(getController.getCommandConnection)
        setPosition(0)
    End Sub

    Public Overrides Sub playNextItem(Optional ByRef lastPlayed As IPlaylistItem = Nothing)
        If getDelay() > 0 AndAlso timer.Enabled = False Then
            timer = New Timers.Timer(getDelay)
            AddHandler timer.Elapsed, Sub() playNextItem()
            timer.Enabled = True
            stopWatch.Restart()
            waiting = True
            raiseWaitForNext(Me)
        Else
            timer.Enabled = False
            timer.Dispose()
            timer = New Timers.Timer
            stopWatch.Reset()

            If getController.containsChannel(getChannel) AndAlso getLayer() > -1 Then
                ' if the template is allready loaded, just start, else load & start
                Dim cmd As AbstractCommand
                If isLoaded() Or isShowing() Then
                    cmd = New CgPlayCommand(getChannel, getLayer, flashlayer)
                    logger.log("PlaylistTemplateItem.playNextItem: Start allready loaded template " & getMedia.getName)
                Else
                    cmd = New CgAddCommand(getChannel, getLayer, getMedia, flashlayer, True, media.getDataString)
                    logger.log("PlaylistTemplateItem.playNextItem: Load and start Template " & getMedia.getName)
                End If
                If isShowing() OrElse cmd.execute(getController.getCommandConnection).isOK Then
                    loaded = False
                    showing = False
                    playing = True
                    waiting = False
                    raiseStarted(Me)
                    stopWatch.Restart()
                    logger.log("PlaylistTemplateItem.playNextItem: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " started.")

                    ' if a duration is given, we need to start a timer as ccg doesn't know a template length at all
                    If getDuration() > 0 Then
                        timer = New Timers.Timer(getDuration())
                        If ClearAfterPlayback() Then
                            AddHandler timer.Elapsed, AddressOf halt
                        Else
                            AddHandler timer.Elapsed, AddressOf stoppedPlaying
                        End If
                        timer.Start()
                    End If
                Else
                    loaded = False
                    showing = False
                    playing = False
                    waiting = False
                    raiseCanceled(Me)
                    logger.err("PlaylistTemplateItem.playNextItem: Could not start " & media.getFullName & ". ServerMessage was: " & cmd.getResponse.getServerMessage)
                End If
            Else
                waiting = False
                logger.err("PlaylistTemplateItem.playNextItem: Error playing " & getName() & ". The channel " & getChannel() & " does not exist on the server. Aborting start.")
            End If
        End If
    End Sub

    Public Overrides Sub start()
        logger.debug("PlaylistItem.start: Start " & getName())
        playing = True
        raiseStarted(Me)

        If isAutoLoading() Then
            show()
        Else
            load()
        End If
        If Not isAutoStarting() Then
            raiseWaitForNext(Me)
            waiting = True
        Else
            playNextItem()
        End If
        logger.debug("PlaylistItem.start: Start " & getName() & " has been completed")
    End Sub

    ''' <summary>
    ''' Triggers a next to the template
    ''' </summary>
    Public Sub cgNext()
        If isLoaded() Or isShowing() Or isPlaying() Then
            Dim cmd As New CgNextCommand(getChannel, getLayer, flashlayer)
            If Not cmd.execute(getController.getCommandConnection).isOK Then
                logger.err("PlaylistTemplateItem.cgNext: Error executing next command for template " & getMedia.getName & "(" & getChannel() & "-" & getLayer() & ":" & flashlayer & ")" & vbNewLine & cmd.getResponse.getServerMessage)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Triggers a invoke to the template
    ''' </summary>
    Public Sub cgInvoke(invokeString As String)
        If isLoaded() Or isShowing() Or isPlaying() Then
            Dim cmd As New CgInvokeCommand(getChannel, getLayer, flashlayer, invokeString)
            If Not cmd.execute(getController.getCommandConnection).isOK Then
                logger.err("PlaylistTemplateItem.cgInvoke: Error executing invoke command for template " & getMedia.getName & "(" & getChannel() & "-" & getLayer() & ":" & flashlayer & ")" & vbNewLine & cmd.getResponse.getServerMessage)
            End If
        End If
    End Sub

    Public Overrides Sub load()
        '' ToDo: Check if layer is free OR a flash producer is running
        If getController.getCommandConnection.isLayerFree(getLayer, getChannel, True, False) And Not isLoaded() And Not isShowing() Then
            Dim cmd As New CgAddCommand(getChannel, getLayer, media, flashlayer, False, media.getDataString)
            If cmd.execute(getController.getCommandConnection).isOK Then
                loaded = True
                logger.log("PlaylistTemplateItem.load: Loaded " & getMedia.getName + " to " & getChannel() & "-" & getLayer() & ":" & flashlayer)
            Else
                loaded = False
                logger.warn("PlaylistTemplateItem.load: Could not load " & getMedia.getName + " to " & getChannel() & "-" & getLayer() & ":" & flashlayer & ". Server resonded " & vbNewLine & cmd.getResponse.getServerMessage)
            End If
        ElseIf isLoaded() Or isShowing() Then
            logger.log("PlaylistTemplatetem.load: Won't load allready loaded " & getMedia.getName + " to " & getChannel() & "-" & getLayer() & ":" & flashlayer)
        Else
            logger.warn("PlaylistStilItem.load: Can't load background if layer isn't free. Did not load " & getMedia.getName + " to " & getChannel() & "-" & getLayer())
        End If
    End Sub

    Public Overrides Sub show()
        If isAutoLoading() AndAlso Not isShowing() Then
            Dim cmd As New CgAddCommand(getChannel, getLayer, media, flashlayer, False, media.getDataString)
            If cmd.execute(getController.getCommandConnection).isOK Then
                showing = True
                loaded = False
                logger.log("PlaylistTemplateItem.load: Loaded " & getMedia.getName + " to " & getChannel() & "-" & getLayer() & ":" & flashlayer)
            Else
                showing = False
                logger.warn("PlaylistTemplateItem.load: Could not load " & getMedia.getName + " to " & getChannel() & "-" & getLayer() & ":" & flashlayer & ". Server resonded " & vbNewLine & cmd.getResponse.getServerMessage)
            End If
        Else
            logger.log("PlaylistTemplatetem.load: Won't load allready loaded " & getMedia.getName + " to " & getChannel() & "-" & getLayer() & ":" & flashlayer)
        End If
    End Sub

    Public Overrides Function getPosition() As Long
        If isPlaying() OrElse (hasPlayingParent() AndAlso Not isWaiting()) Then
            If getDuration() > 0 Then
                ' Normal playing
                Return stopWatch.ElapsedMilliseconds
            Else
                'raiseChanged(Me)
                Return 0 - stopWatch.ElapsedMilliseconds
            End If
        ElseIf isWaiting() AndAlso getDelay() > 0 Then
            ' Delay countdown
            Return (0 - getDelay()) + stopWatch.ElapsedMilliseconds
        Else : Return 0
        End If
    End Function

    Public Overrides Function isLoaded() As Boolean
        Return loaded
    End Function

    Public Overrides Function isShowing() As Boolean
        Return showing
    End Function

    Public Overrides Sub unPause()
        If isPaused() Then
            If isWaiting() AndAlso getDelay() > 0 Then
                timer.Interval = getDelay() - stopWatch.ElapsedMilliseconds
                timer.Start()
            ElseIf getDuration() > 0 Then
                timer.Interval = getDuration() - getPosition()
                timer.Start()
            End If
            stopWatch.Start()
            _paused = False
            raiseUnpaused(Me)
        End If
    End Sub

    Public Overrides Sub pause()
        If isPlaying() OrElse isWaiting() Then
            timer.Stop()
            stopWatch.Stop()
            _paused = True
            raisePaused(Me)
        End If
    End Sub

    Public Overrides Function getMedia() As AbstractCasparCGMedia
        Return media
    End Function
End Class
