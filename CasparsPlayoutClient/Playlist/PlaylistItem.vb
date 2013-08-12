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

Public MustInherit Class PlaylistItem
    Implements IPlaylistItem

    Private name As String
    Private layer As Integer
    Private channel As Integer
    Private delay As Long
    Private looping As Boolean
    Private autoStart As Boolean
    Private parallel As Boolean
    Private fps As Integer
    Private parent As IPlaylistItem

    ' Die (Kinder)Items dieses Items
    Private items As List(Of IPlaylistItem)
    Private currentItem As IPlaylistItem
    Private WithEvents controller As ServerControler
    Private Duration As Long ' Gesamtlaufzeit in Frames
    Private Position As Long ' aktuelle Frame
    Private Remaining As Long ' noch zu spielende Frames
    Private ItemType As PlaylistItem.PlaylistItemTypes ' Typ des Item
    Friend playing As Boolean
    Private _paused As Boolean
    Private startThread As Threading.Thread
    Private pauseThread As Threading.Thread
    Friend playNext As Boolean = False
    Friend waiting As Boolean = False

    Private updateItems As New Threading.Semaphore(1, 1)

    Public Event waitForNext(ByRef sender As IPlaylistItem) Implements IPlaylistItem.waitForNext
    Public Event aborted(ByRef sender As IPlaylistItem) Implements IPlaylistItem.aborted
    Public Event canceled(ByRef sender As IPlaylistItem) Implements IPlaylistItem.canceled
    Public Event changed(ByRef sender As IPlaylistItem) Implements IPlaylistItem.changed
    Public Event paused(ByRef sender As IPlaylistItem) Implements IPlaylistItem.paused
    Public Event started(ByRef sender As IPlaylistItem) Implements IPlaylistItem.started
    Public Event stopped(ByRef sender As IPlaylistItem) Implements IPlaylistItem.stopped

    Enum PlaylistItemTypes
        BLOCK = -1
        STILL = 0
        MOVIE = 1
        AUDIO = 2
        TEMPLATE = 3
        COMMAND = 4
    End Enum

    ''' <summary>
    ''' Constructor for a generic PlaylistItem (Should be called by Subclasses)
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="itemType"></param>
    ''' <param name="controller"></param>
    ''' <param name="duration"></param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal name As String, ByVal itemType As PlaylistItemTypes, ByRef controller As ServerControler, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        Me.name = name
        Me.ItemType = itemType
        Me.controller = controller
        setChannel(channel)
        setLayer(layer)
        If duration > -1 Then
            setDuration(duration)
        End If
        items = New List(Of IPlaylistItem)
    End Sub

    Public Overridable Sub halt() Implements IPlaylistItem.halt
        playing = False
        For Each item In items
            If item.isPlaying Then RemoveHandler item.stopped, AddressOf itemStopped
            item.halt()
        Next
        stoppedPlaying()
    End Sub

    Public Overridable Sub abort() Implements IPlaylistItem.abort
        waiting = False
        'playNext = False
        playing = False
        setPosition(0)
        For Each item In items
            item.abort()
        Next
        RaiseEvent aborted(Me)
    End Sub

    Public Overridable Sub stoppedPlaying() Implements IPlaylistItem.stoppedPlaying
        playing = False
        'For Each item In items
        '    item.stoppedPlaying()
        'Next
        setPosition(0)
        RaiseEvent stopped(Me)
        logger.debug("PlaylistItem.stoppedPlaying: Stopped playing " & getName() & " (" & getChannel() & "-" & getLayer() & ")")
    End Sub

    Public Sub load() Implements IPlaylistItem.load
        '' Wird nur in den Medien Items implementiert
    End Sub

    '' ToDo:
    '' Alle Pause und delay funktionen über events realisieren und eventuell mit dem server Tick synchronisieren
    ''
    Public Overridable Sub pause(ByVal frames As Long) Implements IPlaylistItem.pause
        '' ToDo
        _paused = True
        RaiseEvent paused(Me)
    End Sub

    Public Overridable Sub unPause() Implements IPlaylistItem.unPause
        RaiseEvent started(Me)
    End Sub

    ''' <summary>
    ''' Start this item and all childItems. If noWait is given and false, start will wait till all items has been stopped and then end,
    ''' else it will start each item and then end imediatly
    ''' </summary>
    ''' <param name="noWait"></param>
    ''' <remarks></remarks>
    Public Overridable Sub start(Optional ByVal noWait As Boolean = False) Implements IPlaylistItem.start
        logger.debug("PlaylistItem.start: Start " & getName())

        playing = True
        RaiseEvent started(Me)

        ' alle Unteritems starten.
        ' Wenn parallel, dann wird nicht gewaret und alle starten
        ' semi gleichzeitig
        ' Sost startet das nächste erst wenn das vorherige fertig ist.
        If Not isAutoStarting() Then
            RaiseEvent waitForNext(Me)
            waiting = True
        Else
            playNextItem()
        End If
        logger.debug("PlaylistItem.start: Start " & getName() & " has been completed")
    End Sub

    Public Overridable Sub playNextItem(Optional ByRef lastPlayed As IPlaylistItem = Nothing) Implements IPlaylistItem.playNextItem

        If isParallel() Then
            'Stat all subitems
            For Each item In items
                AddHandler item.stopped, AddressOf itemStopped
                item.start()
            Next
        Else
            ' only start the next subitem
            Dim nextItem As IPlaylistItem = getNextToPlay(lastPlayed)
            waiting = False
            If Not IsNothing(nextItem) AndAlso Not nextItem.isPlaying Then
                AddHandler nextItem.stopped, AddressOf itemStopped
                AddHandler nextItem.aborted, AddressOf itemStopped
                AddHandler nextItem.canceled, AddressOf itemCanceled
                'AddHandler nextItem.started, AddressOf itemStarted
                nextItem.start()
            Else : RaiseEvent stopped(Me)
            End If
        End If
    End Sub

    Private Sub itemStopped(ByRef sender As IPlaylistItem)
        '' Check what kind of item stopped. If we're parallel and all items has stopped,
        '' but looping is active, start again.
        '' If seq. let playNext decide what to do.
        RemoveHandler sender.stopped, AddressOf itemStopped
        If isParallel() Then
            If isLooping() AndAlso Not isPlaying() Then
                start()
            End If
        Else
            playNextItem(sender)
        End If
    End Sub

    Private Sub itemStarted(ByRef sender As IPlaylistItem)
        RemoveHandler sender.started, AddressOf itemStarted
    End Sub

    Private Sub itemCanceled(ByRef sender As IPlaylistItem)
        ' if something unexpected happens, we schedule the next, but wait for the user to start it
        RemoveHandler sender.canceled, AddressOf itemCanceled
        playNextItem(sender)
    End Sub

    Private Function getNextToPlay(ByRef lastPlayed As IPlaylistItem) As IPlaylistItem
        If Not IsNothing(lastPlayed) AndAlso items.Contains(lastPlayed) Then
            ' calculate the next item
            '' LOCK the items list while doing this!!
            updateItems.WaitOne()
            Dim pos As Integer = items.IndexOf(lastPlayed)
            If pos = items.Count - 1 Then
                ' we played the last item. if loop, start again with first, else return nothing 
                If isLooping() Then
                    updateItems.Release()
                    Return items.First
                Else
                    updateItems.Release()
                    Return Nothing
                End If
            Else
                ' not the end of the list, so return next
                updateItems.Release()
                Return items.Item(pos + 1)
            End If
        ElseIf items.Count > 0 Then
            ' No current item, just play the first
            Return items.First
        Else
            ' no Item to play
            Return Nothing
        End If
    End Function

    Public Overrides Function toString() As String Implements IPlaylistItem.toString
        Return getName() & "(" & getItemType.ToString & ") " & getChannel() & "-" & getLayer() & " playing: " & isPlaying.ToString
    End Function

    Public Function toXML() As String Implements IPlaylistItem.toXML
        Dim xml As String = "<item><name>" & _
            name & "</name><type>" & _
            getItemType.ToString & "</type><layer>" & _
            layer.ToString & "</layer><channel>" & _
            channel.ToString & "</channel><autostarting>" & _
            isAutoStarting.ToString & "</autostarting><isParallel>" & _
            isParallel.ToString & "</isParallel><isLooping>" & _
            isLooping.ToString & "</isLooping><duration>" & _
            Duration.ToString & "</duration><delay>" & _
            delay.ToString & "</delay> "
        For Each item As IPlaylistItem In items
            xml = xml & item.toXML
        Next
        Return xml & "</item>"
    End Function

    Public Sub loadXML(ByVal xml As String) Implements IPlaylistItem.loadXML
        '' ToDo
    End Sub


    '' GETTER:
    ''--------

    Public Function getParent() As IPlaylistItem Implements IPlaylistItem.getParent
        Return parent
    End Function

    Public Function getChannel() As Integer Implements IPlaylistItem.getChannel
        Return channel
    End Function

    Public Function getDelay() As Long Implements IPlaylistItem.getDelay
        Return delay
    End Function

    Public Function getLayer() As Integer Implements IPlaylistItem.getLayer
        Return layer
    End Function

    Public Function getName() As String Implements IPlaylistItem.getName
        Return name
    End Function

    Public Function isAutoStarting() As Boolean Implements IPlaylistItem.isAutoStarting
        Return autoStart
    End Function

    Public Function isLooping() As Boolean Implements IPlaylistItem.isLooping
        Return looping
    End Function

    Public Function isParallel() As Boolean Implements IPlaylistItem.isParallel
        Return parallel
    End Function

    Public Function isWaiting() As Boolean Implements IPlaylistItem.isWaiting
        Return waiting AndAlso getController.isConnected
    End Function

    Public Overridable Function getDuration() As Long Implements IPlaylistItem.getDuration
        Return Duration
    End Function

    Public Function getItemType() As PlaylistItemTypes Implements IPlaylistItem.getItemType
        Return ItemType
    End Function

    Public Overridable Function getPosition() As Long Implements IPlaylistItem.getPosition
        Return Position
    End Function

    Public Overridable Function getRemaining() As Long Implements IPlaylistItem.getRemaining
        Return getDuration() - getPosition()
    End Function

    Public Function getPlayingChildItems(Optional ByVal recursiv As Boolean = False, Optional ByVal onlyPlayable As Boolean = False) As IEnumerable(Of IPlaylistItem) Implements IPlaylistItem.getPlayingChildItems
        Dim activeItems As New List(Of IPlaylistItem)
        For Each item In getChildItems(recursiv)
            If item.isPlaying Then
                If onlyPlayable Then
                    If item.isPlayable Then
                        activeItems.Add(item)
                    End If
                Else
                    activeItems.Add(item)
                End If
            End If
        Next
        Return activeItems
    End Function

    Public Function getChildItems(Optional ByVal recursiv As Boolean = False) As List(Of IPlaylistItem) Implements IPlaylistItem.getChildItems
        Dim childItems As New List(Of IPlaylistItem)
        For Each item In items
            childItems.Add(item)
            If recursiv Then
                childItems.AddRange(item.getChildItems(recursiv))
            End If
        Next
        Return childItems
    End Function

    Public Overridable Function getPlayed() As Byte Implements IPlaylistItem.getPlayed
        If getDuration() > 0 AndAlso getController.isConnected Then
            Return Math.Min(100, (100 / getDuration()) * getPosition())
        Else
            Return 0
        End If
    End Function

    Public Overridable Function isPlaying() As Boolean Implements IPlaylistItem.isPlaying
        Return playing AndAlso getController.isConnected
    End Function

    Public Function isPaused() As Boolean Implements IPlaylistItem.isPaused
        Return _paused AndAlso getController.isConnected
    End Function

    Public Function isPlayable() As Boolean Implements IPlaylistItem.isPlayable
        Select Case getItemType()
            Case PlaylistItem.PlaylistItemTypes.AUDIO, PlaylistItem.PlaylistItemTypes.MOVIE, PlaylistItem.PlaylistItemTypes.STILL
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function getController() As ServerControler Implements IPlaylistItem.getController
        Return controller
    End Function

    Public Overridable Function getMedia() As CasparCGMedia Implements IPlaylistItem.getMedia
        Return Nothing
    End Function

    Friend Function getFPS() As Integer Implements IPlaylistItem.getFPS
        If fps <= 0 Then
            Return getController.getFPS(getChannel)
        Else
            Return fps
        End If
    End Function

    Public Function hasPlayingParent() As Boolean Implements IPlaylistItem.hasPlayingParent
        If IsNothing(getParent) Then
            Return False
        Else
            Return getParent.isPlaying OrElse getParent.hasPlayingParent
        End If
    End Function


    '' SETTER:
    ''---------

    Public Sub setParent(ByRef parent As IPlaylistItem) Implements IPlaylistItem.setParent
        Me.parent = parent
        RaiseEvent changed(Me)
    End Sub

    Public Sub setName(ByVal Name As String) Implements IPlaylistItem.setName
        Me.name = Name
        RaiseEvent changed(Me)
    End Sub

    Public Sub setChildItems(ByRef items As System.Collections.Generic.List(Of IPlaylistItem)) Implements IPlaylistItem.setChildItems
        ' einzeln und nicht als block hinzufügen damit die duration berechnet wird
        updateItems.WaitOne()
        For Each item In items
            addItem(item)
        Next
        updateItems.Release()
        RaiseEvent changed(Me)
    End Sub

    Public Sub setDuration(ByVal duration As Long) Implements IPlaylistItem.setDuration
        Me.Duration = duration
        RaiseEvent changed(Me)
    End Sub

    Public Overridable Sub setPosition(ByVal position As Long) Implements IPlaylistItem.setPosition
        Me.Position = position
        'RaiseEvent changed 
    End Sub

    Public Sub setRemaining(ByVal remaining As Long) Implements IPlaylistItem.setRemaining
        Me.Remaining = remaining
        'RaiseEvent changed 
    End Sub

    Public Sub addItem(ByRef item As IPlaylistItem) Implements IPlaylistItem.addItem
        logger.log("PlaylistItem.addItem: " & getName() & "(" & getChannel() & "-" & getLayer() & "): Adding new Item " & item.getName)
        If Not IsNothing(item) Then
            If item.getChannel < 0 Then
                item.setChannel(getChannel)
            End If
            If item.getLayer < 0 Then
                item.setLayer(getLayer)
            End If

            item.setParent(Me)
            updateItems.WaitOne()
            items.Add(item)
            updateItems.Release()
            If isParallel() Then
                setDuration(Math.Max(getDuration, item.getDuration))
            Else
                setDuration(getDuration() + item.getDuration)
            End If
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Sub setAutoStart(ByVal autoStart As Boolean) Implements IPlaylistItem.setAutoStart
        If autoStart <> isAutoStarting() Then
            Me.autoStart = autoStart
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Overridable Sub setChannel(ByVal channel As Integer) Implements IPlaylistItem.setChannel
        If channel <> -1 Then
            If Not controller.containsChannel(channel) Then
                logger.warn("PlaylistItem.setChannel: Playlist " & getName() & ": The channel " & channel & " is not configured at the given server. This could lead to errors during playlist playback.")
                fps = -1
            Else
                fps = getController.getFPS(channel)
            End If
        End If
        Me.channel = channel
        RaiseEvent changed(Me)
    End Sub

    Public Sub setDelay(ByVal delay As Long) Implements IPlaylistItem.setDelay
        Me.delay = delay
        RaiseEvent changed(Me)
    End Sub

    Public Sub setLayer(ByVal layer As Integer) Implements IPlaylistItem.setLayer
        If layer > -2 Then
            Me.layer = layer
        Else
            logger.warn("PlaylistItem.setLayer: Playlist " & getName() & ": Can't set layer to " & layer & ". Leaving it unset which means it will be the standard layer")
            Me.layer = -1
        End If
        RaiseEvent changed(Me)
    End Sub

    Public Sub setLooping(ByVal looping As Boolean) Implements IPlaylistItem.setLooping
        If Me.isLooping <> looping Then
            Me.looping = looping
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Sub setParallel(ByVal parallel As Boolean) Implements IPlaylistItem.setParallel
        If Me.isParallel <> parallel Then
            Me.parallel = parallel
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Sub removeChild(ByRef child As IPlaylistItem) Implements IPlaylistItem.removeChild
        If items.Contains(child) Then
            updateItems.WaitOne()
            items.Remove(child)
            updateItems.Release()
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Sub insertChildAt(ByRef child As IPlaylistItem, ByRef position As IPlaylistItem) Implements IPlaylistItem.insertChildAt
        If items.Contains(position) Then
            updateItems.WaitOne()
            items.Insert(items.IndexOf(position), child)
            updateItems.Release()
            RaiseEvent changed(Me)
        End If
    End Sub
End Class
