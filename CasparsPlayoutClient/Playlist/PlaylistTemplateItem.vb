Public Class PlaylistTemplateItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGTemplate
    Private flashlayer As Integer = -1

    Public Sub New(ByVal name As String, ByRef controller As ServerControler, ByVal template As CasparCGTemplate, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal flashlayer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.TEMPLATE, controller, channel, layer, duration)
    End Sub
End Class
