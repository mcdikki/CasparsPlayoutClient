Imports System.Threading


Public Class ServerController

    Private cmdConnection As CasparCGConnection
    Private updateConnection As CasparCGConnection
    Private testConnection As CasparCGConnection
    Private updateThread As Thread
    Private serverAddress As String = "localhost"
    Private serverPort As Integer = 5250
    Private testChannel As Integer = 1


    Public Sub open()
        open(serverAddress, serverPort)
    End Sub

    Public Sub open(ByVal serverAddress As String, ByVal severPort As Integer)
        Me.serverAddress = serverAddress
        Me.serverPort = serverPort
        cmdConnection = New CasparCGConnection(serverAddress, serverPort)
        updateConnection = New CasparCGConnection(serverAddress, serverPort)
        testConnection = New CasparCGConnection(serverAddress, serverPort)
    End Sub

    ''' <summary>
    ''' Returns the media duration in milliseconds if playing in native fps.
    ''' </summary>
    ''' <param name="media"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getOriginalMediaDuration(ByRef media As CasparCGMedia) As Long
        Select Case media.getMediaType
            Case CasparCGMedia.MediaType.COLOR, CasparCGMedia.MediaType.STILL, CasparCGMedia.MediaType.TEMPLATE, CasparCGMedia.MediaType.AUDIO
                '' These mediatyps doesn't have any durations
                Return 0
            Case Else
                If media.getInfos.Count = 0 Then
                    '' no media info is loaded
                    '' load it now
                    media.parseXML(getMediaInfo(media))
                End If
                If media.containsInfo("nb-frames") AndAlso media.containsInfo("fps") AndAlso media.containsInfo("progressive") Then
                    Dim fps As Integer = Integer.Parse(media.getInfo("fps"))
                    If Not Boolean.Parse(media.getInfo("progressive")) Then
                        fps = fps / 2
                    End If
                    Return getTimeInMS(media.getInfo("nb-frames"), fps)
                End If
                logger.err("Could not get media duration of " & media.getFullName & ".")
                Return 0
        End Select
    End Function

    ''' <summary>
    ''' Returns the media duration in milliseconds if playing at a given channel.
    ''' </summary>
    ''' <param name="media"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getMediaDuration(ByRef media As CasparCGMedia, ByVal channel As Integer) As Long
        Select Case media.getMediaType
            Case CasparCGMedia.MediaType.COLOR, CasparCGMedia.MediaType.STILL, CasparCGMedia.MediaType.TEMPLATE, CasparCGMedia.MediaType.AUDIO
                '' These mediatyps doesn't have any durations
                Return 0
            Case Else
                If media.getInfos.Count = 0 Then
                    '' no media info is loaded
                    '' load it now
                    media.parseXML(getMediaInfo(media))
                End If
                If media.containsInfo("nb-frames") Then
                    Return getTimeInMS(media.getInfo("nb-frames"), getFPS(channel))
                End If
                logger.err("Could not get media duration of " & media.getFullName & ".")
                Return 0
        End Select
    End Function

    Public Function getMediaInfo(ByRef media As CasparCGMedia) As String
        If media.getMediaType = CasparCGMedia.MediaType.TEMPLATE Then
            Dim response = testConnection.sendCommand(CasparCGCommandFactory.getInfo(media))
            If response.isOK Then
                Return response.getXMLData
            End If
        Else
            Dim layer = getFreeLayer(testChannel)
            Dim response = testConnection.sendCommand(CasparCGCommandFactory.getLoadbg(testChannel, layer, media.getFullName))
            If response.isOK Then
                Dim configDoc As New MSXML2.DOMDocument
                response = testConnection.sendCommand(CasparCGCommandFactory.getInfo(testChannel, layer, True))
                testConnection.sendAsyncCommand(CasparCGCommandFactory.getCGClear(testChannel, layer))
                configDoc.loadXML(response.getXMLData())
                If configDoc.hasChildNodes Then
                    Return configDoc.selectSingleNode("producer").selectSingleNode("destination").selectSingleNode("producer").xml
                End If
            End If
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Returns the smallest free layer of the given channel
    ''' </summary>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getFreeLayer(ByVal channel As Integer) As Integer
        Return 0
    End Function

    ''' <summary>
    ''' Returns whether or not a layer of a channel is free, which means no producer is playing on it.
    ''' </summary>
    ''' <param name="layer"></param>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isLayerFree(ByVal layer As Integer, ByVal channel As Integer) As Boolean
        Dim answer = testConnection.sendCommand("INFO " & channel & "-" & layer)
        Dim doc As New MSXML2.DOMDocument()
        If answer.isOK AndAlso doc.loadXML(answer.getXMLData) Then
            For Each type As MSXML2.IXMLDOMNode In doc.getElementsByTagName("type")
                If Not type.nodeTypedValue.Equals("empty-producer") Then
                    Return False
                End If
            Next
            Return True
        End If
        If Not IsNothing(doc.parseError) Then
            logger.warn("Error checking layer." & vbNewLine & doc.parseError.reason & vbNewLine & doc.parseError.line & ":" & doc.parseError.linepos & vbNewLine & doc.parseError.srcText)
        Else
            logger.warn("Could not check layer. Server response was incorrect.")
        End If
        Return False
    End Function

    ''' <summary>
    ''' Returns the time in milliseconds needed to play the given number of frames at a specified framerate.
    ''' </summary>
    ''' <param name="frames">the number of frames</param>
    ''' <param name="fps">the framerate multiplied by 100 to avoid floating numbers like 59.94.</param> 
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getTimeInMS(ByVal frames As Long, ByVal fps As Integer) As Long
        Return (frames * 1000) / (fps / 100)
    End Function

    ''' <summary>
    ''' Returns the framerate of the spezified channel or 0 if the channel does not exist.
    ''' </summary>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getFPS(ByVal channel As Integer) As Integer
        Dim result = testConnection.sendCommand("info " & channel)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(result.getXMLData())
        If configDoc.hasChildNodes Then
            If Not IsNothing(configDoc.selectSingleNode("channel")) AndAlso Not IsNothing(configDoc.selectSingleNode("channel").selectSingleNode("video-mode")) Then
                Select Case configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue
                    Case "PAL"
                        Return 2500
                    Case "NTSC"
                        Return 2994
                    Case Else
                        If configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Contains("i") Then
                            Return Integer.Parse(configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Substring(configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.IndexOf("i"))) / 2
                        ElseIf configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Contains("p") Then
                            Return Integer.Parse(configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.Substring(configDoc.selectSingleNode("channel").selectSingleNode("video-mode").nodeTypedValue.IndexOf("p") + 1))
                        Else
                            Return 0
                        End If
                End Select
            End If
        End If
        Return 0
    End Function

End Class
