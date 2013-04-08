Public Interface IPlaylistItem

    ' Properties sind alle Werte des Items die vom User gesetzt werden sollen
    ' Werte die vom Server kommen oder nur beim erzeugen gesetzt werden
    ' sollen über Getter/Setter verarbeitet werden, auch wenn das gegen das Prinzip von 
    ' Properties verstößt hat es den vorteil, das ich über .GetType.GetProperties
    ' nachher alle zugreifbaren member des Items als liste bekommen kann ohne sie vorher zu kennen

    Property Name As String ' Name des Items
    Property Delay As Long ' Verzögerung bis zum start dieses Items in Frames
    Property Layer As Integer ' Layer des Items
    Property Channel As Integer ' Server Channel des Items

    Property isLooping As Boolean
    Property isAutoStarting As Boolean
    Property isParallel As Boolean


    Function getDuration() As Long ' Gesamtlaufzeit in Frames
    Function getPosition() As Long ' aktuelle Frame
    Function getRemaining() As Long ' noch zu spielende Frames
    Function getItemType() As PlaylistItem.PlaylistItemTypes ' Typ des Item
    Function isPlaying() As Boolean
    Function isPaused() As Boolean
    Function getPlayed() As Single '% des Items gespielt
    Function getChildItems() As List(Of IPlaylistItem) ' alle Items in diesem Item
    Function getActiveChildItems(ByVal recursiv As Boolean) As List(Of IPlaylistItem) ' alle activen, spielenden Items in diesem Item
    Function toXML() As String
    Function toString() As String


    Sub setDuration(ByVal duration As Long) ' Gesamtlaufzeit in Frames
    Sub setPosition(ByVal position As Long) ' aktuelle Frame
    Sub setRemaining(ByVal remaining As Long) ' noch zu spielende Frames
    Sub setChildItems(ByRef items As List(Of IPlaylistItem))
    Sub addItem(ByRef item As IPlaylistItem)

    Sub load() ' lädt wenn möglich item schon im Hintergrund (ACMP loadbg)
    Sub start(Optional ByVal noWait As Boolean = True)
    Sub abort() ' stopt item (ACMP stop)
    Sub pause(ByVal frames As Long) ' pausiert das Spielen des Items for frames Frames oder bis zum manuellen start bei 0 (ACMP pause)
    Sub unPause()

End Interface
