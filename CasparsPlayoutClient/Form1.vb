Public Class Form1
    Dim con As CasparCGConnection
    Dim sc As ServerController

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        logger.debug("Try to connect casparCG server..")
        con = New CasparCGConnection("localhost", 5250)
        'con.connect()
        sc = New ServerController()
        sc.open()
        'logger.debug("Connected to server")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim media As CasparCGMedia = New CasparCGMovie("amb")
        media.parseXML(sc.getMediaInfo(media))
        logger.log("MediaInfo (" & media.getMediaType.ToString & ") for " & media.getFullName)
        logger.log("duration at channel 1=" & sc.getMediaDuration(media, 1) & "ms")
        logger.log("duration=" & sc.getOriginalMediaDuration(media) & "ms")
        For Each info As String In media.getInfos.Keys
            logger.log(info & "=" & media.getInfo(info))
        Next

        media = New CasparCGTemplate("caspar_media_playback_template\telegram\TELEGRAM", "")
        media.parseXML(sc.getMediaInfo(media))
        logger.log("MediaInfo (" & media.getMediaType.ToString & ") for " & media.getFullName)
        logger.log("duration at channel 1=" & sc.getMediaDuration(media, 1) & "ms")
        logger.log("duration=" & sc.getOriginalMediaDuration(media) & "ms")
        For Each info As String In media.getInfos.Keys
            logger.log(info & "=" & media.getInfo(info))
        Next

        media = New CasparCGStill("split")
        media.parseXML(sc.getMediaInfo(media))
        logger.log("MediaInfo (" & media.getMediaType.ToString & ") for " & media.getFullName)
        logger.log("duration at channel 1=" & sc.getMediaDuration(media, 1) & "ms")
        logger.log("duration=" & sc.getOriginalMediaDuration(media) & "ms")
        For Each info As String In media.getInfos.Keys
            logger.log(info & "=" & media.getInfo(info))
        Next

        media = New CasparCGColor("#FFFF00FF")
        media.parseXML(sc.getMediaInfo(media))
        logger.log("MediaInfo (" & media.getMediaType.ToString & ") for " & media.getFullName)
        logger.log("duration at channel 1=" & sc.getMediaDuration(media, 1) & "ms")
        logger.log("duration=" & sc.getOriginalMediaDuration(media) & "ms")
        For Each info As String In media.getInfos.Keys
            logger.log(info & "=" & media.getInfo(info))
        Next

        media = New CasparCGAudio("Enlighten")
        media.parseXML(sc.getMediaInfo(media))
        logger.log("MediaInfo (" & media.getMediaType.ToString & ") for " & media.getFullName)
        logger.log("duration at channel 1=" & sc.getMediaDuration(media, 1) & "ms")
        logger.log("duration=" & sc.getOriginalMediaDuration(media) & "ms")
        For Each info As String In media.getInfos.Keys
            logger.log(info & "=" & media.getInfo(info))
        Next
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.addLogAction(New consoleLogger(4))
        logger.log("Console Logger startet")
    End Sub
End Class
