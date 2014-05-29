using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {
	private bool m_TriggerOnNextUpdate = false;

	void OnLevelWasLoaded(int level){
		if(Game.isLoadingLevel()){
			UIRoot uiRoot = FindObjectOfType<UIRoot>();
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if(uiRoot && player){
				GameData gameData = Game.getGameData();
				
				Debug.Log("Loaded level: "+level+", checkpoint: "+gameData.currentCheckpointID);
				
				player.transform.position = new Vector3(gameData.spawnPosition.x, gameData.spawnPosition.y, gameData.spawnPosition.z);
				player.transform.rotation = Quaternion.Euler(0.0f, gameData.spawnRotation, 0.0f);
				
				List<SerializablePair<int, string>> interactableStates = gameData.interactableStates;
				List<string> inventoryItems = gameData.inventoryItems;
				//Debug.Log("Num interactables: "+interactableStates.Count);
				//Debug.Log("Num available interactables: "+GameObject.FindObjectsOfType<Interactable>().Length);
				foreach(SerializablePair<int, string> stateSaver in interactableStates){
					Interactable inter = GameData.getInteractableFromID(stateSaver.first);
					//Debug.Log("inter: "+inter+" state: "+stateSaver.second);
					inter.setPuzzleState(stateSaver.second);
				}
				//Debug.Log("Inventory items: "+inventoryItems.Count);
				foreach(string inventoryItem in inventoryItems){
					UISprite sprite = InventoryThumbnailDatabase.getThumbnail(inventoryItem);
					if(sprite){
						//Debug.Log("Add "+inventoryItem);
						InventoryLogic.Instance.addItem(inventoryItem, sprite);
					}
				}
			}
			else{
				Debug.Log("OnLevelWasLoaded: UIRoot and Player has not been initialized yet.");
			}
			m_TriggerOnNextUpdate = true;
			Game.setLoadingLevel(false);
		}
	}

	void LateUpdate(){
		if(m_TriggerOnNextUpdate){
			PuzzleEvent.trigger("onCheckpointLoaded", gameObject, false);
			m_TriggerOnNextUpdate = false;
		}
	}
}
