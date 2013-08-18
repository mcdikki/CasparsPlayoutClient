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

Public Class PlaylistStillItem
    Inherits AbstractPlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGStill
    Private timer As System.Timers.Timer
    Private stopWatch As New Stopwatch()
    Private loaded As Boolean = False

    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByVal still As CasparCGStill, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.STILL, controller, channel, layer, duration)
        setAutoStart(True)
        setClearAfterPlayback()
        media = still
        timer = New Timers.Timer()
    End Sub

    Public Overrides Function getMedia() As AbstractCasparCGMedia
        Return media
    End Function


    Public Overrides Sub start()
        ' Wait if autostart not checked
        If getChannel() > 0 AndAlso getLayer() > -1 Then
            ' Wait if autostart not checked
            If Not isAutoStarting() Then
                raiseWaitForNext(Me)
                waiting = True
            Else
                playNextItem()
            End If
        Else
            raiseCanceled(Me)
        End If
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
            waiting = False

            If getController.containsChannel(getChannel) AndAlso getLayer() > -1 Then
                ' if the clip is allready loaded to bg, just start, else load & start
                Dim cmd As ICommand
                If isLoaded() Then
                    cmd = New PlayCommand(getChannel, getLayer)
                    loaded = False
                    logger.log("PlaylistStillItem.playNextItem: Start allready loaded still " & getMedia.getName)
                Else
                    cmd = New PlayCommand(getChannel, getLayer, getMedia)
                    logger.log("PlaylistStillItem.playNextItem: Load and start still " & getMedia.getName)
                End If
                If cmd.execute(getController.getCommandConnection).isOK Then
                    playing = True
                    raiseStarted(Me)
                    stopWatch.Restart()
                    logger.log("PlaylistStillItem.playNextItem: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " started.")

                    ' if a duration is given, we need to start a timer as ccg doesn't know a image length at all
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
                    playing = False
                    raiseCanceled(Me)
                    logger.err("PlaylistStillItem..playNextItem: Could not start " & media.getFullName & ". ServerMessage was: " & cmd.getResponse.getServerMessage)
                End If
            Else
                logger.err("PlaylistStillItem..playNextItem: Error playing " & getName() & ". The channel " & getChannel() & " does not exist on the server. Aborting start.")
            End If
        End If
    End Sub

    Public Overrides Sub stoppedPlaying()
        timer.Enabled = False
        timer.Dispose()
        timer = New Timers.Timer()
        stopWatch.Reset()
        playing = False
        raiseStopped(Me)
        logger.log("PlaylistStillItem.stoppedPlaying: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " stopped playing.")
    End Sub

    Public Overrides Sub abort()
        playing = False
        waiting = False
        timer.Enabled = False
        timer.Dispose()
        timer = New Timers.Timer()
        stopWatch.Reset()
        Dim cmd As New StopCommand(getChannel, getLayer)
        cmd.execute(getController.getCommandConnection)
        setPosition(0)
    End Sub

    Public Overrides Sub halt()
        If isPlaying() Then
            Dim cmd As New StopCommand(getChannel, getLayer)
            cmd.execute(getController.getCommandConnection)
        End If
        waiting = False
        stoppedPlaying()
    End Sub

    Public Overrides Sub load()
        If getController.getCommandConnection.isLayerFree(getLayer, getChannel, False, True) Then
            Dim cmd As New LoadbgCommand(getChannel, getLayer, getMedia)
            If cmd.execute(getController.getCommandConnection).isOK Then
                loaded = True
                logger.log("PlaylistStillItem.load: Loaded " & getMedia.getName + " to bg " & getChannel() & "-" & getLayer())
            Else
                loaded = False
                logger.warn("PlaylistStillItem.load: Could not load " & getMedia.getName + " to bg " & getChannel() & "-" & getLayer() & ". Server resonded " & vbNewLine & cmd.getResponse.getServerMessage)
            End If
        Else
            logger.warn("PlaylistStillItem.load: Can't load background if layer isn't free. Did not load " & getMedia.getName + " to " & getChannel() & "-" & getLayer())
        End If
    End Sub

    Public Overrides Function getPosition() As Long
        If isPlaying() OrElse (hasPlayingParent() AndAlso Not isWaiting()) Then
            If timer.Enabled And getDuration() > 0 Then
                ' Normal playing
                'raiseChanged(Me)
                Return stopWatch.ElapsedMilliseconds
            Else
                'raiseChanged(Me)
                Return 0 - stopWatch.ElapsedMilliseconds
            End If
        ElseIf isWaiting() AndAlso timer.Enabled Then
            ' Delay countdown
            Return (0 - timer.Interval) + stopWatch.ElapsedMilliseconds
        Else : Return 0
        End If
    End Function

    'Public Overrides Function getDuration() As Long
    '    If isPlaying() AndAlso MyBase.getDuration = 0 Then
    '        Return 0 - getPosition()
    '    Else
    '        Return MyBase.getDuration()
    '    End If
    'End Function

    Public Overrides Function isLoaded() As Boolean
        Return loaded
    End Function

    Public Overrides Sub unPause()
    End Sub

    Public Overrides Sub pause(frames As Long)
    End Sub

End Class
