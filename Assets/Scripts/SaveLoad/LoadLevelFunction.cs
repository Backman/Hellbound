using UnityEngine;
using System.Collections;
//peter
public class LoadLevelFunction : MonoBehaviour {
	public string SceneName;
	public void LoadLevel(string name){
		if (Application.CanStreamedLevelBeLoaded (name)) {
			Application.LoadLevel (name);
		} 
		else {
			Debug.LogWarning ("You cant load this name or scene.");
		}
	}
	public void LoadLevel(){
		Game.createSavegame();
		LoadLevel(SceneName);
	}
}
