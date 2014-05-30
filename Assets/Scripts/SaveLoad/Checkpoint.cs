using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {
	[SerializeField] string m_LoadingMessage = "";
	[SerializeField] float m_SpawnRotation = 0.0f;
	
	public void saveStates(){
		List<string> inventoryItems = Game.getGameData().inventoryItems;
		inventoryItems.Clear();
		foreach(string item in InventoryLogic.Instance.getItems().Keys){
			inventoryItems.Add(item);
		}
		
		List<SerializablePair<int, string>> interactableStateData = Game.getGameData().interactableStates;
		interactableStateData.Clear();
		int idx = 0;
		foreach(Interactable inter in GameObject.FindObjectsOfType<Interactable>()){
			interactableStateData.Add(new SerializablePair<int, string>(idx++, inter.getPuzzleState()));
		}
	}
	
	void OnTriggerEnter(Collider col){
		// For debugging usage. Doesn't affect release version.
		if(Game.doesSavegameExist()){
			Game.load(false);
		}else{
			Game.createSavegame();
		}
		if(col.tag == "Player" && !Game.hasCheckpointBeenUsed(gameObject.GetComponent<Checkpoint>())){
			
			Vector3 pos = gameObject.transform.position;
			pos.y += 1.0f;
			
			GameData gameData = Game.getGameData();
			gameData.levelToLoad = Application.loadedLevelName;
			gameData.loadingMessage = m_LoadingMessage;
			gameData.spawnPosition = new SVector3(pos);
			gameData.spawnRotation = m_SpawnRotation;
			
			saveStates();
			Game.setCurrentSavegameCheckpoint(getUniqueID());
			Game.save();
		}
	}
	
	public string getUniqueID(){
		return gameObject.name;
	}
	
	public string getLoadingMessage(){
		return m_LoadingMessage;
	}
	
	public float getSpawnRotation(){
		return m_SpawnRotation;
	}
	
	public void setLoadingMessage(string loadingMessage){
		m_LoadingMessage = loadingMessage;
	}
	
	public void setSpawnRotation(float spawnRotation){
		m_SpawnRotation = spawnRotation;
	}
}
