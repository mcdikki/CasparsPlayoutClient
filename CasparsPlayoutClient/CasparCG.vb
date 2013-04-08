Imports System.Net
Imports System.Net.Sockets

Public Class CasparCGConnection
    Private serveraddress As String = "localhost"
    Private serverport As Integer = 5250 ' std. acmp2 port
    Private client As TcpClient
    Private reconnectTries = 10
    Private connectionAttemp = 0
    Private reconnectTimeout = 1000 ' 1sec
    Private tryConnect As Boolean = True

    Public Sub New(ByVal serverAddress As String, ByVal serverPort As Integer)
        Me.serveraddress = serverAddress
        Me.serverport = serverPort
        client = New TcpClient()
        client.NoDelay = True
    End Sub

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    Public Function connect() As Boolean
        If Not client.Connected Then
            Try
                client.Connect(serveraddress, serverport)
                client.NoDelay = True
                If client.Connected Then
                    connectionAttemp = 0
                    logger.log("Connected to " & serveraddress & ":" & serverport.ToString)
                    Return True
                End If
            Catch e As Exception
                logger.warn(e.Message)
                If connectionAttemp < reconnectTries Then
                    connectionAttemp = connectionAttemp + 1
                    logger.warn("Try to reconnect " & connectionAttemp & "/" & reconnectTries)
                    Dim i As Integer = 0
                    Threading.Thread.Sleep(reconnectTimeout)
                    Return connect()
                Else
                    logger.err("Could not connect to " & serveraddress & ":" & serverport.ToString)
                    Return False
                End If
            End Try
        Else
            logger.log("Allready connected to " & serveraddress & ":" & serverport.ToString)
        End If
        Return client.Connected
    End Function

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    ''' <param name="serverAddress">the server ip or hostname</param>
    ''' <param name="serverPort">the servers port</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function connect(ByVal serverAddress As String, ByVal serverPort As Integer) As Boolean
        Me.serveraddress = serverAddress
        Me.serverport = serverPort
        Return connect()
    End Function

    ''' <summary>
    ''' Return whether or not the CasparCGConnection is connect to the server. If tryConnect is given and true, it will try to establish a connection if not allready connected.
    ''' </summary>
    ''' <param name="tryConnect"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function connected(Optional ByVal tryConnect As Boolean = False) As Boolean
        If client.Connected Then
            Return True
        Else
            If tryConnect Then
                connect()
            End If
            Return client.Connected
        End If
    End Function

    Public Sub close()
        If client.Connected Then
            client.Client.Close()
        End If
    End Sub

    ''' <summary>
    ''' Sends a command to the casparCG server and returns imediatly after sending no matter if the command was accepted or not.
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <remarks></remarks>
    Public Sub sendAsyncCommand(ByVal cmd As String)
        If connected(tryConnect) Then
            logger.debug("Send command: " & cmd)
            client.GetStream.Write(System.Text.UTF8Encoding.UTF8.GetBytes(cmd & vbCrLf), 0, cmd.Length + 2)
            logger.debug("Command sent")
        Else : logger.err("Not connected to server. Can't send command.")
        End If
    End Sub

    ''' <summary>
    ''' Sends a command to the casparCG server and returns a CasparCGResonse.
    ''' sendCommand will wait until it receives a returncode. So it may stay longer inside the function.
    ''' If the given commandstring has more than one casparCG command, the response will be only for one of those!
    ''' </summary>
    ''' <param name="cmd"></param>
    Public Function sendCommand(ByVal cmd As String) As CasparCGResponse
        If connected(tryConnect) Then
            Dim isOk As Boolean = False
            Dim buffer() As Byte
            If client.Available > 0 Then
                ReDim buffer(client.Available)
                ' den alten Buffer erstmal auslesen, ihn brauchen wir nicht
                client.GetStream.Read(buffer, 0, client.Available)
            End If
            ' Befehl senden
            logger.debug("Send command: " & cmd)
            client.GetStream.Write(System.Text.UTF8Encoding.UTF8.GetBytes(cmd & vbCrLf), 0, cmd.Length + 2)
            logger.debug("Command sent")

            ' Auf antwort warten
            Dim i As Integer = 0
            While client.Available < 3
                Threading.Thread.Sleep(2)
                i = i + 2
            End While
            logger.debug("Waited " & i & "ms for an answer")
            ReDim buffer(client.Available)
            client.GetStream.Read(buffer, 0, client.Available)
            Return New CasparCGResponse(Trim(System.Text.UTF8Encoding.UTF8.GetString(buffer)))
        Else
            logger.err("Not connected to server. Can't send command.")
            Return Nothing
        End If
    End Function

End Class

Public Class CasparCGCommandFactory

    Public Shared Function getLoadbg(ByVal channel As Integer, ByVal layer As Integer, ByVal media As String, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
        Dim cmd As String = "LOADBG " & getDestination(channel, layer) & " " & media

        If looping Then
            cmd = cmd & " LOOP"
        End If
        If Not IsNothing(transition) Then
            cmd = cmd & " " & transition.toString
        End If
        If seek > 0 Then
            cmd = cmd & " SEEK " & seek
        End If
        If length > 0 Then
            cmd = cmd & " LENGTH " & length
        End If
        If filter.Length > 0 Then
            cmd = cmd & " FILTER " & filter
        End If

        Return escape(cmd)
    End Function

    Public Shared Function getLoad(ByVal channel As Integer, ByVal layer As Integer, ByVal media As String, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
        Dim cmd As String = "LOAD " & getDestination(channel, layer) & " " & media

        If looping Then
            cmd = cmd & " LOOP"
        End If
        If Not IsNothing(transition) Then
            cmd = cmd & " " & transition.toString
        End If
        If seek > 0 Then
            cmd = cmd & " SEEK " & seek
        End If
        If length > 0 Then
            cmd = cmd & " LENGTH " & length
        End If
        If filter.Length > 0 Then
            cmd = cmd & " FILTER " & filter
        End If

        Return escape(cmd)
    End Function

    Public Shared Function getPlay(ByVal channel As Integer, ByVal layer As Integer, Optional ByVal media As String = "", Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
        Dim cmd As String = "PLAY " & getDestination(channel, layer)

        If media.Length > 0 Then
            cmd = cmd & " " & media
        End If
        If looping Then
            cmd = cmd & " LOOP"
        End If
        If Not IsNothing(transition) Then
            cmd = cmd & " " & transition.toString
        End If
        If seek > 0 Then
            cmd = cmd & " SEEK " & seek
        End If
        If length > 0 Then
            cmd = cmd & " LENGTH " & length
        End If
        If filter.Length > 0 Then
            cmd = cmd & " FILTER " & filter
        End If

        Return escape(cmd)
    End Function

    Public Shared Function getCall(ByVal channel As Integer, ByVal layer As Integer, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
        Dim cmd As String = "CALL " & getDestination(channel, layer)

        If looping Then
            cmd = cmd & " LOOP"
        End If
        If Not IsNothing(transition) Then
            cmd = cmd & " " & transition.toString
        End If
        If seek > 0 Then
            cmd = cmd & " SEEK " & seek
        End If
        If length > 0 Then
            cmd = cmd & " LENGTH " & length
        End If
        If filter.Length > 0 Then
            cmd = cmd & " FILTER " & filter
        End If

        Return cmd
    End Function

    Public Shared Function getSwap(ByVal channelA As Integer, ByVal channelB As Integer) As String
        Return "SWAP " & channelA & " " & channelB
    End Function

    Public Shared Function getSwap(ByVal channelA As Integer, ByVal channelB As Integer, ByVal layerA As Integer, ByVal layerB As Integer) As String
        Return "SWAP " & channelA & "-" & layerA & " " & channelB & "-" & layerB
    End Function

    Public Shared Function getStop(ByVal channel As Integer, ByVal layer As Integer) As String
        Return "STOP " & getDestination(channel, layer)
    End Function

    Public Shared Function getPause(ByVal channel As Integer, ByVal layer As Integer) As String
        Return "PAUSE " & getDestination(channel, layer)
    End Function

    Public Shared Function getClear(Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1) As String
        Dim cmd As String = "CLEAR"
        If channel > -1 Then
            cmd = cmd & getDestination(channel, layer)
        End If
        Return cmd
    End Function

    Public Shared Function getInfo(Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal onlyBackground As Boolean = False, Optional ByVal onlyForeground As Boolean = False) As String
        Dim cmd As String = "INFO"
        If channel > -1 Then
            cmd = cmd & " " & getDestination(channel, layer)
            If layer > -1 Then
                If onlyBackground Then
                    cmd = cmd & " B"
                ElseIf onlyForeground Then
                    cmd = cmd & " F"
                End If
            End If
        End If
        Return cmd
    End Function

    Public Shared Function getInfo(ByRef template As CasparCGTemplate) As String 
        Return escape("INFO TEMPLATE '" & template.getFullName & "'")
    End Function


    '' CG CMD für Flashtemplates
    Public Shared Function getCGAdd(ByVal channel As Integer, ByVal layer As Integer, ByVal template As CasparCGTemplate, ByVal flashlayer As Integer, Optional ByVal playOnLoad As Boolean = False) As String
        Dim cmd As String = "CG " & getDestination(channel, layer) & " ADD " & flashlayer & " " & template.getFullName

        If playOnLoad Then
            cmd = cmd & " 1 "
        Else
            cmd = cmd & " 0 "
        End If
        cmd = cmd & template.getData.toXML
        Return escape(cmd)
    End Function

    Public Shared Function getCGRemove(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
        Return "CG " & getDestination(channel, layer) & " REMOVE " & flashlayer
    End Function

    Public Shared Function getCGPlay(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
        Return "CG " & getDestination(channel, layer) & " PLAY " & flashlayer
    End Function

    Public Shared Function getCGStop(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
        Return "CG " & getDestination(channel, layer) & " STOP " & flashlayer
    End Function

    Public Shared Function getCGNext(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
        Return "CG " & getDestination(channel, layer) & " NEXT " & flashlayer
    End Function

    Public Shared Function getCGUpdate(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer, ByRef data As CasparCGTemplateData) As String
        Return "CG " & getDestination(channel, layer) & " UPDATE " & flashlayer & " " & escape(data.toXML)
    End Function

    Public Shared Function getCGInvoke(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer, ByVal method As String) As String
        Return "CG " & getDestination(channel, layer) & " INVOKE " & flashlayer & " " & method
    End Function

    Public Shared Function getCGClear(ByVal channel As Integer, ByVal layer As Integer) As String
        Return "CG " & getDestination(channel, layer) & " CLEAR"
    End Function

    Private Shared Function getDestination(ByVal channel As Integer, ByVal layer As Integer) As String
        Dim cmd As String
        If channel > -1 Then
            cmd = channel
        Else
            cmd = "0"
        End If
        If layer > -1 Then
            cmd = cmd & "-" & layer
        End If
        Return cmd
    End Function


    ''' <summary>
    ''' Escapes the string str as needed for casparCG Server
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function escape(ByVal str As String) As String
        ' Backslash
        str = str.Replace("\", "\\")

        ' Hochkommata
        str = str.Replace("'", "\'")
        str = str.Replace("""", "\""")
        Return str
    End Function

End Class

Public Class CasparCGResponse

    Private returncode As CasparReturnCode
    Private command As String
    Private data As String

    Public Enum CasparReturnCode
        UNKNOWN_RETURNCODE = 0  ' This code is not known
        INFO = 100              ' 100 [action] - Information about an event.
        INFO_DATA = 101         ' 101[action] - Information about an event. A line of data is being returned. 
        OK_MULTI_DATA = 200     ' 200 [command] OK	- The command has been executed and several lines of data are being returned (terminated by an empty line).
        OK_DATA = 201           ' 201 [command] OK	- The command has been executed and a line of data is being returned
        OK = 202                ' 202 [command] OK	- The command has been executed 
        ERR_CMD_UNKNOWN = 400   ' 400 ERROR	- Command not understood
        ERR_CHANNEL = 401       ' 401 [command] ERROR	- Illegal video_channel
        ERR_PARAMETER_MISSING = 402 ' 402 [command] ERROR	- Parameter missing
        ERR_PARAMETER = 403     ' 403 [command] ERROR	- Illegal parameter
        ERR_MEDIA_UNKNOWN = 404 ' 404 [command] ERROR	- Media file not found
        ERR_SERVER = 500        ' 500 FAILED	- Internal server error
        ERR_SERVER_DATA = 501   ' 501 [command] FAILED	- Internal server error
        ERR_FILE_UNREADABLE = 502 ' 502 [command] FAILED	- Media file unreadable 
    End Enum

    Public Sub New(ByVal returnmessage As String)
        Me.returncode = parseReturnCode(returnmessage)
        Me.command = parseReturnCommand(returnmessage)
        Me.data = parseReturnData(returnmessage)
    End Sub

    Public Sub New(ByVal returnCode As CasparReturnCode, ByVal command As String, ByVal data As String)
        Me.returncode = returnCode
        Me.command = command
        Me.data = data
    End Sub

    Public Shared Function parseReturnCode(ByVal returnmessage As String) As CasparReturnCode
        If Not IsNothing(returnmessage) Then
            returnmessage = Trim(returnmessage)
            If returnmessage.Length > 2 AndAlso IsNumeric(returnmessage.Substring(0, 3)) Then
                Dim returncode As Integer = Integer.Parse(returnmessage.Substring(0, 3))
                If [Enum].IsDefined(GetType(CasparReturnCode), returncode) Then
                    Return returncode
                End If
            End If
        End If
        Return 0
    End Function

    Public Shared Function parseReturnCommand(ByVal returnmessage As String) As String
        If Not IsNothing(returnmessage) AndAlso returnmessage.Length > 0 Then
            returnmessage = Trim(returnmessage).Substring(4) ' Code wegschneiden
            Return returnmessage.Substring(0, returnmessage.IndexOf(" "))
        End If
        Return ""
    End Function

    Public Shared Function parseReturnData(ByVal returnmessage As String) As String
        If Not IsNothing(returnmessage) AndAlso returnmessage.Length > 0 Then
            returnmessage = Trim(returnmessage).Substring(4) ' Code wegschneiden
            Return returnmessage.Substring(returnmessage.IndexOf(vbCr) + 1)
        End If
        Return ""
    End Function

    Public Function getCode() As CasparReturnCode
        Return returncode
    End Function

    Public Function getCommand() As String
        Return command
    End Function

    Public Function getData() As String
        Return data
    End Function

    Public Function getXMLData() As String
        If Not IsNothing(data) Then
            Dim xml As String = ""
            If data.Contains("<?") And data.Contains("?>") Then
                xml = data
            End If
            While Xml.Contains("<?") And Xml.Contains("?>")
                Dim start As Integer = Xml.IndexOf("<")
                While Not Xml.Substring(start, 2) = "<?"
                    start = Xml.Substring(start + 1).IndexOf("<")
                End While
                Dim ende As Integer = xml.IndexOf(">")
                While Not xml.Substring(ende - 1, 2) = "?>"
                    Dim str As String = xml.Substring(ende - 1, 2)
                    ende = xml.Substring(ende + 1).IndexOf(">")
                End While
                xml = xml.Remove(start, ende + 2 - start)
            End While
            Return Xml
        End If
        Return ""
    End Function


    Public Function isOK() As Boolean
        If returncode >= 200 AndAlso returncode < 300 Then
            Return True
        Else : Return False
        End If
    End Function

    Public Function isERR() As Boolean
        If returncode >= 400 Then
            Return True
        Else : Return False
        End If
    End Function

    Public Function isINFO() As Boolean
        If returncode >= 100 AndAlso returncode < 200 Then
            Return True
        Else : Return False
        End If
    End Function

    Public Function isUNKNOWN() As Boolean
        If returncode = CasparReturnCode.UNKNOWN_RETURNCODE Then
            Return True
        Else : Return False
        End If
    End Function

End Class


'' Base Class for all playable media in CasparCG which are
'' movies, stills, audios, colors and templates
Public MustInherit Class CasparCGMedia
    Private name As String
    Private path As String
    Private Infos As Dictionary(Of String, String)

    Enum MediaType
        STILL = 0
        MOVIE = 1
        AUDIO = 2
        TEMPLATE = 3
        COLOR = 4
    End Enum

    Public MustOverride Function getMediaType() As MediaType

    Public Sub New(ByVal name As String)
        Me.name = parseName(name)
        Me.path = parsePath(name)
        Infos = New Dictionary(Of String, String)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        Me.name = parseName(name)
        Me.path = parsePath(name)
        Infos = New Dictionary(Of String, String)
        parseXML(xml)
    End Sub

    Public Function parseName(ByVal nameWithPath As String) As String
        If nameWithPath.Contains("\") Then
            Return nameWithPath.Substring(nameWithPath.LastIndexOf("\") + 1)
        ElseIf nameWithPath.Contains("/") Then
            Return nameWithPath.Substring(name.LastIndexOf("/") + 1)
        Else
            Return nameWithPath
        End If
    End Function

    Public Function parsePath(ByVal nameWithPath As String) As String
        If nameWithPath.Contains("\") Then
            Return nameWithPath.Substring(0, nameWithPath.LastIndexOf("\") + 1)
        ElseIf nameWithPath.Contains("/") Then
            Return nameWithPath.Substring(0, nameWithPath.LastIndexOf("/") + 1)
        Else
            Return ""
        End If
    End Function

    Public Overridable Sub parseXML(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            '' Add all mediaInformation found by INFO
            For Each info As MSXML2.IXMLDOMNode In configDoc.firstChild.childNodes
                addInfo(info.nodeName, info.nodeTypedValue)
            Next
        End If
    End Sub

    Public Function getName() As String
        Return name
    End Function

    Public Function getPath() As String
        Return path
    End Function

    Public Function getFullName() As String
        Return path & name
    End Function

    Public Function getInfo(ByVal info As String) As String
        If Infos.ContainsKey(info) Then
            Return Infos.Item(info)
        Else : Return ""
        End If
    End Function

    Public Function getInfos() As Dictionary(Of String, String)
        Return Infos
    End Function

    Public Function containsInfo(ByVal info As String) As Boolean
        Return Infos.ContainsKey(info)
    End Function

    Public Sub addInfo(ByVal info As String, ByVal value As String)
        Infos.Add(info, value)
    End Sub

End Class

Public Class CasparCGColor
    Inherits CasparCGMedia

    Public Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name, xml)
    End Sub

    Public Overrides Function getMediaType() As CasparCGMedia.MediaType
        Return MediaType.COLOR
    End Function
End Class

Public Class CasparCGMovie
    Inherits CasparCGMedia

    Public Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name, xml)
    End Sub

    Public Overrides Function getMediaType() As CasparCGMedia.MediaType
        Return MediaType.MOVIE
    End Function

End Class

Public Class CasparCGStill
    Inherits CasparCGMedia

    Public Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name, xml)
    End Sub

    Public Overrides Function getMediaType() As CasparCGMedia.MediaType
        Return MediaType.STILL
    End Function
End Class

Public Class CasparCGAudio
    Inherits CasparCGMedia

    Public Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name, xml)
    End Sub

    Public Overrides Function getMediaType() As CasparCGMedia.MediaType
        Return MediaType.AUDIO
    End Function

End Class

Public Class CasparCGTemplate
    Inherits CasparCGMedia

    Private components As Dictionary(Of String, CasparCGTemplateComponent)
    Private data As CasparCGTemplateData

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name)
        components = New Dictionary(Of String, CasparCGTemplateComponent)
        parseXML(xml)
    End Sub

    Public Overrides Function getMediaType() As CasparCGMedia.MediaType
        Return MediaType.TEMPLATE
    End Function

    Public Overrides Sub parseXML(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            '' Attribute verarbeiten
            For Each attrib As MSXML2.IXMLDOMNode In configDoc.selectSingleNode("template").attributes
                addInfo(attrib.nodeName, attrib.nodeTypedValue)
            Next

            '' Components verarbeiten
            For Each comp As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("component")
                addComponent(New CasparCGTemplateComponent(comp.xml))
            Next

            '' Instances
            For Each instance As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("instance")
                If Not IsDBNull(instance.attributes.getNamedItem("type")) AndAlso Not IsDBNull(instance.attributes.getNamedItem("name")) AndAlso containsComponent(instance.attributes.getNamedItem("type").nodeTypedValue) Then
                    data.addInstance(New CasparCGTemplateInstance(instance.attributes.getNamedItem("name").nodeTypedValue, getComponent(instance.attributes.getNamedItem("type").nodeTypedValue)))
                End If
            Next
        End If
    End Sub

    Private Sub addComponent(ByRef component As CasparCGTemplateComponent)
        If Not components.ContainsValue(component) Then
            components.Add(component.getName, component)
        End If
    End Sub

    Public Function getData() As CasparCGTemplateData
        Return data
    End Function

    Public Function getComponents() As IEnumerable(Of CasparCGTemplateComponent)
        Return components.Values
    End Function

    Public Function getComponent(ByVal componentName As String) As CasparCGTemplateComponent
        If components.ContainsKey(componentName) Then
            Return components.Item(componentName)
        Else : Return Nothing
        End If
    End Function

    Public Function containsComponent(ByVal compnentName As String) As Boolean
        Return components.ContainsKey(compnentName)
    End Function

End Class

Public Class CasparCGTemplateData

    '' Beinhaltet alle Daten für das Template
    '' also seine Component Instances
    Private instances As Dictionary(Of String, CasparCGTemplateInstance)

    Public Sub New()
        instances = New Dictionary(Of String, CasparCGTemplateInstance)
    End Sub

    Public Sub New(ByRef instances As IEnumerable(Of CasparCGTemplateInstance))
        instances = New Dictionary(Of String, CasparCGTemplateInstance)
        For Each instance As CasparCGTemplateInstance In instances
            addInstance(instance)
        Next
    End Sub

    Public Sub addInstance(ByRef instance As CasparCGTemplateInstance)
        If Not IsNothing(instance) AndAlso Not contains(instance.getName) Then
            instances.Add(instance.getName, instance)
        End If
    End Sub

    Public Function getInstance(ByVal instanceName As String) As CasparCGTemplateInstance
        If instances.ContainsKey(instanceName) Then
            Return instances.Item(instanceName)
        Else
            Return Nothing
        End If
    End Function

    Public Function getInstances() As IEnumerable(Of CasparCGTemplateInstance)
        Return instances.Values
    End Function

    Public Function contains(ByVal instanceName As String) As Boolean
        Return instances.ContainsKey(instanceName)
    End Function

    Public Function toXML() As String
        Dim xml As String = "<templateData>"
        For Each instance As CasparCGTemplateInstance In instances.Values
            xml = xml & instance.toXML
        Next
        Return xml & "</templateData>"
    End Function

End Class

Public Class CasparCGTemplateComponent
    '' Beinhaltet alle Eigenschaften einer CasparCG Template Componente 
    Private name As String
    Private properties As Dictionary(Of String, CasparCGTemplateComponentProperty)

    Public Sub New(ByVal xml As String)
        properties = New Dictionary(Of String, CasparCGTemplateComponentProperty)
        parseXML(xml)
    End Sub

    Private Sub parseXML(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            name = configDoc.nodeName
            For Each prop As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("property")
                addProperty(New CasparCGTemplateComponentProperty(prop.xml))
            Next
        End If
    End Sub

    Public Sub addProperty(ByRef componentProperty As CasparCGTemplateComponentProperty)
        If Not properties.ContainsKey(componentProperty.propertyName) AndAlso Not properties.ContainsValue(componentProperty) Then
            properties.Add(componentProperty.propertyName, componentProperty)
        End If
    End Sub

    Public Function getName() As String
        Return name
    End Function

    Public Function getProperties() As IEnumerable(Of CasparCGTemplateComponentProperty)
        Return properties.Values
    End Function

    Public Function getProperty(ByVal name As String) As CasparCGTemplateComponentProperty
        If properties.ContainsKey(name) Then
            Return properties.Item(name)
        Else
            Return Nothing
        End If
    End Function

    Public Function containsProperty(ByVal name As String) As Boolean
        Return properties.ContainsKey(name)
    End Function

    Public Function toXML() As String
        Dim xml As String = "<component name='" & name & "'>"
        For Each prop As CasparCGTemplateComponentProperty In properties.Values
            xml = xml & prop.toXML
        Next
        Return xml & "</component>"
    End Function

End Class

Public Class CasparCGTemplateComponentProperty

    Public Property propertyName As String = ""
    Public Property propertyType As String = "none"
    Public Property propertyInfo As String = "This property is not initialized"

    Public Sub New(ByVal name As String, ByVal type As String, ByVal info As String)
        propertyName = name
        propertyType = type
        propertyInfo = info
    End Sub

    Public Sub New(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            If Not IsNothing(configDoc.selectSingleNode("property")) AndAlso Not IsDBNull(configDoc.selectSingleNode("property")) Then
                propertyName = configDoc.selectSingleNode("property").attributes.getNamedItem("name").nodeTypedValue
                propertyType = configDoc.selectSingleNode("property").attributes.getNamedItem("type").nodeTypedValue
                propertyInfo = configDoc.selectSingleNode("property").attributes.getNamedItem("info").nodeTypedValue
            End If
        End If
    End Sub

    Public Function toXML() As String
        Return "<property name='" & propertyName & "' type='" & propertyType & "' info='" & propertyInfo & "'/>"
    End Function

End Class

Public Class CasparCGTemplateInstance
    '' Ist eine Instance einer CasparCG Componente in einem Template und beinhaltet den namen, die Componente und die Daten
    Private name As String
    Private component As CasparCGTemplateComponent
    Private values As Dictionary(Of CasparCGTemplateComponentProperty, String)

    Public Sub New(ByVal name As String, ByRef component As CasparCGTemplateComponent)
        Me.name = name
        values = New Dictionary(Of CasparCGTemplateComponentProperty, String)
        For Each prop As CasparCGTemplateComponentProperty In component.getProperties
            values.Add(prop, "")
        Next
    End Sub

    Public Sub setData(ByRef componentProperty As CasparCGTemplateComponentProperty, ByVal value As String)
        If values.ContainsKey(componentProperty) Then
            values.Item(componentProperty) = value
        End If
    End Sub

    Public Function getData(ByRef componentProperty As CasparCGTemplateComponentProperty) As String
        If values.ContainsKey(componentProperty) Then
            Return values.Item(componentProperty)
        Else
            Return ""
        End If
    End Function

    Public Function getComponent() As CasparCGTemplateComponent
        Return component
    End Function

    Public Function getName() As String
        Return name
    End Function

    Public Function toXML() As String
        Dim xml As String = "<componentData id='" & getName() & "'>"
        For Each prop As CasparCGTemplateComponentProperty In values.Keys
            xml = xml & "<data id='" & prop.propertyName & "' value='" & values.Item(prop) & "'>"
        Next
        Return xml & "</componentData>"
    End Function
End Class


Public Class CasparCGTransition

    Public Enum Transitions
        CUT = 0
        MIX = 1
        PUSH = 2
        WIPE = 3
        SLIDE = 4
    End Enum

    Public Enum Directions
        LEFT = 1
        RIGHT = 0
    End Enum

    Public Enum Tweens
        linear = 0
        easenone
        easeinquad
        easeoutquad
        easeinoutquad
        easeoutinquad
        easeincubic
        easeoutcubic
        easeinoutcubic
        easeoutincubic
        easeinquart
        easeoutquart
        easeinoutquart
        easeoutinquart
        easeinquint
        easeoutquint
        easeinoutquint
        easeoutinquint
        easeinsine
        easeoutsine
        easeinoutsine
        easeoutinsine
        easeinexpo
        easeoutexpo
        easeinoutexpo
        easeoutinexpo
        easeincirc
        easeoutcirc
        easeinoutcirc
        easeoutincirc
        easeinelastic
        easeoutelastic
        easeinoutelastic
        easeoutinelastic
        easeinback
        easeoutback
        easeinoutback
        easeoutintback
        easeoutbounce
        easeinbounce
        easeinoutbounce
        easeoutinbounce
    End Enum

    Private trans As Transitions
    Private duration As Integer
    Private direction As Directions
    Private tween As Tweens

    Public Sub New(ByVal transition As Transitions, Optional ByVal duration As Integer = 0, Optional ByVal direction As Directions = Directions.RIGHT, Optional ByVal tween As Tweens = Tweens.linear)
        '' Logik checken!!
        trans = transition
        Me.duration = duration
        Me.direction = direction
        Me.tween = tween
    End Sub

    Public Overloads Function toString() As String
        Return Transitions.GetName(GetType(Transitions), trans) & " " & duration & " " & Directions.GetName(GetType(Directions), direction) & Tweens.GetName(GetType(Tweens), tween)
    End Function

End Class
