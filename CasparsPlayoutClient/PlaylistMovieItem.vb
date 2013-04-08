Public Class PlaylistMovieItem
    Inherits PlaylistItem

    Property file As String
    Property path As String
    '' eigentlich nur infos und nicht zu bearbeiten (vom User) -- Wie sperren?
    Property fps As Integer
    Property width As Integer
    Property height As Integer
    Property isInterlaced As Boolean

    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByVal duration As Long)
        MyBase.New(name, PlaylistItemTypes.MOVIE, controller, duration)
    End Sub


    '' Methoden die überschrieben werden müssen weil sie andere oder mehr functionen haben
    ''-------------------------------------------------------------------------------------
    Public Overloads Sub start(Optional ByVal noWait As Boolean = True)
        '' CMD an ServerController schicken
    End Sub

    Public Overloads Sub abort()
        '' CMD an ServerController schicken

        '' Der rest wird von der Elternklasse erledigt
        MyBase.abort()
    End Sub

    Public Overloads Sub pause(ByVal frames As Long)
        '' cmd an ServerController schicken

        '' pause wird über die Elternklasse realisiert
        MyBase.pause(frames)
    End Sub

    Public Overloads Sub unPause()
        '' cms an ServerController schicken

        '' Aufheben der Pause wird über die Elternklasse realisiert
        MyBase.unPause()
    End Sub



    '' Methoden die Überschrieben werden müssen weil sie leer sind
    ''-------------------------------------------------------------
    Public Overloads Sub addItem(ByVal item As IPlaylistItem)
        '' Nothing to do. Movie Items dosen't have child items
    End Sub

    Public Overloads Sub setChildItems(ByRef items As List(Of IPlaylistItem))
        '' Nothing to do. Movie Items dosen't have child items
    End Sub
End Class
