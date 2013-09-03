'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports CasparCGNETConnector
Imports logger

''' <summary>
''' Represents the media library of the casparCG Server.
''' Allows to get items of given type, all items or filterd matches.
''' </summary>
''' <remarks></remarks>
Public Class Library

    Private media As Dictionary(Of String, AbstractCasparCGMedia)
    Private controller As ServerController
    Private updateThread As Threading.Thread

    Public Event updated(ByRef sender As Object, ByRef media As Dictionary(Of String, AbstractCasparCGMedia))
    Public Event updatedAborted(ByRef sender As Object)


    Public Sub New(ByVal controller As ServerController)
        Me.controller = controller
        media = New Dictionary(Of String, AbstractCasparCGMedia)
    End Sub

    Public Sub addItem(ByRef item As AbstractCasparCGMedia)
        If Not media.ContainsKey(item.getFullName & "(" & item.getMediaType.ToString & ")") Then media.Add(item.getFullName & "(" & item.getMediaType.ToString & ")", item)
    End Sub

    Public Sub updateItem(ByRef item As AbstractCasparCGMedia)
        If media.ContainsKey(item.getFullName & "(" & item.getMediaType.ToString & ")") Then
            'ToDo: Update the media in the list
        Else
            addItem(item)
        End If
    End Sub

    Public Function getItems() As IEnumerable(Of AbstractCasparCGMedia)
        Return media.Values
    End Function

    Public Function getItemsOfType(ByVal type As AbstractCasparCGMedia.MediaType) As IEnumerable(Of AbstractCasparCGMedia)
        Dim items As New List(Of AbstractCasparCGMedia)
        For Each item In media.Values
            If item.getMediaType = type Then
                items.Add(item)
            End If
        Next
        Return items
    End Function

    Public Function getItem(ByVal name As String) As AbstractCasparCGMedia
        For Each med In media.Values
            If med.getFullName.Equals(name) OrElse med.getName.Equals(name) Then Return med
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Rereads the Server List of Mediafiles and refreshs the Library.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub refreshLibrary()
        If controller.isConnected Then
            updateThread = New Threading.Thread(AddressOf update)
            updateThread.Start()
        Else
            RaiseEvent updated(Me, New Dictionary(Of String, AbstractCasparCGMedia))
        End If
    End Sub

    Public Function isUpdating() As Boolean
        Return Not IsNothing(updateThread) AndAlso updateThread.IsAlive
    End Function

    Public Sub abortUpdate()
        If isUpdating() Then
            updateThread.Abort()
            RaiseEvent updatedAborted(Me)
        End If
    End Sub

    Private Sub update()
        RaiseEvent updated(Me, controller.getMediaList)
    End Sub

    Public Sub updateLibrary(ByRef sender As Object, ByRef media As Dictionary(Of String, AbstractCasparCGMedia)) Handles Me.updated
        Me.media = media
        updateThread = Nothing
    End Sub
End Class
