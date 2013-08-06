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

Public Class PlaylistTemplateItem
    Inherits PlaylistItem
    Implements IPlaylistItem

    Private media As CasparCGTemplate
    Private flashlayer As Integer = -1

    Public Sub New(ByVal name As String, ByRef controller As ServerControler, ByVal template As CasparCGTemplate, Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal flashlayer As Integer = -1, Optional ByVal duration As Long = -1)
        MyBase.New(name, PlaylistItemTypes.TEMPLATE, controller, channel, layer, duration)
    End Sub
End Class
