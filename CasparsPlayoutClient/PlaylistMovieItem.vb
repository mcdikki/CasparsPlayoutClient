Public Class PlaylistMovieItem
    Inherits PlaylistItem

    Private media As CasparCGMovie

    ''' <summary>
    ''' Create a PlaylistMovieItem. If a duration is given and smaller than the original duration of the file, the file will only be played for that long.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="controller"></param>
    ''' <param name="movie"></param>
    ''' <param name="duration"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByRef movie As CasparCGMovie, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.MOVIE, controller, duration)
        If duration > Long.Parse(movie.getInfo("nb-frames")) OrElse duration = -1 Then
            setDuration(Long.Parse(movie.getInfo("nb-frames")))
        End If
        media = movie
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
