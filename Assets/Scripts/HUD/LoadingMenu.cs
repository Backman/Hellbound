using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingMenu : MonoBehaviour {
	public void load(){
		// This check should also be used in StartMenu initialization to change UI 
		// depending on if a valid savegame exists
		if(Game.doesSavegameExist()){
			Game.load();
		}
	}
}
