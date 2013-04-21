Imports System.Threading

Public Class PlaylistBlockItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private lastPlaying As Integer

    Public Sub New(ByVal name As String, ByRef controller As ServerController, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1)
        MyBase.New(name, PlaylistItemTypes.BLOCK, controller, channel, layer, 0)
    End Sub

    Public Overloads Function isPlaying()
        For Each child In getChildItems()
            If child.isPlaying Then Return True
        Next
        Return False
    End Function

    Public Overrides Sub start(Optional ByVal noWait As Boolean = False)
        MyBase.start(noWait)

        While isPlaying() AndAlso Not isParallel()
            Thread.Sleep(1)
        End While
    End Sub

    Public Overrides Function getPosition() As Long
        Dim pos As Long
        For Each child In getChildItems()
            If isParallel() Then
                pos = Math.Max(pos, child.getPosition)
            Else
                pos = pos + child.getPosition
            End If
        Next
        Return pos
    End Function

End Class
