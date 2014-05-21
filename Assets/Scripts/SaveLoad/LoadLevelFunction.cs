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
		} 
		else {
			Debug.LogWarning ("You cant load this name or scene.");
		}
	}

	public void LoadLevel(int index){
		if (Application.CanStreamedLevelBeLoaded (index)) {
			Application.LoadLevel(index);
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
}
