Public Class PlaylistItem
    Implements IPlaylistItem

    ' Die (Kinder)Items dieses Items
    Private items As List(Of IPlaylistItem)
    Private controller As ServerController
    Private Duration As Long ' Gesamtlaufzeit in Frames
    Private Position As Long ' aktuelle Frame
    Private Remaining As Long ' noch zu spielende Frames
    Private ItemType As PlaylistItem.PlaylistItemTypes ' Typ des Item
    Private playing As Boolean
    Private paused As Boolean
    Private startThread As Threading.Thread
    Private pauseThread As Threading.Thread

    ' Properties
    Property Name As String Implements IPlaylistItem.Name  ' Name des Items
    Property Delay As Long Implements IPlaylistItem.Delay
    Property Layer As Integer Implements IPlaylistItem.Layer
    Property Channel As Integer Implements IPlaylistItem.Channel
    Property isLooping As Boolean Implements IPlaylistItem.isLooping
    Property isAutoStarting As Boolean Implements IPlaylistItem.isAutoStarting
    Property isParallel As Boolean Implements IPlaylistItem.isParallel

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
    Protected Sub New(ByVal name As String, ByVal itemType As PlaylistItemTypes, ByRef controller As ServerController, ByVal duration As Long)
        Me.Name = name
        Me.ItemType = itemType
        Me.controller = controller
        setDuration(duration)
        items = New List(Of IPlaylistItem)
    End Sub

    Public Sub abort() Implements IPlaylistItem.abort
        If Not IsNothing(startThread) Then
            startThread.Abort()
            startThread = Nothing
        End If
        If Not IsNothing(pauseThread) Then
            pauseThread.Abort()
            pauseThread = Nothing
        End If
        For Each item In items
            item.abort()
        Next
    End Sub

    Public Sub load() Implements IPlaylistItem.load
        '' Bin mir noch nicht sicher ob es wirklich sinnvoll ist das zu implementieren...? 
    End Sub

    Public Sub pause(ByVal frames As Long) Implements IPlaylistItem.pause
        If Not IsNothing(startThread) Then
            startThread.Suspend()
        End If
        For Each item In items
            item.pause(frames)
        Next

        '' Wenn gesagt wird wie lange die pause geht, starte einen kleinen 
        '' wecker der nach dieser Zeit die Pause aufhebt.
        If frames >= 0 Then
            pauseThread = New Threading.Thread(AddressOf Me.unPause)
            pauseThread.Start(controller.getTimeInMS(frames, Channel))
        End If
        paused = True
    End Sub

    Public Sub unPause(ByVal time As Long)
        Threading.Thread.Sleep(time)
        unPause()
    End Sub

    Public Sub unPause() Implements IPlaylistItem.unPause
        For Each item In items
            item.unPause()
        Next
        If Not IsNothing(startThread) Then
            startThread.Resume()
        End If
        pauseThread = Nothing
        paused = False
    End Sub

    ''' <summary>
    ''' Start this item and all childItems. If noWait is given and false, start will wait till all items has been stopped and then end,
    ''' else it will start each item then then end imediatly
    ''' </summary>
    ''' <param name="noWait"></param>
    ''' <remarks></remarks>
    Public Sub start(Optional ByVal noWait As Boolean = True) Implements IPlaylistItem.start
        If noWait AndAlso IsNothing(startThread) Then
            startThread = New Threading.Thread(AddressOf Me.start)
            startThread.Start(noWait)
        Else
            playing = True

            ' alle Unteritems starten.
            ' Wenn parallel, dann wird nicht gewaret und alle starten
            ' semi gleichzeitig
            ' Sost startet das nächste erst wenn das vorherige fertig ist.
            For Each item In items
                item.start(isParallel)
            Next
        End If
    End Sub

    Public Overrides Function toString() As String Implements IPlaylistItem.toString
        Return MyBase.ToString
    End Function

    Public Function toXML() As String Implements IPlaylistItem.toXML
        Dim xml As String = "<item><name>" & _
            Name & "</name><type>" & _
            getItemType.ToString & "</type><layer>" & _
            Layer.ToString & "</layer><channel>" & _
            Channel.ToString & "</channel><autostarting>" & _
            isAutoStarting.ToString & "</autostarting><isParallel>" & _
            isParallel.ToString & "</isParallel><isLooping>" & _
            isLooping.ToString & "</isLooping><duration>" & _
            Duration.ToString & "</duration>"
        For Each item As IPlaylistItem In items
            xml = xml & item.toXML
        Next
        Return xml & "</item>"
    End Function


    '' GETTER:
    ''--------
    Public Function getDuration() As Long Implements IPlaylistItem.getDuration
        Return Duration
    End Function

    Public Function getItemType() As PlaylistItemTypes Implements IPlaylistItem.getItemType
        Return ItemType
    End Function

    Public Function getPosition() As Long Implements IPlaylistItem.getPosition
        Return Position
    End Function

    Public Function getRemaining() As Long Implements IPlaylistItem.getRemaining
        Return Remaining
    End Function

    Public Function getActiveChildItems(ByVal recursiv As Boolean) As System.Collections.Generic.List(Of IPlaylistItem) Implements IPlaylistItem.getActiveChildItems
        Dim activeItems As New List(Of IPlaylistItem)
        For Each item In items
            If item.isPlaying Then
                activeItems.Add(item)
                If recursiv Then
                    activeItems.AddRange(item.getActiveChildItems(recursiv))
                End If
            End If
        Next
        Return activeItems
    End Function

    Public Function getChildItems() As System.Collections.Generic.List(Of IPlaylistItem) Implements IPlaylistItem.getChildItems
        Return items
    End Function

    Public Function getPlayed() As Single Implements IPlaylistItem.getPlayed
        Return (1 / Duration) * Position
    End Function

    Public Function isPlaying() As Boolean Implements IPlaylistItem.isPlaying
        Return playing
    End Function

    Public Function isPaused() As Boolean Implements IPlaylistItem.isPaused
        Return paused
    End Function


    '' SETTER:
    ''---------
    Public Sub setChildItems(ByRef items As System.Collections.Generic.List(Of IPlaylistItem)) Implements IPlaylistItem.setChildItems
        ' einzeln und nicht als block hinzufügen damit die duration berechnet wird
        For Each item In items
            addItem(item)
        Next
    End Sub

    Public Sub setDuration(ByVal duration As Long) Implements IPlaylistItem.setDuration
        Me.Duration = duration
    End Sub

    Public Sub setPosition(ByVal position As Long) Implements IPlaylistItem.setPosition
        Me.Position = position
    End Sub

    Public Sub setRemaining(ByVal remaining As Long) Implements IPlaylistItem.setRemaining
        Me.Remaining = remaining
    End Sub

    Public Sub addItem(ByRef item As IPlaylistItem) Implements IPlaylistItem.addItem
        If Not IsNothing(item) Then
            items.Add(item)
            If isParallel Then
                setDuration(Math.Max(getDuration, item.getDuration))
            Else
                setDuration(getDuration() + item.getDuration)
            End If
        End If
    End Sub
End Class
