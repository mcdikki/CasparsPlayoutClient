Imports CasparCGVBNETConnector
Imports logger

Public Class PlaylistStillItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGStill

    Public Sub New(ByVal name As String, ByRef controller As ServerControler, ByVal still As CasparCGStill, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.STILL, controller, channel, layer, duration)
    End Sub
End Class
