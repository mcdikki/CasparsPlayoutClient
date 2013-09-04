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
    Private _abortUpdate As Boolean = False

    Public Event itemUpdated(ByRef item As AbstractCasparCGMedia)
    Public Event updated(ByRef sender As Object)
    Public Event updatedAborted(ByRef sender As Object)


    Public Sub New(ByVal controller As ServerController)
        Me.controller = controller
        media = New Dictionary(Of String, AbstractCasparCGMedia)
    End Sub

    Public Sub clear()
        media.Clear()
        RaiseEvent updated(Me)
    End Sub

    Public Sub addItem(ByRef item As AbstractCasparCGMedia)
        If Not media.ContainsKey(item.getUuid) Then media.Add(item.getUuid, item)
    End Sub

    Public Sub updateItem(ByRef item As AbstractCasparCGMedia) Handles Me.itemUpdated
        If media.ContainsKey(item.getUuid) Then
            Dim dest As AbstractCasparCGMedia = media.Item(item.getUuid)
            ' update infos
            If item.getInfos.Count > 0 Then
                dest.setInfos(item.getInfos)
            End If

            ' update thumbnail
            If item.getBase64Thumb.Length > 0 Then dest.setBase64Thumb(item.getBase64Thumb)
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
            RaiseEvent updatedAborted(Me)
        End If
    End Sub

    Public Function isUpdating() As Boolean
        Return Not IsNothing(updateThread)
    End Function

    Public Sub abortUpdate()
        If isUpdating() Then
            'updateThread.Abort()
            'RaiseEvent updatedAborted(Me)
            _abortUpdate = True
        End If
    End Sub

    Private Sub updateAborted(ByRef sender As Object) Handles Me.updatedAborted, Me.updated
        If Not IsNothing(updateThread) Then
            updateThread = Nothing
            _abortUpdate = False
        End If
    End Sub

    Private Sub update()

        ' Get a list of known media
        Dim mediaItems As New List(Of AbstractCasparCGMedia)
        Dim cmd As ICommand
        Dim newMedia As AbstractCasparCGMedia = Nothing

        ' Templates first
        '' Catch the template list and create the template objects
        cmd = New TlsCommand()
        If cmd.execute(controller.getTestConnection).isOK Then
            For Each line As String In cmd.getResponse.getData.Split(vbCrLf)
                Application.DoEvents()
                If _abortUpdate Then
                    RaiseEvent updatedAborted(Me)
                    Exit Sub
                End If
                line = line.Trim.Replace(vbCr, "").Replace(vbLf, "")
                If line <> "" AndAlso line.Split(" ").Length > 2 Then
                    Dim name = line.Substring(1, line.LastIndexOf("""") - 1).ToUpper
                    newMedia = New CasparCGTemplate(name)
                    'newMedia.fillMediaInfo(controller.getTestConnection, controller.getTestChannel)
                    mediaItems.Add(newMedia)
                    RaiseEvent itemUpdated(newMedia)
                End If
            Next
        End If

        If _abortUpdate Then
            RaiseEvent updatedAborted(Me)
            Exit Sub
        End If

        '' Catch the media list and create the media objects
        If controller.isConnected Then
            cmd = New ClsCommand()
            If cmd.execute(controller.getTestConnection).isOK Then
                For Each line As String In cmd.getResponse.getData.Split(vbCrLf)
                    Application.DoEvents()
                    If _abortUpdate Then
                        RaiseEvent updatedAborted(Me)
                        Exit Sub
                    End If
                    line = line.Trim()
                    If line <> "" AndAlso line.Split(" ").Length > 2 Then
                        ' removed .toUpper for the name as the server is allways using uppercases and on some special chars like µ this leads to an error.
                        Dim name = line.Substring(1, line.LastIndexOf("""") - 1)
                        line = line.Remove(0, line.LastIndexOf("""") + 1)
                        line = line.Trim().Replace("""", "").Replace("  ", " ")
                        Dim values() = line.Split(" ")
                        Select Case values(0)
                            Case "MOVIE"
                                newMedia = New CasparCGMovie(name)
                            Case "AUDIO"
                                newMedia = New CasparCGAudio(name)
                            Case "STILL"
                                newMedia = New CasparCGStill(name)
                        End Select
                        If Not IsNothing(newMedia) Then
                            mediaItems.Add(newMedia)
                            RaiseEvent itemUpdated(newMedia)
                        End If
                    End If
                Next
            End If
        End If


        ' retrieve info for each media and update this media
        For Each item In mediaItems
            Application.DoEvents()
            If _abortUpdate Then
                RaiseEvent updatedAborted(Me)
                Exit Sub
            End If

            item.fillMediaInfo(controller.getTestConnection)
            RaiseEvent itemUpdated(item)
        Next

        ' retrive thumbnails for each media
        For Each item In mediaItems
            Application.DoEvents()
            If _abortUpdate Then
                RaiseEvent updatedAborted(Me)
                Exit Sub
            End If

            If item.getMediaType = AbstractCasparCGMedia.MediaType.MOVIE OrElse item.getMediaType = AbstractCasparCGMedia.MediaType.STILL Then
                item.fillThumbnail(controller.getTestConnection)
                RaiseEvent itemUpdated(item)
            End If
        Next
        RaiseEvent updated(Me)
    End Sub
End Class
