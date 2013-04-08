Public Class PlaylistBlockItem
    Inherits PlaylistItem

    Public Sub New(ByVal name As String, ByRef controller As ServerController)
        MyBase.New(name, PlaylistItemTypes.BLOCK, controller, 0)
    End Sub

End Class
