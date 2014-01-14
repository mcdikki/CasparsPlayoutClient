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

Public Interface IPlaylistItem

    Function getName() As String ' Name des Items
    Function getDelay() As Long ' Verzögerung bis zum start dieses Items in Frames
    Function getLayer() As Integer ' Layer des Items
    Function getChannel() As Integer ' Server Channel des Items
    Function isPlayable() As Boolean
    Function isLooping() As Boolean
    Function isAutoStarting() As Boolean
    Function isAutoLoading() As Boolean
    Function isParallel() As Boolean
    Function isLoaded() As Boolean
    Function isShowing() As Boolean

    Function getDuration() As Long ' Gesamtlaufzeit in ms
    Function getPosition() As Long ' aktuelle ms
    Function getRemaining() As Long ' noch zu spielende ms
    Function getItemType() As AbstractPlaylistItem.PlaylistItemTypes ' Typ des Item
    Function isPlaying() As Boolean
    Function isPaused() As Boolean
    Function isWaiting() As Boolean
    Function clearAfterPlayback() As Boolean
    Function getPlayed() As Byte '% des Items gespielt
    Function getChildItems(Optional ByVal recursiv As Boolean = False) As List(Of IPlaylistItem) ' alle Items in diesem Item
    Function getPlayingChildItems(Optional ByVal recursiv As Boolean = False, Optional ByVal onlyPlayable As Boolean = False) As IEnumerable(Of IPlaylistItem) ' alle activen, spielenden Items in diesem Item
    Function getMedia() As AbstractCasparCGMedia
    Function getFPS() As Integer
    Function getController() As ServerController
    Function getParent() As IPlaylistItem
    Function hasPlayingParent() As Boolean
    'Function getLayerUser(Optional ByVal recursiv As Boolean = False) As Dictionary(Of Integer, Integer)
    Function toXMLString() As String
    Function toXML() As MSXML2.DOMDocument
    Function toString() As String

    Sub setParent(ByRef parent As IPlaylistItem)
    Sub setName(ByVal name As String)
    Sub setLayer(ByVal layer As Integer)
    Sub setChannel(ByVal channel As Integer)
    Sub setLooping(ByVal looping As Boolean)
    Sub setAutoStart(ByVal autoStart As Boolean)
    Sub setAutoLoad(ByVal autoLoad As Boolean)
    Sub setParallel(ByVal parallel As Boolean)
    Sub setClearAfterPlayback(Optional ByVal active As Boolean = True)
    Sub setDelay(ByVal delay As Long)
    Sub setDuration(ByVal duration As Long) ' Gesamtlaufzeit in ms
    Sub setPosition(ByVal position As Long) ' aktuelle ms
    Sub setRemaining(ByVal remaining As Long) ' noch zu spielende ms
    Sub setChildItems(ByRef items As List(Of IPlaylistItem))
    Sub addItem(ByRef item As IPlaylistItem)
    Sub removeChild(ByRef child As IPlaylistItem)
    Sub insertChildAt(ByRef child As IPlaylistItem, ByRef position As IPlaylistItem)


    Sub loadXML(ByVal xml As String) ' Erstellt ein IPlaylistItem aus einer xml definition)
    Sub loadXML(ByRef xmlDoc As MSXML2.DOMDocument)


    Sub load() ' lädt wenn möglich item schon im Hintergrund (ACMP loadbg)
    Sub show() ' load to foreground (ACMP load)
    Sub start()
    Sub playNextItem(Optional ByRef lastPlayed As IPlaylistItem = Nothing)
    Sub abort() ' bricht ausführung ab
    Sub halt()  'stopt item (ACMP stop)
    Sub stoppedPlaying() ' informs item that it has stopped playing
    Sub pause() ' pausiert das Spielen des Items for frames Frames oder bis zum manuellen start bei 0 (ACMP pause)
    Sub unPause()

    Event waitForNext(ByRef sender As IPlaylistItem)
    Event stopped(ByRef sender As IPlaylistItem)
    Event started(ByRef sender As IPlaylistItem)
    Event paused(ByRef sender As IPlaylistItem)
    Event unpaused(ByRef sender As IPlaylistItem)
    Event aborted(ByRef sender As IPlaylistItem)
    Event changed(ByRef sender As IPlaylistItem)
    Event canceled(ByRef sender As IPlaylistItem)

End Interface
