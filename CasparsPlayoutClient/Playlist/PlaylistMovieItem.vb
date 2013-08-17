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

Public Class PlaylistMovieItem
    Inherits AbstractPlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGMovie
    Private timer As System.Timers.Timer
    Private stopWatch As New Stopwatch()
    Private loaded As Boolean = False

    ''' <summary>
    ''' Create a PlaylistMovieItem. If a duration is given and smaller than the original duration of the file, the file will only be played for that long.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="controller"></param>
    ''' <param name="movie"></param>
    ''' <param name="duration"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByVal movie As CasparCGMovie, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.MOVIE, controller, channel, layer, duration)
        If Not IsNothing(movie) Then
            If movie.getInfos.Count = 0 Then
                movie.fillMediaInfo(getController.getTestConnection, getController.getTestChannel)
            End If
            If movie.containsInfo("nb-frames") AndAlso (duration > getController.getOriginalMediaDuration(movie) OrElse duration = -1) Then
                setDuration(getController.getMediaDuration(movie, channel))
            End If
            media = movie
            timer = New Timers.Timer()
            timer.Enabled = False
            AddHandler timer.Elapsed, Sub() playNextItem()
        Else
            logger.critical("PlaylistMovieItem.new: ERROR: Given movie was nothing - Stopping now")
            Throw New Exception("NOTHING not allowed")
        End If
    End Sub


    '' Methoden die überschrieben werden müssen weil sie andere oder mehr functionen haben
    ''-------------------------------------------------------------------------------------
    Public Overrides Sub start()
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
        '' CMD an ServerController schicken
        'logger.log("PlaylistMovieItem.playNextItem: Starte " & getChannel() & "-" & getLayer() & ": " & getMedia.getName)
        If getDelay() > 0 AndAlso timer.Enabled = False Then
            timer.Interval = getDelay()
            timer.Enabled = True
            stopWatch.Restart()
            waiting = True
            raiseWaitForNext(Me)
        Else
            timer.Enabled = False
            stopWatch.Reset()
            waiting = False
            playing = True

            If getController.containsChannel(getChannel) AndAlso getLayer() > -1 Then
                ' if the clip is allready loaded to bg, just start, else load & start
                Dim cmd As ICommand
                If isLoaded() Then
                    cmd = New PlayCommand(getChannel, getLayer)
                    loaded = False
                    logger.log("PlaylistMovieItem.playNextItem: Start allready loaded clip " & getMedia.getName)
                Else
                    Dim d As Long = getMedia.getInfo("nb-frames")
                    If getDuration() < getController.getMediaDuration(getMedia, getChannel) Then d = ServerController.getMsToFrames(getDuration, getFPS)
                    getMedia.setInfo("duration", d)
                    cmd = New PlayCommand(getChannel, getLayer, getMedia, isLooping, , d)
                    logger.log("PlaylistMovieItem.playNextItem: Load and start clip " & getMedia.getName)
                End If
                If cmd.execute(getController.getCommandConnection).isOK Then
                    raiseStarted(Me)
                    ' InfoMediaUpdater needs an empty to detect end of file due to BUG: frame-number never reaches nb-frames
                    If Not getController.getCommandConnection.isOSCSupported() Or ClearAfterPlayback() Then
                        cmd = New LoadbgCommand(getChannel, getLayer, "empty", True)
                        cmd.execute(getController.getCommandConnection)
                    End If
                    logger.log("PlaylistMovieItem.playNextItem: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " started.")
                Else
                    playing = False
                    raiseCanceled(Me)
                    logger.err("PlaylistMovieItem..playNextItem: Could not start " & media.getFullName & ". ServerMessage was: " & cmd.getResponse.getServerMessage)
                End If
            Else
                logger.err("PlaylistMovieItem..playNextItem: Error playing " & getName() & ". The channel " & getChannel() & " does not exist on the server. Aborting start.")
            End If
        End If
    End Sub

    Public Overrides Sub abort()
        '' CMD an ServerController schicken
        Dim cmd As New StopCommand(getChannel, getLayer)
        cmd.execute(getController.getCommandConnection)
        setPosition(0)
        waiting = False
        playing = False
        loaded = False
        timer.Enabled = False
        stopWatch.Stop()
        raiseAborted(Me)
        logger.log("PlaylistMovieItem.abort: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " aborted.")
    End Sub

    Public Overrides Sub halt()
        If isPlaying() Then
            Dim cmd As New StopCommand(getChannel, getLayer)
            cmd.execute(getController.getCommandConnection)
        End If
        waiting = False
        stoppedPlaying()
    End Sub


    Public Overrides Sub stoppedPlaying()
        timer.Enabled = False
        stopWatch.Stop()
        playing = False
        setPosition(0)
        raiseStopped(Me)
        logger.log("PlaylistMovieItem.stoppedPlaying: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " stopped playing.")
    End Sub

    Public Overrides Sub pause(ByVal frames As Long)
        '' cmd an ServerController schicken
        Dim cmd As New PauseCommand(getChannel, getLayer)
        cmd.execute(getController.getCommandConnection)
        raisePaused(Me)
        logger.log("PlaylistMovieItem.pause: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " paused.")
    End Sub

    Public Overrides Sub unPause()
        '' cms an ServerController schicken

        ' check if a media is loaded to bg, than we need to play me with seek!!
        ' because otherwise it would start the bg clip
        Dim cmd As New PlayCommand(getChannel(), getLayer())
        cmd.execute(getController.getCommandConnection)
        raiseStarted(Me)
        logger.log("PlaylistMovieItem.unPause: " & getChannel() & "-" & getLayer() & ": " & getMedia.getName & " restarted.")
    End Sub

    Public Overrides Sub load()
        ''ToDo
        If getController.getCommandConnection.isLayerFree(getLayer, getChannel, False, True) Then

            Dim d As Long = getMedia.getInfo("nb-frames")
            If getDuration() < getController.getMediaDuration(getMedia, getChannel) Then d = ServerController.getMsToFrames(getDuration, getFPS)
            getMedia.setInfo("duration", d)

            Dim cmd As New LoadbgCommand(getChannel, getLayer, getMedia, False, isLooping, , d)
            If cmd.execute(getController.getCommandConnection).isOK Then
                loaded = True
                logger.log("PlaylistMovieItem.load: Loaded " & getMedia.getName + " to bg " & getChannel() & "-" & getLayer())
            Else
                loaded = False
                logger.warn("PlaylistMovieItem.load: Could not load " & getMedia.getName + " to bg " & getChannel() & "-" & getLayer() & ". Server resonded " & vbNewLine & cmd.getResponse.getServerMessage)
            End If
        Else
            logger.warn("PlaylistMovieItem.load: Can't load background if layer isn't free. Did not load " & getMedia.getName + " to " & getChannel() & "-" & getLayer())
        End If
    End Sub

    Public Overrides Function isLoaded() As Boolean
        Return loaded
    End Function

    Public Overrides Function getMedia() As CasparCGMedia
        Return media
    End Function

    Public Overrides Function getPosition() As Long
        If getMedia.containsInfo("frame-number") AndAlso isPlaying() Then 'OrElse hasPlayingParent()) Then
            Return ServerController.getFramesToMS(Long.Parse(getMedia.getInfo("frame-number")), getFPS())
        ElseIf timer.Enabled Then
            Return stopWatch.ElapsedMilliseconds - timer.Interval
        Else
            Return 0
        End If
    End Function

    Public Overrides Sub setPosition(ByVal position As Long)
        'If Not isPlaying() Then
        getMedia.setInfo("frame-number", ServerController.getMsToFrames(position, getFPS))
        raiseChanged(Me)
        'End If
    End Sub

    Public Overrides Sub setDuration(ByVal duration As Long)
        MyBase.setDuration(duration)
        ' if we're allready loaded to bg, we need to update us
        If isLoaded() Then

            Dim d As Long = getMedia.getInfo("nb-frames")
            If getDuration() < getController.getMediaDuration(getMedia, getChannel) Then d = ServerController.getMsToFrames(getDuration, getFPS)
            getMedia.setInfo("duration", d)

            Dim cmd As New CallCommand(getChannel, getLayer, isLooping, , d)
            cmd.execute(getController.getCommandConnection)
        End If
    End Sub

    Public Overrides Sub setChannel(ByVal channel As Integer)
        MyBase.setChannel(channel)
        If getController.containsChannel(channel) AndAlso Not IsNothing(getMedia) Then
            setDuration(getController.getMediaDuration(getMedia, channel))
        End If
    End Sub


    '' Methoden die Überschrieben werden müssen weil sie leer sind
    ''-------------------------------------------------------------
    Public Overloads Sub addItem(ByVal item As IPlaylistItem)
        '' Nothing to do. Movie Items dosen't have child items
    End Sub

    Public Overloads Sub setChildItems(ByRef items As List(Of IPlaylistItem))
        '' Nothing to do. Movie Items dosen't have child items
    End Sub
End Class
