Public Class PlaylistCommandItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByVal command As String, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1)
        MyBase.New(name, PlaylistItemTypes.COMMAND, controller, channel, layer)
    End Sub
End Class
