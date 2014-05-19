using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour {
	public Checkpoint r_CheckpointToTrigger;

	void Awake() {
		if(r_CheckpointToTrigger){
			Vector3 pos = gameObject.transform.position;
			pos.y += 1.0f;
			r_CheckpointToTrigger.setSpawnPosition(pos);
		}
	}
	
	void OnTriggerEnter(Collider col){
		// For debugging usage. Doesn't affect release version.
		if(!Game.doesSavegameExist()){
			Game.createSavegame();
		}
		if(col.tag == "Player" && !Game.hasCheckpointBeenUsed(r_CheckpointToTrigger)){
			Debug.Log("Save checkpoint "+r_CheckpointToTrigger.getUniqueID());
			Game.setCurrentSavegameCheckpoint(r_CheckpointToTrigger.getUniqueID());
		}
	}
}
