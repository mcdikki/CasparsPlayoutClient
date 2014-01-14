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

Public MustInherit Class AbstractPlaylistItem
    Implements IPlaylistItem

    Private name As String
    Private layer As Integer
    Private channel As Integer
    Private delay As Long
    Private looping As Boolean
    Private autoStart As Boolean
    Private autoLoad As Boolean = True
    Private parallel As Boolean
    Private fps As Integer
    Private parent As IPlaylistItem

    ' Die (Kinder)Items dieses Items
    Private items As List(Of IPlaylistItem)
    Private WithEvents controller As ServerController
    Private duration As Long ' Gesamtlaufzeit in Frames
    Private position As Long ' aktuelle Frame
    Private remaining As Long ' noch zu spielende Frames
    Private itemType As AbstractPlaylistItem.PlaylistItemTypes ' Typ des Item
    Protected playing As Boolean
    Private _paused As Boolean
    Protected waiting As Boolean = False
    Private _clearAfterPlayback As Boolean = False

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
    ''' <param name="controler"></param>
    ''' <param name="duration"></param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal name As String, ByVal itemType As PlaylistItemTypes, ByRef controler As ServerController, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        Me.name = name
        Me.itemType = itemType
        Me.controller = controler
        setChannel(channel)
        setLayer(layer)
        If duration > -1 Then
            setDuration(duration)
        End If
        items = New List(Of IPlaylistItem)
    End Sub


    '' ABSTRACT MEMBERS
    ''------------------

    Public MustOverride Sub halt() Implements IPlaylistItem.halt

    Public MustOverride Sub abort() Implements IPlaylistItem.abort

    Public MustOverride Sub stoppedPlaying() Implements IPlaylistItem.stoppedPlaying

    Public MustOverride Sub load() Implements IPlaylistItem.load

    Public MustOverride Sub show() Implements IPlaylistItem.show

    Public MustOverride Sub pause(ByVal frames As Long) Implements IPlaylistItem.pause

    Public MustOverride Sub unPause() Implements IPlaylistItem.unPause

    ''' <summary>
    ''' Start this item and all childItems. If noWait is given and false, start will wait till all items has been stopped and then end,
    ''' else it will start each item and then end imediatly
    ''' </summary>
    Public MustOverride Sub start() Implements IPlaylistItem.start

    Public MustOverride Sub playNextItem(Optional ByRef lastPlayed As IPlaylistItem = Nothing) Implements IPlaylistItem.playNextItem


    '' COMMON MEMBERS SHARED BY ALL PLAYLISTS
    ''----------------------------------------

    Public Overrides Function toString() As String Implements IPlaylistItem.toString
        Return getName() & "(" & getItemType.ToString & ") " & getChannel() & "-" & getLayer() & " playing: " & isPlaying.ToString
    End Function

    Public Overridable Function toXML() As MSXML2.DOMDocument Implements IPlaylistItem.toXML
        Dim configDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMNode
        Dim node As MSXML2.IXMLDOMNode
        ' Head and ID data
        pnode = configDoc.createElement("playlist")
        node = configDoc.createElement("name")
        node.nodeTypedValue = getName()
        pnode.appendChild(node)
        node = configDoc.createElement("type")
        node.nodeTypedValue = getItemType()
        pnode.appendChild(node)
        node = configDoc.createElement("typename")
        node.nodeTypedValue = getItemType().ToString
        pnode.appendChild(node)

        ' Class members
        node = configDoc.createElement("channel")
        node.nodeTypedValue = getChannel()
        pnode.appendChild(node)
        node = configDoc.createElement("layer")
        node.nodeTypedValue = getLayer()
        pnode.appendChild(node)
        node = configDoc.createElement("loop")
        node.nodeTypedValue = isLooping()
        pnode.appendChild(node)
        node = configDoc.createElement("autostart")
        node.nodeTypedValue = isAutoStarting()
        pnode.appendChild(node)
        node = configDoc.createElement("parallel")
        node.nodeTypedValue = isParallel()
        pnode.appendChild(node)
        node = configDoc.createElement("delay")
        node.nodeTypedValue = getDelay()
        pnode.appendChild(node)
        node = configDoc.createElement("duration")
        node.nodeTypedValue = MyClass.getDuration()
        pnode.appendChild(node)
        node = configDoc.createElement("clearAfterPlayback")
        node.nodeTypedValue = ClearAfterPlayback()
        pnode.appendChild(node)

        ' media if any
        If Not IsNothing(getMedia) Then
            pnode.appendChild(getMedia.toXml.firstChild())
        End If

        '' Add all subplaylists
        For Each sp As IPlaylistItem In items
            pnode.appendChild(sp.toXML.firstChild)
        Next

        '' Trigger (not yet implementet
        node = configDoc.createElement("trigger")
        pnode.appendChild(node)

        configDoc.appendChild(pnode)
        Return configDoc
    End Function

    Public Function toXMLString() As String Implements IPlaylistItem.toXMLString
        Return toXML.xml
    End Function

    Public Sub loadXML(ByVal xml As String) Implements IPlaylistItem.loadXML
        Dim confDoc As New MSXML2.DOMDocument
        confDoc.loadXML(xml)
        loadXML(confDoc)
    End Sub

    Public Sub loadXML(ByRef xmlDoc As MSXML2.DOMDocument) Implements IPlaylistItem.loadXML
        '' ToDo
        If Not IsNothing(xmlDoc) AndAlso xmlDoc.parsed Then
            If Not IsNothing(xmlDoc.firstChild) AndAlso xmlDoc.firstChild.nodeName.Equals("playlist") AndAlso Not IsNothing(xmlDoc.firstChild.selectSingleNode("name")) Then
                ' remove old subplaylists
                Dim tItems() = items.ToArray
                For Each item In titems
                    removeChild(item)
                Next

                ' read in new settings
                setName(xmlDoc.firstChild.selectSingleNode("name").nodeTypedValue)

                setChannel(xmlDoc.firstChild.selectSingleNode("channel").nodeTypedValue)
                setLayer(xmlDoc.firstChild.selectSingleNode("layer").nodeTypedValue)

                setLooping(xmlDoc.firstChild.selectSingleNode("loop").nodeTypedValue)
                setAutoStart(xmlDoc.firstChild.selectSingleNode("autostart").nodeTypedValue)
                setParallel(xmlDoc.firstChild.selectSingleNode("parallel").nodeTypedValue)
                setClearAfterPlayback(xmlDoc.firstChild.selectSingleNode("clearAfterPlayback").nodeTypedValue)
                setDelay(xmlDoc.firstChild.selectSingleNode("delay").nodeTypedValue)
                setDuration(xmlDoc.firstChild.selectSingleNode("duration").nodeTypedValue)

                ' add subplayliss
                For Each child As MSXML2.IXMLDOMElement In xmlDoc.firstChild.selectNodes("playlist")
                    addItem(PlaylistFactory.getPlaylist(child.xml, getController))
                Next
            Else
                logger.err("AbstractPlaylist.loadXml: Error reading xml. Not valid playlist definition.")
            End If
        Else
            logger.err("AbstractPlaylist.loadXml: Error reading xml. No or empty xml document given.")
        End If
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

    Public Function isAutoLoading() As Boolean Implements IPlaylistItem.isAutoLoading
        Return autoLoad
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
        Return duration
    End Function

    Public Function getItemType() As PlaylistItemTypes Implements IPlaylistItem.getItemType
        Return itemType
    End Function

    Public Overridable Function getPosition() As Long Implements IPlaylistItem.getPosition
        Return position
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
            Return Math.Min(100, (100 / getDuration()) * Math.Max(getPosition(), 0))
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
            Case AbstractPlaylistItem.PlaylistItemTypes.AUDIO, AbstractPlaylistItem.PlaylistItemTypes.MOVIE, AbstractPlaylistItem.PlaylistItemTypes.STILL
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function getController() As ServerController Implements IPlaylistItem.getController
        Return controller
    End Function

    Public Overridable Function getMedia() As AbstractCasparCGMedia Implements IPlaylistItem.getMedia
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

    Public Overridable Function isLoaded() As Boolean Implements IPlaylistItem.isLoaded
        Return False
    End Function

    Public Overridable Function isShowing() As Boolean Implements IPlaylistItem.isShowing
        Return False
    End Function

    Public Overridable Function ClearAfterPlayback() As Boolean Implements IPlaylistItem.clearAfterPlayback
        Return _clearAfterPlayback
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

    Public Overridable Sub setDuration(ByVal duration As Long) Implements IPlaylistItem.setDuration
        Me.duration = duration
        RaiseEvent changed(Me)
    End Sub

    Public Overridable Sub setPosition(ByVal position As Long) Implements IPlaylistItem.setPosition
        Me.position = position
        RaiseEvent changed(Me)
    End Sub

    Public Overridable Sub setRemaining(ByVal remaining As Long) Implements IPlaylistItem.setRemaining
        Me.remaining = remaining
        RaiseEvent changed(Me)
    End Sub

    Public Sub setAutoStart(ByVal autoStart As Boolean) Implements IPlaylistItem.setAutoStart
        If autoStart <> isAutoStarting() Then
            Me.autoStart = autoStart
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Sub setAutoLoad(ByVal autoLoad As Boolean) Implements IPlaylistItem.setAutoLoad
        If autoLoad <> isAutoLoading() Then
            Me.autoLoad = autoLoad
            RaiseEvent changed(Me)
        End If
    End Sub

    Public Overridable Sub setChannel(ByVal channel As Integer) Implements IPlaylistItem.setChannel
        If channel > -1 Then
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

    Public Overridable Sub setDelay(ByVal delay As Long) Implements IPlaylistItem.setDelay
        Me.delay = delay
        RaiseEvent changed(Me)
    End Sub

    Public Overridable Sub setLayer(ByVal layer As Integer) Implements IPlaylistItem.setLayer
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

    Public Sub setClearAfterPlayback(Optional ByVal active As Boolean = True) Implements IPlaylistItem.setClearAfterPlayback
        _clearAfterPlayback = active
    End Sub

    Public Sub removeChild(ByRef child As IPlaylistItem) Implements IPlaylistItem.removeChild
        If items.Contains(child) Then
            logger.log("Playlist " + getName() + ": Remove " + child.getName)
            updateItems.WaitOne()
            items.Remove(child)
            updateItems.Release()
            child.setParent(Nothing)
            RemoveHandler child.changed, AddressOf raiseChanged
            RaiseEvent changed(Me)
        Else
            logger.warn("Playlist " + getName() + ": Can't remove " + child.getName + ". No such child playlist found.")
        End If
    End Sub

    Public Sub insertChildAt(ByRef child As IPlaylistItem, ByRef position As IPlaylistItem) Implements IPlaylistItem.insertChildAt
        If Not IsNothing(child) AndAlso items.Contains(position) Then
            logger.log("Playlist " + getName() + ": Insert item " + child.getName + " at position of " + position.getName)
            If child.getChannel < 0 Then
                child.setChannel(getChannel)
            End If
            If child.getLayer < 0 Then
                child.setLayer(getLayer)
            End If

            If items.Contains(child) Then
                ' The item is already in our list, so we have to care if child and position are neighbors
                If items.IndexOf(child) + 1 = items.IndexOf(position) Then
                    ' special case, they are neighbors and child is before pos
                    ' -> instead of moving child to pos, we move pos. to child
                    removeChild(position)
                    position.setParent(Me)
                    updateItems.WaitOne()
                    items.Insert(items.IndexOf(child), position)
                    updateItems.Release()
                    AddHandler position.changed, AddressOf raiseChanged
                Else
                    removeChild(child)
                    child.setParent(Me)
                    updateItems.WaitOne()
                    items.Insert(items.IndexOf(position), child)
                    updateItems.Release()
                    AddHandler child.changed, AddressOf raiseChanged
                End If
            Else
                If Not IsNothing(child.getParent) Then child.getParent.removeChild(child)
                child.setParent(Me)
                updateItems.WaitOne()
                items.Insert(items.IndexOf(position), child)
                updateItems.Release()
                AddHandler child.changed, AddressOf raiseChanged
            End If

            If isParallel() Then
                setDuration(Math.Max(getDuration, child.getDuration))
            Else
                setDuration(getDuration() + child.getDuration)
            End If
            RaiseEvent changed(Me)
        End If
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
            AddHandler item.changed, AddressOf raiseChanged
            RaiseEvent changed(Me)
        End If
    End Sub

    Protected Sub raiseStopped(ByRef sender As IPlaylistItem)
        RaiseEvent stopped(sender)
    End Sub

    Protected Sub raiseStarted(ByRef sender As IPlaylistItem)
        RaiseEvent started(sender)
    End Sub

    Protected Sub raiseAborted(ByRef sender As IPlaylistItem)
        RaiseEvent aborted(sender)
    End Sub

    Protected Sub raiseCanceled(ByRef sender As IPlaylistItem)
        RaiseEvent canceled(sender)
    End Sub

    Protected Sub raiseWaitForNext(ByRef sender As IPlaylistItem)
        RaiseEvent waitForNext(sender)
    End Sub

    Protected Sub raisePaused(ByRef sender As IPlaylistItem)
        RaiseEvent paused(sender)
    End Sub

    Protected Sub raiseChanged(ByRef sender As IPlaylistItem)
        RaiseEvent changed(sender)
    End Sub
End Class
