Public Class PlaylistAudioItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGAudio

    Public Sub New(ByVal name As String, ByRef controller As ServerControler, ByVal audio As CasparCGAudio, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.AUDIO, controller, channel, layer, duration)
    End Sub

End Class