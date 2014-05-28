using UnityEngine;
using System.Collections;
//peter
public class LoadLevelFunction : MonoBehaviour {
	public string SceneName;
	[TooltipAttribute("This variable changes the behaviour of this script to load levels via their level index instead of their level name.\n" +
					  "A levels index is decided by the number next to it after it has been added in the Build Settings menue (File -> Build Settings...)")]
	public bool UseLevelIndexInstead = false;
	[TooltipAttribute("This variable is only used of the UseLevelIndexInstead bool is set to true")]
	public int SceneIndex;

	public void LoadLevel(string name){
		if (Application.CanStreamedLevelBeLoaded (name)) {
			Application.LoadLevel (name);
			Messenger.Cleanup();
		} 
		else {
			Debug.LogWarning ("You cant load this name or scene.");
		}
	}

	public void LoadLevel(int index){
		if (Application.CanStreamedLevelBeLoaded (index)) {
			Application.LoadLevel(index);
			Messenger.Cleanup();
		} 
		else {
			Debug.LogWarning ("You cant load this name or scene.");
		}
	}

	public void LoadLevel(){
		Game.createSavegame();
		if( !UseLevelIndexInstead ){
			LoadLevel(SceneName);
		} else {
			LoadLevel(SceneIndex);
		}
	}

	public void delayLoad(){
		// Adds/Removes blur on the game view and shows/hides the PauseMenu UI widgets
		/*bool show = false;
		if (show) m_PauseWindow.r_MainWindow.GetComponent<Menu>().show ();
		m_PauseWindow.r_MainWindow.GetComponent<UIPlayTween>().Play(show);
		if(!show) m_PauseWindow.r_MainWindow.GetComponent<Menu>().dontShow();
		
		PauseGameEffect pge = r_MainCamera.GetComponent(typeof( PauseGameEffect ) ) as PauseGameEffect;
		if( pge != null ){
			pge.StopCoroutine("pauseGame");
			pge.StartCoroutine("pauseGame", show);
			PauseMenu.getInstance().hideAll();	//If this row is removed, we will display hints when we open the pause menu the first time
		} else {
			Debug.LogError("Error! No PauseGameEffect present. Are you using the correct PlayerController?\nIf you are, the PlayerController prefab is blue while you are in edit mode, otherwize it is red");
		}*/
		LoadLevel ();
	}
}
