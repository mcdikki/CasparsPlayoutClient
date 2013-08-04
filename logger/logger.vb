''' <summary>
''' This module is a logging and debugging utility which provides functions for logging eighter to a file or a console
''' </summary>
''' <remarks></remarks>
Public Module logger

    Private messages As List(Of message) = New List(Of message)
    Private logActions As List(Of ILogAction) = New List(Of ILogAction)
    Public stdLogAction As New consoleLogger(loglevels.log)


    Public Enum loglevels
        critical = 0
        err = 1
        warn = 2
        log = 3
        debug = 4
    End Enum

    Public Sub config(ByVal xml As String)

        ' XML Verarbeiten

        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            For Each logDefinition As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("logger")
                Try
                    Select Case Strings.LCase(logDefinition.selectSingleNode("type").nodeTypedValue)
                        ''
                        '' Add your own logActions here
                        ''
                        Case "console"
                            Dim loglevel As Byte = logDefinition.selectSingleNode("loglevel").nodeTypedValue
                            Dim colored As Boolean = logDefinition.selectSingleNode("colored").nodeTypedValue
                            addLogAction(New consoleLogger(loglevel, colored))
                            debug("consoleLogger added.")
                        Case "file"
                            Dim loglevel As Byte = logDefinition.selectSingleNode("loglevel").nodeTypedValue
                            Dim file As String = logDefinition.selectSingleNode("logfile").nodeTypedValue
                            Dim directWrite As Boolean = logDefinition.selectSingleNode("writethrough").nodeTypedValue
                            Dim append As Boolean = logDefinition.selectSingleNode("append").nodeTypedValue
                            addLogAction(New fileLogger(loglevel, file, directWrite, append))
                            debug("fileLogger added.")
                    End Select
                Catch e As Exception
                    err(e.Message)
                End Try
            Next
        End If
    End Sub

    Public Sub addLogAction(ByRef logAction As ILogAction)
        If Not logActions.Contains(logAction) Then
            logActions.Add(logAction)
            logAction.open()
        End If
    End Sub

    Public Sub removeLogAction(ByRef logAction As ILogAction)
        If logActions.Contains(logAction) Then
            logAction.close()
            logActions.Remove(logAction)
        End If
    End Sub

    Public Sub flush()
        For Each lg As ILogAction In logActions
            lg.flush()
        Next
    End Sub

    Public Sub close()
        For Each lg As ILogAction In logActions
            lg.close()
        Next
    End Sub

    Public Sub log(ByVal loglevel As Byte, ByVal logMessage As String)
        Dim message As New message(loglevel, logMessage)
        messages.Add(message)
        logAction(message)
    End Sub

    Public Sub debug(ByVal logMessage As String)
        log(loglevels.debug, logMessage)
    End Sub

    Public Sub log(ByVal logMessage As String)
        log(loglevels.log, logMessage)
    End Sub

    Public Sub warn(ByVal logMessage As String)
        log(loglevels.warn, logMessage)
    End Sub

    Public Sub err(ByVal logMessage As String)
        log(loglevels.err, logMessage)
    End Sub

    Public Sub critical(ByVal logMessage As String)
        log(loglevels.critical, logMessage)
    End Sub

    Private Function getLastMessages(ByVal loglevel As Byte, ByVal numberOfMessages As Integer) As List(Of message)
        Dim out As List(Of message) = New List(Of message)
        If numberOfMessages < 0 Then numberOfMessages = messages.Count
        For mes As Integer = messages.Count - 1 To 0 Step -1
            Dim message As message = messages.Item(mes)
            If numberOfMessages = 0 Then Exit For
            If message.getLevel <= loglevel Then
                numberOfMessages = numberOfMessages - 1
                out.Add(message)
            End If
        Next
        Return out
    End Function

    Public Function getLastLog(ByVal loglevel As Byte, ByVal numberOfMessages As Integer) As String
        Dim out As String = ""
        Dim lastMessages As List(Of message) = getLastMessages(loglevel, numberOfMessages)
        For Each message As message In lastMessages
            out = message.toString & vbNewLine & out
        Next
        Return out
    End Function

    Public Sub printDebug()
        Dim LastMessages = getLastMessages(loglevels.debug, -1)
        LastMessages.Reverse()
        For Each message As message In LastMessages
            message.print()
        Next
    End Sub

    Public Sub printLog()
        Dim LastMessages = getLastMessages(loglevels.log, -1)
        LastMessages.Reverse()
        For Each message As message In LastMessages
            message.print()
        Next
    End Sub

    Public Sub printWarn()
        Dim LastMessages = getLastMessages(loglevels.warn, -1)
        LastMessages.Reverse()
        For Each message As message In LastMessages
            message.print()
        Next
    End Sub

    Public Sub printErr()
        Dim LastMessages = getLastMessages(loglevels.err, -1)
        LastMessages.Reverse()
        For Each message As message In LastMessages
            message.print()
        Next
    End Sub

    Public Sub printCritical()
        Dim LastMessages = getLastMessages(loglevels.critical, -1)
        LastMessages.Reverse()
        For Each message As message In LastMessages
            message.print()
        Next
    End Sub

    Public Function getDebug() As String
        Return getLastLog(loglevels.debug, -1)
    End Function

    Public Function getLog() As String
        Return getLastLog(loglevels.log, -1)
    End Function

    Public Function getWarn() As String
        Return getLastLog(loglevels.warn, -1)
    End Function

    Public Function getErr() As String
        Return getLastLog(loglevels.err, -1)
    End Function

    Public Function getCritical() As String
        Return getLastLog(loglevels.critical, -1)
    End Function

    Public Function getLastDebug() As String
        Return getLastLog(loglevels.debug, 1)
    End Function

    Public Function getLastLog() As String
        Return getLastLog(loglevels.log, 1)
    End Function

    Public Function getLastWarn() As String
        Return getLastLog(loglevels.warn, 1)
    End Function

    Public Function getLastErr() As String
        Return getLastLog(loglevels.err, 1)
    End Function

    Public Function getLastDebug(ByVal numberOfMessages As Byte) As String
        Return getLastLog(loglevels.debug, numberOfMessages)
    End Function

    Public Function getLastLog(ByVal numberOfMessages As Byte) As String
        Return getLastLog(loglevels.log, numberOfMessages)
    End Function

    Public Function getLastWarn(ByVal numberOfMessages As Byte) As String
        Return getLastLog(loglevels.warn, numberOfMessages)
    End Function

    Public Function getLastErr(ByVal numberOfMessages As Byte) As String
        Return getLastLog(loglevels.err, numberOfMessages)
    End Function

    Public Function getLastCritical(ByVal numberOfMessages As Byte) As String
        Return getLastLog(loglevels.critical, numberOfMessages)
    End Function

    Private Sub logAction(ByVal message As message)
        For Each la As ILogAction In logActions
            la.receiveMessage(message)
        Next
    End Sub
End Module

Public Class consoleLogger
    Implements ILogAction

    Declare Function AllocConsole Lib "kernel32" () As Integer
    Declare Function FreeConsole Lib "kernel32" () As Integer

    Private level As Byte
    Private colored As Boolean

    Public Sub New(ByVal loglevel As Byte, Optional ByVal colored As Boolean = True)
        level = loglevel
        Me.colored = colored
    End Sub

    Public Sub setLoglevel(ByVal loglevel As Byte) Implements ILogAction.setLoglevel
        level = loglevel
    End Sub

    Public Function getLevel() As Byte Implements ILogAction.getLogLevel
        Return level
    End Function

    Public Sub receiveMessage(ByVal message As message) Implements ILogAction.receiveMessage
        If message.getLevel <= level Then
            If colored Then
                message.print()
            Else
                Console.WriteLine(message.toString)
            End If
        End If
    End Sub

    Public Sub close() Implements ILogAction.close
        FreeConsole()
    End Sub

    Public Sub flush() Implements ILogAction.flush
    End Sub

    Public Sub open() Implements ILogAction.open
        AllocConsole()
    End Sub
End Class

Public Class fileLogger
    Implements ILogAction

    Private level As Byte
    Private file As String
    Private directWrite As Boolean
    Private buffer As String

    ''' <summary>
    ''' Creates a new fileLogger which writes log messages at a given loglevel and lower to a logfile
    ''' </summary>
    ''' <param name="loglevel">the loglevel between 0 (critical error) and greater/equal 4 (debug)</param>
    ''' <param name="logfile">the filename with path to the logfile</param>
    ''' <param name="directWrite">enables imediate writing of every messages to file. If disabled, messages will only be written when manuelly flushed or logger closed</param>
    ''' <param name="appendExisting">enables appending to existing files. If disabled, old logfiles will be overwritten</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal loglevel As Byte, ByVal logfile As String, ByVal directWrite As Boolean, Optional ByVal appendExisting As Boolean = True)
        level = loglevel
        file = logfile
        Me.directWrite = directWrite
        buffer = ""

        'If appendExisting Then
        My.Computer.FileSystem.WriteAllText(file, "", appendExisting)
        'End If
    End Sub

    Public Sub setLoglevel(ByVal loglevel As Byte) Implements ILogAction.setLoglevel
        level = loglevel
    End Sub

    Public Function getLevel() As Byte Implements ILogAction.getLogLevel
        Return level
    End Function

    Public Sub receiveMessage(ByVal message As message) Implements ILogAction.receiveMessage
        If message.getLevel <= level Then
            buffer = buffer & message.toString & vbNewLine
            If directWrite Then
                flush()
            End If
        End If
    End Sub

    Public Sub close() Implements ILogAction.close
        flush()
    End Sub

    Public Sub flush() Implements ILogAction.flush
        If buffer.Length > 0 Then
            Try
                My.Computer.FileSystem.WriteAllText(file, buffer, True)
                buffer = ""
            Catch e As IO.IOException
                Threading.Thread.Sleep(Int(5 + (Rnd() * 50)))
                flush()
            End Try
        End If
    End Sub

    Public Sub open() Implements ILogAction.open
    End Sub
End Class


Public Class message
    Private level As logger.loglevels
    Private text As String
    Private time As Date
    Private colormap As ConsoleColor()

    Public Sub New(ByVal loglevel As Byte, ByVal logmessage As String)
        level = loglevel
        text = logmessage
        time = Now
        colormap = {ConsoleColor.DarkRed, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkGreen, ConsoleColor.Blue}
    End Sub

    Public Sub New(ByVal loglevel As Byte, ByVal logmessage As String, ByRef colormap As ConsoleColor())
        level = loglevel
        text = logmessage
        time = Now
        MyClass.colormap = colormap
    End Sub

    Public Function getTime() As Date
        Return time
    End Function

    Public Function getMessage() As String
        Return text
    End Function

    Public Function getLevel() As Byte
        Return level
    End Function

    Public Overrides Function toString() As String
        Return time.ToString & ": " + level.ToString & "(" & level & "): " & text
    End Function

    Public Sub print()
        Console.ResetColor()
        If level < colormap.Length Then
            Console.ForegroundColor = colormap(level)
        Else
            Console.ForegroundColor = colormap(colormap.Length - 1)
        End If
        Console.WriteLine(toString)
        Console.ResetColor()
    End Sub
End Class


Public Interface ILogAction
    Sub setLoglevel(ByVal loglevel As Byte)
    Function getLogLevel() As Byte
    Sub receiveMessage(ByVal message As message)
    Sub flush()
    Sub close()
    Sub open()
End Interface