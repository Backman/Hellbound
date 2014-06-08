using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingMenu : MonoBehaviour {
	public void load(){
		// This check should also be used in StartMenu initialization to change UI 
		// depending on if a valid savegame exists
		if(Game.doesSavegameExist()){

			if( Application.loadedLevel != 0 ) GUIManager.Instance.togglePause();
			Game.load();

		}
	}
}
