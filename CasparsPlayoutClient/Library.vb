''' <summary>
''' Represents the media library of the casparCG Server.
''' Allows to get items of given type, all items or filterd matches.
''' </summary>
''' <remarks></remarks>
Public Class Library

    Public Function getLibraryItems() As List(Of LibraryItem)
        Return Nothing
    End Function

    'Public Function getLibraryItems(ByVal ItemType As Library.ItemTypes) As List(Of LibraryItem)
    '    Return Nothing
    'End Function

    Public Function getMovieItems() As List(Of LibraryItem)
        Return Nothing
    End Function

    Public Function getStillItems() As List(Of LibraryItem)
        Return Nothing
    End Function

    Public Function getAudioItems() As List(Of LibraryItem)
        Return Nothing
    End Function

    Public Function getTemplateItems() As List(Of LibraryItem)
        Return Nothing
    End Function

    ''' <summary>
    ''' Rereads the Server List of Mediafiles and refreshs the Library.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub refreshLibrary()
        '' todo
    End Sub
End Class
