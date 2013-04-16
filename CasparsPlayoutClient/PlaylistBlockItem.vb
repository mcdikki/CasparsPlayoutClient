Imports System.Threading

Public Class PlaylistBlockItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private lastPlaying As IPlaylistItem

    Public Sub New(ByVal name As String, ByRef controller As ServerController, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1)
        MyBase.New(name, PlaylistItemTypes.BLOCK, controller, channel, layer, 0)
    End Sub

    Public Overloads Function isPlaying()
        For Each child In getChildItems()
            If child.isPlaying Then Return True
        Next
        Return False
    End Function

    Private Sub receiveTick(ByVal sender As Object, ByVal e As frameTickEventArgs)
        '' Verarbeitet tickEvents
        ''
        '' wenn seq. dann schauen ob das zuletzt aktive child
        '' nicht mehr aktiv ist und das nächste starten

        If Not isParallel() Then
            '' Wenn das zuletzt gespielte nicht mehr aktiv ist
            '' das nächste starten falls möglich
            If Not lastPlaying.isPlaying AndAlso getChildItems().Count > getChildItems().IndexOf(lastPlaying) + 1 Then
                getChildItems().Item(getChildItems().IndexOf(lastPlaying) + 1).start()
            End If
        End If

        If Not isPlaying() Then
            abort()
        End If

    End Sub

    Public Overloads Sub start(Optional ByVal noWait As Boolean = True)
        MyBase.start(noWait)
        AddHandler getController.getTicker.frameTick, AddressOf receiveTick
    End Sub

    Public Overloads Sub abort()
        MyBase.abort()
        RemoveHandler getController.getTicker.frameTick, AddressOf receiveTick
    End Sub
End Class
