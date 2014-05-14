using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to save inventory items for checkpoint
/// By: Aleksi Lindeman
/// </summary>
[System.Serializable]
public class InventoryItemSaver{
	[SerializeField] Behaviour_PickUp r_PickupInteractable;
	[SerializeField] int m_Amount;
	public InventoryItemSaver(){
		r_PickupInteractable = null;
		m_Amount = 0;
	}
	
	public Behaviour_PickUp getPickupItem(){
		return r_PickupInteractable;
	}
	
	public int getAmount(){
		return m_Amount;
	}
	
	public void setPickupItem(Behaviour_PickUp inter){
		r_PickupInteractable = inter;
	}
	
	public void setAmount(int amount){
		m_Amount = amount;
	}
}

/// <summary>
/// Class to have references to checkpoints, so that we can get checkpoint using its id
/// that has been saved into savegame file
/// By: Aleksi Lindeman
/// </summary>
public class Checkpoints{
	private static Dictionary<string, Checkpoint> m_Checkpoints = new Dictionary<string, Checkpoint>();
	
	public static void add(Checkpoint checkpoint){
		if(!m_Checkpoints.ContainsKey(checkpoint.getUniqueID())){
			m_Checkpoints.Add(checkpoint.getUniqueID(), checkpoint);
		}
	}
	
	public static void remove(Checkpoint checkpoint){
		m_Checkpoints.Remove(checkpoint.getUniqueID());
	}
	
	public static Checkpoint getCheckpointFromID(string id){
		if(m_Checkpoints.ContainsKey(id)){
			return m_Checkpoints[id];
		}
		return null;
	}
}

/// <summary>
/// Class that is used with CheckpointEditor to add objects to set state on, and items to add to inventory
/// when checkpoint is loaded from
/// By: Aleksi Lindeman
/// </summary>
public class Checkpoint : MonoBehaviour {
	[SerializeField] List<ObjectState> m_ObjectStates = new List<ObjectState>();
	[SerializeField] List<InventoryItemSaver> m_InventoryItems = new List<InventoryItemSaver>();
	[SerializeField] string m_UniqueID = "";
	[SerializeField] string m_SceneToLoad = "";
	[SerializeField] string m_LoadingMessage = "";
	[SerializeField] Vector3 m_SpawnPosition = new Vector3(0.0f, 0.0f, 0.0f);
	[SerializeField] Vector3 m_SpawnRotation = new Vector3(0.0f, 0.0f, 0.0f);
	
	// Use this for initialization
	void Start () {
		Checkpoints.add(this);
	}
	
	void OnTriggerEnter(Collider col){
		if(col.tag == "Player" && !Game.hasCheckpointBeenUsed(this)){
			Debug.Log("Save checkpoint "+m_UniqueID);
			Game.setCurrentSavegameCheckpoint(m_UniqueID);
		}
	}
	
	public void load(){
		foreach(ObjectState objectState in m_ObjectStates){
			if(objectState.getObject() && objectState.getObject().GetComponent<Interactable>()){
				objectState.getObject().GetComponent<Interactable>().setPuzzleState(objectState.getState());
			}
		}
		foreach(InventoryItemSaver inventoryItem in m_InventoryItems){
			if(inventoryItem.getPickupItem() != null){
				for(int i = 0; i < inventoryItem.getAmount(); ++i){
					InventoryLogic.Instance.addItem(inventoryItem.getPickupItem().m_ItemName, inventoryItem.getPickupItem().m_ItemThumbnail);
				}
			}
		}
		GUIManager.Instance.loadLevel(m_SceneToLoad, m_LoadingMessage);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = m_SpawnPosition;
		player.transform.rotation = Quaternion.Euler(m_SpawnRotation);
		PuzzleEvent.trigger("onCheckpointLoad", gameObject, false);
	}
	
	public List<ObjectState> getObjectStates(){
		return m_ObjectStates;
	}
	
	public List<InventoryItemSaver> getInventoryItems(){
		return m_InventoryItems;
	}
	
	public string getUniqueID(){
		return m_UniqueID;
	}
	
	public string getSceneToLoad(){
		return m_SceneToLoad;
	}
	
	public string getLoadingMessage(){
		return m_LoadingMessage;
	}
	
	public Vector3 getSpawnPosition(){
		return m_SpawnPosition;
	}
	
	public Vector3 getSpawnRotation(){
		return m_SpawnRotation;
	}
	
	public void setUniqueID(string uniqueID){
		m_UniqueID = uniqueID;
	}
	
	public void setSceneToLoad(string sceneToLoad){
		m_SceneToLoad = sceneToLoad;
	}
	
	public void setLoadingMessage(string loadingMessage){
		m_LoadingMessage = loadingMessage;
	}
	
	public void setSpawnPosition(Vector3 spawnPosition){
		m_SpawnPosition = spawnPosition;
	}
	
	public void setSpawnRotation(Vector3 spawnRotation){
		m_SpawnRotation = spawnRotation;
	}
}
