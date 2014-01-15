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

Public Class PlaylistBlockItem
    Inherits AbstractPlaylistItem
    Implements IPlaylistItem

    Private playedItems As List(Of IPlaylistItem)

    Private timer As System.Timers.Timer
    Private stopWatch As New Stopwatch()

    Private updateItems As New Threading.Semaphore(1, 1)


    Public Sub New(ByVal name As String, ByRef controller As ServerController, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1)
        MyBase.New(name, PlaylistItemTypes.BLOCK, controller, channel, layer, 0)
        playedItems = New List(Of IPlaylistItem)
        timer = New Timers.Timer()
        timer.Enabled = False
        AddHandler timer.Elapsed, Sub() playNextItem()
    End Sub

    Public Overrides Function isPlaying() As Boolean
        If isWaiting() AndAlso Not timer.Enabled Then Return True
        For Each child In getChildItems()
            If child.isPlaying OrElse child.isWaiting Then Return True
        Next
        Return False
    End Function

    Public Overrides Sub halt()
        timer.Enabled = False
        stopWatch.Stop()
        waiting = False
        For Each item In getChildItems()
            If item.isPlaying Then RemoveHandler item.stopped, AddressOf itemStopped
            item.halt()
        Next
        stoppedPlaying()
    End Sub

    Public Overrides Sub abort()
        timer.Enabled = False
        stopWatch.Stop()
        waiting = False
        playing = False
        _paused = False
        setPosition(0)
        playedItems.Clear()
        For Each item In getChildItems()
            item.abort()
        Next
        raiseAborted(Me)
    End Sub

    Public Overrides Sub stoppedPlaying()
        playing = False
        _paused = False
        playedItems.Clear()
        raiseStopped(Me)
        logger.debug("PlaylistItem.stoppedPlaying: Stopped playing " & getName() & " (" & getChannel() & "-" & getLayer() & ")")
    End Sub


    ''' <summary>
    ''' Start this item and all childItems. If noWait is given and false, start will wait till all items has been stopped and then end,
    ''' else it will start each item and then end imediatly
    ''' </summary>
    Public Overrides Sub start()
        logger.debug("PlaylistItem.start: Start " & getName())
        playedItems.Clear()
        playing = True
        raiseStarted(Me)

        ' alle Unteritems starten.
        ' Wenn parallel, dann wird nicht gewartet und alle starten
        ' semi gleichzeitig
        ' Sost startet das nächste erst wenn das vorherige fertig ist.
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

    Public Overrides Sub playNextItem(Optional ByRef lastPlayed As IPlaylistItem = Nothing)

        ' Check if we need to start delayed
        If getDelay() > 0 AndAlso timer.Enabled = False AndAlso IsNothing(lastPlayed) Then
            stopWatch.Reset()
            timer.Interval = getDelay()
            timer.Enabled = True
            stopWatch.Start()
            waiting = True
            raiseWaitForNext(Me)
        Else
            stopWatch.Stop()
            timer.Enabled = False
            stopWatch.Reset()
            waiting = False
            playing = True

            If isParallel() Then
                'Stat all subitems
                For Each item In getChildItems()
                    Application.DoEvents()
                    AddHandler item.stopped, AddressOf itemStopped
                    item.start()
                Next
            Else
                ' only start the next subitem
                Dim nextItem As IPlaylistItem = getNextToPlay(lastPlayed)
                If Not IsNothing(nextItem) AndAlso Not nextItem.isPlaying Then
                    AddHandler nextItem.stopped, AddressOf itemStopped
                    AddHandler nextItem.canceled, AddressOf itemCanceled
                    AddHandler nextItem.started, AddressOf itemStarted
                    nextItem.start()
                ElseIf isLooping() Then
                    start()
                Else : stoppedPlaying()
                End If
            End If
        End If
    End Sub

    Private Sub itemStopped(ByRef sender As IPlaylistItem)
        '' Check what kind of item stopped. If we're parallel and all items has stopped,
        '' but looping is active, start again.
        '' If seq. let playNext decide what to do.
        RemoveHandler sender.stopped, AddressOf itemStopped
        playedItems.Add(sender)
        If isParallel() Then
            If isLooping() AndAlso Not isPlaying() Then
                start()
            ElseIf Not isPlaying() Then
                stoppedPlaying()
            End If
        Else
            playNextItem(sender)
        End If
    End Sub

    Private Sub itemStarted(ByRef sender As IPlaylistItem)
        ' check if we can safely load the next item
        Dim itemToLoad As IPlaylistItem = getNextToPlay(sender)
        If Not IsNothing(itemToLoad) AndAlso itemToLoad.isPlayable AndAlso sender.isPlayable AndAlso sender.getChannel = itemToLoad.getChannel AndAlso sender.getLayer = itemToLoad.getLayer Then
            itemToLoad.load()
        End If
        RemoveHandler sender.started, AddressOf itemStarted
    End Sub

    Private Sub itemCanceled(ByRef sender As IPlaylistItem)
        ' if something unexpected happens, we schedule the next
        RemoveHandler sender.canceled, AddressOf itemCanceled
        playNextItem(sender)
    End Sub

    Private Function getNextToPlay(ByRef lastPlayed As IPlaylistItem) As IPlaylistItem
        If Not IsNothing(lastPlayed) AndAlso getChildItems.Contains(lastPlayed) Then
            ' calculate the next item
            '' LOCK the items list while doing this!!
            updateItems.WaitOne()
            Dim childs = getChildItems()
            Dim pos As Integer = childs.IndexOf(lastPlayed)
            If pos = childs.Count - 1 Then
                ' we played the last item so return nothing 
                updateItems.Release()
                Return Nothing
            Else
                ' not the end of the list, so return next
                updateItems.Release()
                Return childs.Item(pos + 1)
            End If
        ElseIf getChildItems.Count > 0 Then
            ' No current item, just play the first
            Return getChildItems.First
        Else
            ' no Item to play
            Return Nothing
        End If
    End Function

    Public Overrides Function getPosition() As Long

        If timer.Enabled OrElse (isPaused() AndAlso isWaiting() AndAlso getDelay() > 0) Then
            Return stopWatch.ElapsedMilliseconds - getDelay()
        ElseIf IsNothing(getParent) OrElse getParent.isPlaying() OrElse isPlaying() Then
            Dim pos As Long
            For Each child In getChildItems()
                If isParallel() Then
                    If playedItems.Contains(child) Then
                        pos = Math.Max(pos, child.getDuration)
                    Else
                        pos = Math.Max(pos, child.getPosition)
                    End If
                Else
                    If playedItems.Contains(child) Then
                        pos = pos + child.getDuration
                    Else
                        pos = pos + Math.Max(0, child.getPosition)
                    End If
                End If
            Next
            Return pos
        Else
            Return 0
        End If
    End Function

    Public Overrides Function getDuration() As Long
        Dim duration As Long
        For Each child In getChildItems()
            If isParallel() Then
                duration = Math.Max(duration, child.getDuration)
            Else
                duration = duration + child.getDuration
            End If
        Next
        Return duration
    End Function

    Public Overrides Sub load()
        If isParallel() Then
            'load all subitems
            For Each item In getChildItems()
                Application.DoEvents()
                item.load()
            Next
        Else
            getNextToPlay(Nothing).load()
        End If
    End Sub

    Public Overrides Sub show()
        If isParallel() Then
            'load all subitems
            For Each item In getChildItems()
                Application.DoEvents()
                item.show()
            Next
        Else
            getNextToPlay(Nothing).show()
        End If
    End Sub

    Public Overrides Sub pause()
        If Not isPaused() Then
            If isWaiting() AndAlso getDelay() > 0 Then
                timer.Enabled = False
                stopWatch.Stop()
            ElseIf isPlaying() Then
                If isParallel() Then
                    For Each item In getChildItems()
                        Application.DoEvents()
                        item.pause()
                    Next
                Else
                    Dim playingItem As IPlaylistItem = Nothing
                    If playedItems.Count > 0 Then playingItem = getNextToPlay(playedItems.Last)
                    If Not IsNothing(playingItem) Then
                        playingItem.pause()
                    End If
                End If
            End If
            _paused = True
            raisePaused(Me)
        End If
    End Sub

    Public Overrides Sub unPause()
        If isPaused() Then
            If isWaiting() AndAlso getDelay() > 0 Then
                timer.Interval = (getDelay() - stopWatch.ElapsedMilliseconds)
                logger.log(getDelay() - stopWatch.ElapsedMilliseconds & " = " & timer.Interval)
                timer.Enabled = True
                stopWatch.Start()
            ElseIf isPlaying() Then
                If isParallel() Then
                    For Each item In getChildItems()
                        Application.DoEvents()
                        item.unPause()
                    Next
                Else
                    Dim playingItem As IPlaylistItem = Nothing
                    If playedItems.Count > 0 Then playingItem = getNextToPlay(playedItems.Last)
                    If Not IsNothing(playingItem) AndAlso playingItem.isPaused Then
                        playingItem.unPause()
                    End If
                End If
            End If
            _paused = False
            raiseUnpaused(Me)
        End If
    End Sub

    Public Overrides Sub setDuration(duration As Long)
    End Sub
End Class
