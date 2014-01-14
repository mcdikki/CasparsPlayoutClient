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

Public Class PlaylistAudioItem
    Inherits AbstractPlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGAudio

    Public Sub New(ByVal name As String, ByRef controller As ServerController, ByVal audio As CasparCGAudio, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.AUDIO, controller, channel, layer, duration)
    End Sub

    Public Overrides Sub abort()

    End Sub

    Public Overrides Sub halt()

    End Sub

    Public Overrides Sub load()

    End Sub

    Public Overrides Sub show()

    End Sub

    Public Overrides Sub pause()

    End Sub

    Public Overrides Sub playNextItem(Optional ByRef lastPlayed As IPlaylistItem = Nothing)

    End Sub

    Public Overrides Sub start()

    End Sub

    Public Overrides Sub stoppedPlaying()

    End Sub

    Public Overrides Sub unPause()

    End Sub
End Class