Imports System.Threading


Public Class ServerController

    Private cmdConnection As CasparCGConnection
    Private updateConnection As CasparCGConnection
    Private testConnection As CasparCGConnection
    Private updateThread As Thread


    Public Sub open()
        '' nur zum testen
        testConnection = New CasparCGConnection("localhost", 5250)
        'testConnection.connect()
    End Sub

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
        Return frames / (fps / 100)
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
