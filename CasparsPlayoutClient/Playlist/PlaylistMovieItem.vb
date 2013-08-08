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
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGMovie
    Public Shadows Event waitForNext()

    ''' <summary>
    ''' Create a PlaylistMovieItem. If a duration is given and smaller than the original duration of the file, the file will only be played for that long.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="controller"></param>
    ''' <param name="movie"></param>
    ''' <param name="duration"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal name As String, ByRef controller As ServerControler, ByVal movie As CasparCGMovie, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.MOVIE, controller, channel, layer, duration)
        If Not IsNothing(movie) Then
            If movie.getInfos.Count = 0 Then
                movie.fillMediaInfo(getController.getTestConnection, getController.getTestChannel)
                'movie.parseXML(getController.getMediaInfo(movie))
            End If
            If movie.containsInfo("nb-frames") AndAlso (duration > getController.getOriginalMediaDuration(movie) OrElse duration = -1) Then
                'setDuration(getController.getOriginalMediaDuration(movie))
                setDuration(getController.getMediaDuration(movie, channel))
            End If
            media = movie
        Else
            logger.critical("PlaylistMovieItem.new: ERROR: Given movie was nothing - Stopping now")
            Throw New Exception("NOTHING not allowed")
        End If
    End Sub


    '' Methoden die überschrieben werden müssen weil sie andere oder mehr functionen haben
    ''-------------------------------------------------------------------------------------
    Public Overrides Sub start(Optional ByVal noWait As Boolean = False)
        ' Wait if autostart not checked
        If Not isAutoStarting() AndAlso Not playNext Then
            RaiseEvent waitForNext()

            waiting = True
            While Not playNext
                Threading.Thread.Sleep(1)
            End While
            playNext = False
            waiting = False
        End If

        '' CMD an ServerController schicken
        logger.log("PlaylistMovieItem.start: Starte " & getChannel() & "-" & getLayer() & ": " & getMedia.toString)

        If getController.containsChannel(getChannel) AndAlso getLayer() > -1 Then
            Dim cmd As ICommand = New PlayCommand(getChannel, getLayer, getMedia, isLooping, False)

            'Dim result = getController.getCommandConnection.sendCommand(CasparCGCommandFactory.getPlay(getChannel, getLayer, getMedia, isLooping, False))
            If cmd.execute(getController.getCommandConnection).isOK Then
                While Not getController.readyForUpdate.WaitOne()
                    logger.warn("PlaylistMovieItem.start: " & getName() & ": Could not get handle to update my status")
                End While
                playing = True
                getController.readyForUpdate.Release()
                ' InfoMediaUpdater needs an empty to detect end of file due to BUG: frame-number never reaches nb-frames
                If Not getController.getCommandConnection.isOSCSupported() Then
                    cmd = New LoadbgCommand(getChannel, getLayer, "empty", True)
                    cmd.execute(getController.getCommandConnection)
                End If
                logger.log("PlaylistMovieItem.start: ...gestartet " & getChannel() & "-" & getLayer() & ": " & getMedia.toString)
            Else
                logger.err("PlaylistMovieItem.start: Could not start " & media.getFullName & ". ServerMessage was: " & cmd.getResponse.getServerMessage)
            End If

            While isPlaying() AndAlso Not noWait
            End While
        Else
            logger.err("PlaylistMovieItem.start: Error playing " & getName() & ". The channel " & getChannel() & " does not exist on the server. Aborting start.")
        End If
    End Sub

    Public Overrides Sub abort()
        '' CMD an ServerController schicken
        stoppedPlaying()
        setPosition(0)
        '' Der rest wird von der Elternklasse erledigt
        MyBase.abort()
    End Sub

    Public Overrides Sub stoppedPlaying()
        Dim cmd As New StopCommand(getChannel, getLayer)
        cmd.execute(getController.getCommandConnection)
        MyBase.stoppedPlaying()
    End Sub

    Public Overrides Sub pause(ByVal frames As Long)
        '' cmd an ServerController schicken
        Dim cmd As New PauseCommand(getChannel, getLayer)
        cmd.execute(getController.getCommandConnection)
        '' pause wird über die Elternklasse realisiert
        MyBase.pause(frames)
    End Sub

    Public Overrides Sub unPause()
        '' cms an ServerController schicken
        Dim cmd As New PlayCommand(getChannel(), getLayer())
        cmd.execute(getController.getCommandConnection)
        MyBase.unPause()
    End Sub

    Public Overrides Function getMedia() As CasparCGMedia
        Return media
    End Function

    Public Overrides Function getPosition() As Long
        If getMedia.containsInfo("frame-number") AndAlso (isPlaying() OrElse hasPlayingParent()) Then
            Return ServerControler.getTimeInMS(Long.Parse(getMedia.getInfo("frame-number")), getFPS())
        Else
            Return 0
        End If
    End Function

    Public Overrides Sub setPosition(ByVal position As Long)
        If Not isPlaying() AndAlso getMedia.containsInfo("frame-number") AndAlso getMedia.containsInfo("nb-frames") AndAlso Integer.Parse(getMedia.getInfo("nb-frames")) <= position Then
            getMedia.setInfo("frame-number", position)
        ElseIf Not isPlaying() Then
            getMedia.setInfo("frame-number", "0")
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
