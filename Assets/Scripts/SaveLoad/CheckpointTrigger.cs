using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour {
	public Checkpoint r_CheckpointToTrigger;
	
	void OnTriggerEnter(Collider col){
		if(!Game.doesSavegameExist()){
			Game.createSavegame();
		}
		if(col.tag == "Player" && !Game.hasCheckpointBeenUsed(r_CheckpointToTrigger)){
			Debug.Log("Save checkpoint "+r_CheckpointToTrigger.getUniqueID());
			Game.setCurrentSavegameCheckpoint(r_CheckpointToTrigger.getUniqueID());
		}
	}
}
