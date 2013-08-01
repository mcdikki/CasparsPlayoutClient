CasparsPlayoutClient
====================

CasparsPlayoutClient is a playoutclient for the [CasparCG Server] [1] with focus on playout over CG.

This project is in an early development state and thus has an empty master branch till the first release.
See the testing branches for code.

Table of contents
-----------------

* [License](#license)
* [Features](#features)
* [Screenshots](#fcreenshots)
* [Quick Start](#quickstart)
* [Development](#development)


<a id="license"/>
License
-------

This software is licensed under the [Gnu GPL v3] [2].


<a id="features"/>
Features
--------

![Screenshot](/screenshots/cpc_playlist_all.jpg "CasparsPlayoutClient mainwindow")

CasparsPlayoutClient offers a hierarchical playlist system allowing 
* simple parallel rundowns
* simple sequential rundowns
* looping playlists
* autostart / manual start of playlists
* complex blocks of combined playlists
* feedback for every playlist (duration, position, remaining)
* single server multi channel support
* xml import/export of playlists
* searching in medialibrary
* drag and drop playlist editing
* custom playlist and media naming
* collapse/expand of subplaylists

<a id="screenshots"/>
Screenshots
-----------

The playlist

* The playlist tree:  
  ![The playlist tree](/screenshots/cpc_playlist.jpg "The playlist tree")
* A running playlist in expanded view:  
  ![A running playlist in expanded view](/screenshots/cpc_playlist_all_playing.jpg "A running playlist in expanded view")
* A running playlist in collapsed view:  
  ![A running playlist in collapsed view](/screenshots/cpc_playlist_all_playing_collapsed.jpg "A running playlist in collapsed view")
* A running playlist with manuel start waiting for start:  
  ![A running playlist with manuel start waiting for start](/screenshots/cpc_playlist_waiting.jpg "A running playlist with manuel start waiting for start")
* A running playlist with near end warning active:  
  ![A running playlist with near end warning active](/screenshots/cpc_playlist_nearEndWarn.jpg "A running playlist with near end warning active")


The medialibrary

* The media library view:  
  ![The media library view](/screenshots/cpc_medialib.jpg "The media library view")
* The media library with active filter:  
  ![The media library with active filter](/screenshots/cpc_medialib_filter.jpg "The media library with active filter")

<a id="quickstart"/>
Quick Start
-----------

The main window is splitted into 4 areas:  
1. The playlist (left)  
2. The media library (right)  
3. The command bar (bottum)  
4. The Info and CG controll (middle, not yet implemented)  

* **Connect**  
	First connect to your [CasparCG Server][1] at the command bar (3).
	Now, the media library (2) is loaded and you should see your mediafiles known by [CasparCG Server][1] in the list.

* **Add and remove playlist items**  
	Click and drag one of the media files to the playlist (1) and drop it there. This adds the media file to the playlist.
	If you want to delete a playlist item, right click somewhere on it (but not over a textbox) and select ***Remove item*** form the context menu. This will delete the playlist with all sub items.
	For adding a block to the playlist, right click on the playlist you want the block to belong to and choose ***Add block***.
 
* **Change and modify playlist items**  
	You can change the name of each item to a propper one. They don't need to be unique nor do they have to match the file name on the [Server][1].
	Each media file item musst have a valid channel and layer set in order to be playable. To change the channel, use the number picker beside the name box and the most right one for the layer.
	Block items dont need correct channel and layer settings. But if you set them, newly added subitems will inerhit these values saving you time.  
	There are two types of playlist:
	* parallel playlists:  
		each item of the playlist will be started together
	* sequential playlists:  
		each item starts after the previous has stopped  

	And there are 3 checkboxes:
	* par:  
	 	choose parallel playing / sequential playing
	* auto:  
	 	choose auto start / waiting for manual play an the playlist
	* loop:  
	 	choose whether or not, after the whole playlist has been played, it should be started again  

* **Moving playlist items**  
	You can move playlist items via drag and drop. Dropping a playlist onto an other let the dropped playlist taking the place of the underlaying.
	The underlaying will be moved below the dropped. So dropping the most upper playlist to the one after it has no effect.
 
* **Play / Stop / Abort**  
	There is only one controll button for start / stop / abort for each playlist.
	Each playlist could be in one of the three states giving the button it's function:
	* stopped:
		pressing play will start playback for this playlist and it's items
	* playing:
		pressing stop will stop the playlist and all it's items if they are not waiting
	* waiting:
		pressing play will start the waiting playlist to play
	* EVER:
		pressing the play / stop button while holding **CTRL** will abort the playlist no matter in what state it is

<a id="development"/>
Development
-----------

CasparsPlayoutClient is still under development and not released yet.
Please fell free to build it your self for testing.
Any contributions are welcome.
I'm sorry, but most comments are in German. I'll try to change them into english. 
The first development phase was under high time pressure, so some parts of the code are not or only hardly dokumented so far.

[1]: https://github.com/CasparCG/Server "CasparCG Server"
[2]: http://www.gnu.org/licenses/gpl-3.0-standalone.html "Gnu General Public License Version 3"
