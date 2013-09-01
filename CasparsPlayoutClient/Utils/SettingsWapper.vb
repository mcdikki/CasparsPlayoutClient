Imports System.ComponentModel

Public Class SettingsWapper
    ''------------
    '' NETWORK
    ''------------
    <DescriptionAttribute("The OSC UDP port to listen for incomming OSC messages."), CategoryAttribute("Network")>
    Public Property OscPort As Integer
        Get
            Return My.Settings.oscPort
        End Get
        Set(value As Integer)
            My.Settings.oscPort = value
        End Set
    End Property

    <DescriptionAttribute("The default ACMP TCP port."), CategoryAttribute("Network")>
    Public Property AcmpPort As Integer
        Get
            Return My.Settings.deafaultAcmpPort
        End Get
        Set(value As Integer)
            My.Settings.deafaultAcmpPort = value
        End Set
    End Property

    <DescriptionAttribute("The default casparCG server address."), CategoryAttribute("Network")>
    Public Property AcmpServer As String
        Get
            Return My.Settings.defaultAcmpServer
        End Get
        Set(value As String)
            My.Settings.defaultAcmpServer = value
        End Set
    End Property

    <DescriptionAttribute("The network timout in ms."), CategoryAttribute("Network")>
    Public Property connectionTimeout As Integer
        Get
            Return My.Settings.connectionTimeout
        End Get
        Set(value As Integer)
            My.Settings.connectionTimeout = value
        End Set
    End Property

    <DescriptionAttribute("The time to wait in ms until a reconnection attempts fails."), CategoryAttribute("Network")>
    Public Property reconnectTimout As Integer
        Get
            Return My.Settings.reconnectTimout
        End Get
        Set(value As Integer)
            My.Settings.reconnectTimout = value
        End Set
    End Property

    <DescriptionAttribute("The number of reconnect attemps before giving up."), CategoryAttribute("Network")>
    Public Property reconnectAttemps As Integer
        Get
            Return My.Settings.reconnectTries
        End Get
        Set(value As Integer)
            My.Settings.reconnectTries = value
        End Set
    End Property


    ''-------------------
    '' Appeariance
    ''-------------------
    <DescriptionAttribute("If the remaining time is smaller than the warn time (in ms), there will be a visual feedback."), CategoryAttribute("Appeariance")>
    Public Property WarnTime As Integer
        Get
            Return My.Settings.warnTime
        End Get
        Set(value As Integer)
            My.Settings.warnTime = value
        End Set
    End Property

    <DescriptionAttribute("If the remaining time is smaller than the noWarn time (in ms), there will be a visual feedback."), CategoryAttribute("Appeariance")>
    Public Property NoWarnTime As Integer
        Get
            Return My.Settings.noWarnTime
        End Get
        Set(value As Integer)
            My.Settings.noWarnTime = value
        End Set
    End Property

    <DescriptionAttribute("The time in ms log messages will be shown."), CategoryAttribute("Appeariance")>
    Public Property LogShowTime As Integer
        Get
            Return My.Settings.logShowTime
        End Get
        Set(value As Integer)
            My.Settings.logShowTime = value
        End Set
    End Property

    <DescriptionAttribute("Sets whether or not, the last window size, position and it's components layout will be restored at startup."), CategoryAttribute("Appeariance")>
    Public Property rememberLayout As Boolean
        Get
            Return My.Settings.rememberLayout
        End Get
        Set(value As Boolean)
            My.Settings.rememberLayout = value
        End Set
    End Property

    <DescriptionAttribute("Sets whether or not, the library will be saved and loaded at program startup without a server connection."), CategoryAttribute("Appeariance")>
    Public Property rememberLibrary As Boolean
        Get
            Return My.Settings.rememberLibrary
        End Get
        Set(value As Boolean)
            My.Settings.rememberLibrary = value
        End Set
    End Property

    <DescriptionAttribute("Sets whether or not, the playlist will be saved and loaded at program startup."), CategoryAttribute("Appeariance")>
    Public Property rememberPlaylist As Boolean
        Get
            Return My.Settings.rememberPlaylist
        End Get
        Set(value As Boolean)
            My.Settings.rememberPlaylist = value
        End Set
    End Property

    <DescriptionAttribute("Sets whether or not, the server and port will be saved and loaded at program startup."), CategoryAttribute("Appeariance")>
    Public Property rememberConnection As Boolean
        Get
            Return My.Settings.rememberConnection
        End Get
        Set(value As Boolean)
            My.Settings.rememberConnection = value
        End Set
    End Property


    ''-------------------
    '' Logging
    ''-------------------
    <DescriptionAttribute("Sets whether or not log messages will be writen to a file."), CategoryAttribute("Logging")>
    Public Property logToFile As Boolean
        Get
            Return My.Settings.logToFile
        End Get
        Set(value As Boolean)
            My.Settings.logToFile = value
        End Set
    End Property

    <DescriptionAttribute("Sets whether or not a console window with log messages will be shown.."), CategoryAttribute("Logging")>
    Public Property logToConsole As Boolean
        Get
            Return My.Settings.logToConsole
        End Get
        Set(value As Boolean)
            My.Settings.logToConsole = value
        End Set
    End Property

    <DescriptionAttribute("Sets log level between 0 (critical) and 4 (debug). Default is 3 (log)"), CategoryAttribute("Logging")>
    Public Property logLevel As logger.loglevels 
        Get
            Return My.Settings.loglevel
        End Get
        Set(value As logger.loglevels)
            My.Settings.loglevel = value
        End Set
    End Property

    <DescriptionAttribute("Sets the file name the log will be written to."), CategoryAttribute("Logging")>
    Public Property logFile As String
        Get
            Return My.Settings.logfile
        End Get
        Set(value As String)
            My.Settings.logfile = value
        End Set
    End Property


    ''-------------------
    '' FilePath
    ''-------------------

    <DescriptionAttribute("Sets the directory the log file will be written to."), CategoryAttribute("Storage")>
    Public Property LogDirectory As String
        Get
            Return My.Settings.logdir
        End Get
        Set(value As String)
            My.Settings.logdir = value
        End Set
    End Property

    <DescriptionAttribute("Sets the directory the playlists will be saved to."), CategoryAttribute("Storage")>
    Public Property PlaylistDirectory As String
        Get
            Return My.Settings.playlistdir
        End Get
        Set(value As String)
            My.Settings.playlistdir = value
        End Set
    End Property

    <DescriptionAttribute("Sets the directory the library will be saved to."), CategoryAttribute("Storage")>
    Public Property LibraryDirectory As String
        Get
            Return My.Settings.libdir
        End Get
        Set(value As String)
            My.Settings.libdir = value
        End Set
    End Property


    ''----------------------
    '' Core
    ''----------------------
    <DescriptionAttribute("Sets the number of frames between status updates. Default is every frame (1)"), CategoryAttribute("Core")>
    Public Property FramePerTick As Integer
        Get
            Return My.Settings.frameTickInterval
        End Get
        Set(value As Integer)
            My.Settings.frameTickInterval = value
        End Set
    End Property

    <DescriptionAttribute("Sets whether or not strict version control is enabled. Only commands and parameter supported by the connected server will be applied. Unsupported commands will interrupt the program."), CategoryAttribute("Core")>
    Public Property StrictVersionControl As Boolean
        Get
            Return My.Settings.strictVersionControl
        End Get
        Set(value As Boolean)
            My.Settings.strictVersionControl = value
        End Set
    End Property

End Class
