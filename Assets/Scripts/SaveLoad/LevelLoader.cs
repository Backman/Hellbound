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

				
				player.transform.position = new Vector3(gameData.spawnPosition.x, gameData.spawnPosition.y, gameData.spawnPosition.z);
				player.transform.rotation = Quaternion.Euler(0.0f, gameData.spawnRotation, 0.0f);
				
				List<SerializablePair<int, string>> interactableStates = gameData.interactableStates;
				List<string> inventoryItems = gameData.inventoryItems;

				foreach(SerializablePair<int, string> stateSaver in interactableStates){
					Interactable inter = GameData.getInteractableFromID(stateSaver.first);

					inter.setPuzzleState(stateSaver.second);
				}

				foreach(string inventoryItem in inventoryItems){
					UISprite sprite = InventoryThumbnailDatabase.getThumbnail(inventoryItem);
					if(sprite){
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
