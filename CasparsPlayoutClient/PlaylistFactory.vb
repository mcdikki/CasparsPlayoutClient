Imports CasparCGNETConnector

Public Class PlaylistFactory

    Public Shared Function getPlaylist(ByRef xml As String, ByRef controller As ServerController) As IPlaylistItem
        Dim confDoc As New MSXML2.DOMDocument
        confDoc.loadXML(xml)
        Return getPlaylist(confDoc, controller)
    End Function

    Public Shared Function getPlaylist(ByRef xml As MSXML2.DOMDocument, ByRef controller As ServerController) As IPlaylistItem
        If Not IsNothing(xml) AndAlso xml.hasChildNodes AndAlso xml.firstChild.nodeName.Equals("playlist") Then
            '' ToDo
            Dim pnode As MSXML2.IXMLDOMElement = xml.firstChild
            Dim playlist As IPlaylistItem
            Dim name As String = pnode.selectSingleNode("name").nodeTypedValue

            Select Case pnode.selectSingleNode("type").nodeTypedValue
                Case AbstractPlaylistItem.PlaylistItemTypes.BLOCK
                    playlist = New PlaylistBlockItem(name, controller)
                Case AbstractPlaylistItem.PlaylistItemTypes.MOVIE
                    playlist = New PlaylistMovieItem(name, controller, CasparCGMediaFactory.createMedia(pnode.selectSingleNode("media").xml))
                Case AbstractPlaylistItem.PlaylistItemTypes.STILL
                    playlist = New PlaylistStillItem(name, controller, CasparCGMediaFactory.createMedia(pnode.selectSingleNode("media").xml))
                Case AbstractPlaylistItem.PlaylistItemTypes.COMMAND
                    ' ToDo Command füllen
                    playlist = New PlaylistCommandItem(name, controller, CasparCGCommandFactory.getCommand(pnode.selectSingleNode("command").selectSingleNode("type").nodeTypedValue))
                Case AbstractPlaylistItem.PlaylistItemTypes.TEMPLATE
                    playlist = New PlaylistTemplateItem(name, controller, CasparCGMediaFactory.createMedia(pnode.selectSingleNode("media").xml))
                Case AbstractPlaylistItem.PlaylistItemTypes.AUDIO
                    playlist = New PlaylistAudioItem(name, controller, CasparCGMediaFactory.createMedia(pnode.selectSingleNode("media").xml))
                Case Else
                    logger.err("PlaylistFactory.getPlaylist: Error reading xml. Unknown playlist type.")
                    Return Nothing
            End Select

            playlist.loadXML(xml)

            Return playlist
        Else
            Return Nothing
        End If
    End Function

End Class
