

''' <summary>
''' A trigger is a tool that helps to automate the client. It performs a given action on a playlist when the defined condition of the trigger is reached.
''' This can be used for time based playlists, GPIO support or other automation techniques.
''' </summary>
''' <remarks></remarks>
Public Interface ITrigger

    ''' <summary>
    ''' Sets the destination playlist of the trigger actions.
    ''' </summary>
    ''' <param name="playlist">the playlist this trigger shoulb be set to</param>
    Sub setPlaylist(ByRef playlist As IPlaylistItem)

    ''' <summary>
    ''' Sets the action this trigger will perform on the playlist if triggered
    ''' </summary>
    ''' <param name="action">the action to be performed</param>
    Sub setAction(ByVal action As TriggerAction)

    ''' <summary>
    ''' Performs the trigger actions on the playlist.
    ''' </summary>
    Sub trigger()

    ''' <summary>
    ''' enables or disables this trigger. Only active triggers will perform actions on the playlist.
    ''' </summary>
    ''' <param name="active">the new trigger state</param>
    Sub setActive(ByVal active As Boolean)

    ''' <summary>
    ''' Sets the given trigger parameter to the given value.
    ''' </summary>
    ''' <param name="parameter">the parameter to set</param>
    ''' <param name="value">the value to set</param>
    Sub setParamter(ByVal parameter As String, ByRef value As String)

    ''' <summary>
    ''' Returns the destination playlist of the trigger actions.
    ''' </summary>
    ''' <returns>the playlist this trigger is set to</returns>
    Function getPlaylist() As IPlaylistItem

    ''' <summary>
    ''' Returns whether or not this triggers is active. Only active trigger will perform actions on the playlist.
    ''' </summary>
    ''' <returns>True, if and only if, this trigger is active and will trigger actions, false otherwise</returns>
    Function isActive() As Boolean

    ''' <summary>
    ''' The name of this trigger
    ''' </summary>
    ''' <returns>the name</returns>
    Function getName() As String

    ''' <summary>
    ''' Returns the action this trigger will perform if triggered.
    ''' </summary>
    ''' <returns>the action</returns>
    Function getAction() As TriggerAction

    ''' <summary>
    ''' Returns the value of the requested parameter or "" if the parameter is not set.
    ''' </summary>
    ''' <param name="parameter">the parameter</param>
    ''' <returns>the parameters value or ""</returns>
    Function getCommandParameter(ByVal parameter As String) As String

    ''' <summary>
    ''' Returns all available paramters of this trigger
    ''' </summary>
    ''' <returns>the parameters of this trigger </returns>
    Function getCommandParameters() As Dictionary(Of String, String)

    ''' <summary>
    ''' Returns a Description of the given parameter or "" if the parameter is not set.
    ''' </summary>
    ''' <param name="parameter">the parameter</param>
    ''' <returns>a Description</returns>
    Function getCommandParameterDescription(ByVal parameter As String) As String

    ''' <summary>
    ''' The action a trigger performs in the playlist
    ''' </summary>
    ''' <remarks></remarks>
    Enum TriggerAction
        ' Movie/Audio
        TriggerPlay
        TriggerStop
        TriggerPause
        TriggerAbort

        ' Template
        TriggerNext
        TriggerUpdate

        ' Playlists
        TriggerRemove
        TriggerLoad
    End Enum

End Interface