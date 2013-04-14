Imports System.Threading


Public Class ServerController

    Private cmdConnection As CasparCGConnection
    Private updateConnection As CasparCGConnection
    Private testConnection As CasparCGConnection
    Private updateThread As Thread
    Private tickThread As Thread
    Private serverAddress As String = "localhost"
    Private serverPort As Integer = 5250
    Private testChannel As Integer = 1
    Private opened As Boolean
    Private ticker As FrameTicker

    Public Sub open()
        open(serverAddress, serverPort)
    End Sub

    Public Sub close()
        logger.debug("Close servercontroller...")
        opened = False
        If Not IsNothing(updateThread) Then updateThread.Abort()
        If Not IsNothing(tickThread) Then tickThread.Abort()
        updateConnection.close()
        testConnection.close()
        cmdConnection.close()
    End Sub

    Public Function isOpen() As Boolean
        Return opened
    End Function

    Public Sub open(ByVal serverAddress As String, ByVal severPort As Integer)
        opened = True
        Me.serverAddress = serverAddress
        Me.serverPort = serverPort
        cmdConnection = New CasparCGConnection(serverAddress, serverPort)
        updateConnection = New CasparCGConnection(serverAddress, serverPort)
        testConnection = New CasparCGConnection(serverAddress, serverPort)

        ' Tick Thread starten
        ticker = New FrameTicker(updateConnection, Me, 1000)
        tickThread = New Thread(AddressOf ticker.tick)
        tickThread.Start()
    End Sub

    ''' <summary>
    ''' Returns the media duration in milliseconds if playing in native fps.
    ''' </summary>
    ''' <param name="media"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getOriginalMediaDuration(ByRef media As CasparCGMedia) As Long
        Select Case media.getMediaType
            Case CasparCGMedia.MediaType.COLOR, CasparCGMedia.MediaType.STILL, CasparCGMedia.MediaType.TEMPLATE
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
                logger.err("Could not get media duration of " & media.getFullName & "(" & media.getMediaType.ToString & ").")
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
            Case CasparCGMedia.MediaType.COLOR, CasparCGMedia.MediaType.STILL, CasparCGMedia.MediaType.TEMPLATE
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
                logger.err("Could not get media duration of " & media.getFullName & "(" & media.getMediaType.ToString & ").")
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
        Dim layer As Integer = 0
        While Not isLayerFree(layer, channel)
            layer = layer + 1
        End While
        Return layer
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
        Dim result = testConnection.sendCommand(CasparCGCommandFactory.getInfo(channel))
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
                        End If
                End Select
            End If
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Returns a Dictionary of all media and templates on the server key by their names.
    ''' If withMediaInfo is true, all mediaItems will have filled mediaInfo which is default by need more time.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getMedia(Optional ByVal withMediaInfo As Boolean = True) As Dictionary(Of String, CasparCGMedia)
        Dim media As New Dictionary(Of String, CasparCGMedia)
        '' Catch the media list and create the media objects
        Dim response = testConnection.sendCommand(CasparCGCommandFactory.getCls)
        If response.isOK Then
            For Each line As String In response.getData.Split(vbCrLf)
                line = line.Trim.Replace(vbCr, "").Replace(vbLf, "")
                If line <> "" AndAlso line.Split(" ").Length > 2 Then
                    Dim name = line.Substring(1, line.LastIndexOf("""") - 1)
                    line = line.Remove(0, line.LastIndexOf("""") + 1)
                    line = line.Trim().Replace("""", "").Replace("  ", " ")
                    Dim values() = line.Split(" ")
                    Select Case values(0)
                        Case "MOVIE"
                            media.Add(name, New CasparCGMovie(name))
                        Case "AUDIO"
                            media.Add(name, New CasparCGAudio(name))
                        Case "STILL"
                            media.Add(name, New CasparCGStill(name))
                    End Select
                End If
            Next
        End If

        '' Catch the template list and create the template objects
        response = testConnection.sendCommand(CasparCGCommandFactory.getTls)
        If response.isOK Then
            For Each line As String In response.getData.Split(vbCrLf)
                line = line.Trim.Replace(vbCr, "").Replace(vbLf, "")
                If line <> "" AndAlso line.Split(" ").Length > 2 Then
                    Dim name = line.Substring(1, line.LastIndexOf("""") - 1)
                    media.Add(name, New CasparCGTemplate(name))
                End If
            Next
        End If

        '' Add mediaInfo if requested
        If withMediaInfo Then
            For Each item In media.Values
                item.parseXML(getMediaInfo(item))
            Next
        End If

        Return media
    End Function

    Public Function getTicker() As FrameTicker
        Return ticker
    End Function

End Class

''' <summary>
''' A ticker class which raises events if a frame change is noticed in one of the casparCG channels. 
''' There is no waranty that a tick will be rissen for every framechange at all, but the frame number send should be close to the real one.
''' The period of ticks is close to "per frame" but will increase with every handler added for the frameTickEvent. 
''' Nevertheless, the precision of the frame number given by the event will not be affect much by increasing the eventHandler count.
''' During each poll request to the server for the current frame number, a short period of interpolated ticks will be rissen.
''' These are only calculated and not proofed to be in correct sync to the server. The length of this period could be configured.
''' Use bigger values if you have a poor network  conncetion. Default is 5 seconds (5000ms). It is possible that there will be no 
''' interpolated tick at all if the proccessing of the poll and raising the event takes longer than the given period.
''' In that case, you will only get received values form the server when they arrive.
''' It is very likley that the given frame number is behind the frame number at the servers channel ba a few frames.
''' Since this delay may differ depending on your networkconnection and hardware, it should be, more or less, constant.
''' You may meassure it and give a correction value as timeOffset in milliseconds. This will be staticaly added to the received/calculated frame number
'''  
''' Start tick() in a new Thread and register handlers for the frameTick event. 
''' Keep in mind to use delegates since the event will likely to be rissen by a different thread.
''' </summary>
''' <remarks></remarks>
Public Class FrameTicker

    Private sc As ServerController
    Private con As CasparCGConnection
    Private interpolationTime As Integer
    Private timeOffset As Integer
    Public Event frameTick(ByVal sender As Object, ByVal e As frameTickEventArgs)

    ''' <summary>
    ''' Creates a new ticker instance.
    ''' </summary>
    ''' <param name="con">the connection to poll the channels for the actual frame number</param>
    ''' <param name="controller">the servercontroler</param>
    ''' <param name="interpolationTime">the number of milliseconds between each servercall. In that time, the frame tick will be interpolated by a local timer which may differ from the servers real values.</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef con As CasparCGConnection, ByVal controller As ServerController, Optional ByVal interpolationTime As Integer = 5000, Optional ByVal timeOffset As Integer = 0)
        sc = controller
        Me.con = con
        Me.interpolationTime = interpolationTime
        Me.timeOffset = timeOffset
        logger.debug("Ticker init by thread " & Thread.CurrentThread.ManagedThreadId)
    End Sub

    Public Sub tick()
        logger.debug("Ticker thread " & Thread.CurrentThread.ManagedThreadId & " started")
        Dim timer As New Stopwatch() ' Timer to measure the time it takes to calc current frame / channel and inform listeners
        Dim offsetTimer As New Stopwatch
        Dim iterationTime As Long
        Dim interpolatingSince As Integer
        Dim channelFameTime() As Integer
        Dim minFrameTime As Integer = Integer.MaxValue
        Dim channelFrame As New Dictionary(Of Integer, Long)
        Dim channels As Integer = con.sendCommand(CasparCGCommandFactory.getInfo()).getData.Split(vbNewLine).Length 'Anzahl der Channels bestimmen

        ReDim channelFameTime(channels)
        For i As Integer = 1 To channels
            channelFameTime(i - 1) = 1000 / (sc.getFPS(i) / 100)
            If minFrameTime > channelFameTime(i - 1) - 1 Then minFrameTime = channelFameTime(i - 1) - 1
            channelFrame.Add(i, 0)
        Next


        Dim infoDoc As New MSXML2.DOMDocument
        Dim frame As Long = 0
        Dim ch = 0
        timer.Start()
        offsetTimer.Start()
        While True
            ' Alle channels durchgehen
            For channel As Integer = 1 To channels
                ch = channel - 1
                'werte einlesen
                offsetTimer.Restart()
                infoDoc.loadXML(con.sendCommand(CasparCGCommandFactory.getInfo(channel, 9999)).getXMLData)
                frame = infoDoc.firstChild.selectSingleNode("frame-number").nodeTypedValue + (offsetTimer.ElapsedMilliseconds / 2 / channelFameTime(ch))
                offsetTimer.Stop()

                ' Korrigierten Wert für die Frame number berechen aus dem rückgabewert des servers + der frames die in der bearbeitungszeit
                ' vermeintlich vergangen sind. Wir gehen vereinfacht davon aus, das die hälfte der Zeit für den Rückweg 
                'vom Server zu uns gebaucht wurde da wir das nicht genau messen können.
                If frame <> channelFrame.Item(channel) Then
                    channelFrame.Item(channel) = frame
                End If
            Next
            '' Event auslösen
            RaiseEvent frameTick(Me, New frameTickEventArgs(channelFrame))
            ' Jetzt ein paar frames nur rechnen und dann wieder vergleichen
            Dim hasChanged = False
            interpolatingSince = timer.ElapsedMilliseconds
            While interpolatingSince < interpolationTime

                '' PROBLEM: Wenn die IterationTime < FrameTime ist wird der channel nie hochgezählt
                '' Globaler timer über alle Iterationen?????


                iterationTime = timer.ElapsedMilliseconds
                timer.Restart()
                For channel As Integer = 1 To channels
                    ch = channel - 1

                    '' TODO Umbauen so das nicht einfach ein Tick gemacht wird, sondern anhand des Timerobjekts die genaue anzahl an frames
                    '' berechnet und aufaddiert wird.
                    If iterationTime > 0 AndAlso iterationTime / channelFameTime(ch) > 0 Then
                        hasChanged = True
                        channelFrame.Item(channel) = channelFrame.Item(channel) + (iterationTime / channelFameTime(ch))
                    End If
                Next
                '' Event auslösen
                If hasChanged Then RaiseEvent frameTick(Me, New frameTickEventArgs(channelFrame))
                hasChanged = False
                If timer.ElapsedMilliseconds < minFrameTime Then
                    'logger.debug("Pausing...")
                    Thread.Sleep(1 + (minFrameTime - timer.ElapsedMilliseconds))
                End If
                interpolatingSince = interpolatingSince + timer.ElapsedMilliseconds
            End While
            timer.Restart()
        End While

    End Sub
End Class

''' <summary>
''' The frameTick event result containing the channel which ticked and the actual framenumber.
''' </summary>
''' <remarks></remarks>
Public Class frameTickEventArgs
    Inherits EventArgs

    Public Sub New(ByVal result As Dictionary(Of Integer, Long))
        _result = result
    End Sub

    Private _result As Dictionary(Of Integer, Long)
    Public ReadOnly Property result
        Get
            Return _result
        End Get
    End Property
End Class

