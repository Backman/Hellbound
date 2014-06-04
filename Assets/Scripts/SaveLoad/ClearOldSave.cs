using UnityEngine;
using System.Collections;

public class ClearOldSave : MonoBehaviour {

	public void clearOldSaveData(){
		
		Game.createSavegame();
	}
}
