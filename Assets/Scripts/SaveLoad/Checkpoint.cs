using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to have references to checkpoints, so that we can get checkpoint using its id
/// that has been saved into savegame file
/// By: Aleksi Lindeman
/// </summary>
public class Checkpoints{
	private static Dictionary<string, Checkpoint> m_Checkpoints = new Dictionary<string, Checkpoint>();
	
	public static bool add(Checkpoint checkpoint){
		if(!m_Checkpoints.ContainsKey(checkpoint.getUniqueID())){
			m_Checkpoints.Add(checkpoint.getUniqueID(), checkpoint);
			return true;
		}
		return false;
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
	[SerializeField] string m_UniqueID = "";
	[SerializeField] string m_SceneToLoad = "";
	[SerializeField] string m_LoadingMessage = "";
	[SerializeField] Vector3 m_SpawnPosition = new Vector3(0.0f, 0.0f, 0.0f);
	[SerializeField] float m_SpawnRotation = 0.0f;
	private bool m_LoadingThisCheckpoint = false;
	private bool m_UseThis = false; // debug
	
	// Use this for initialization
	void Start () {
		setUniqueID(gameObject.name);
		if(Checkpoints.add(this)){
			m_UseThis = true;
			//Debug.Log("Add checkpoint: "+m_UniqueID);
			Messenger.AddListener<int>("OnLevelWasLoaded", onLevelWasLoaded);
		}
	}
	
	public void start(){
		Start();
	}
	
	void onLevelWasLoaded(int level){
		if(m_LoadingThisCheckpoint){
			UIRoot uiRoot = FindObjectOfType<UIRoot>();
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if(uiRoot && player){
				Debug.Log("Loaded level: "+level+", checkpoint: "+m_UniqueID);
				
				player.transform.position = m_SpawnPosition;
				player.transform.rotation = Quaternion.Euler(0.0f, m_SpawnRotation, 0.0f);
				
				List<SerializablePair<int, string>> interactableStates = Game.getGameData().interactableStates;
				List<string> inventoryItems = Game.getGameData().inventoryItems;
				//Debug.Log("Num interactables: "+interactableStates.Count);
				//Debug.Log("Num available interactables: "+GameObject.FindObjectsOfType<Interactable>().Length);
				foreach(SerializablePair<int, string> stateSaver in interactableStates){
					Interactable inter = Game.getGameData().getInteractableFromID(stateSaver.first);
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
				PuzzleEvent.trigger("onCheckpointLoaded", gameObject, false);
			}
			m_LoadingThisCheckpoint = false;
		}
	}
	
	public void load(){
		//LoadingLogic.Instance.loadLevel(m_SceneToLoad, m_LoadingMessage);
		//GUIManager.Instance.loadLevel(m_SceneToLoad, m_LoadingMessage);
		m_LoadingThisCheckpoint = true;
		if(/*Application.loadedLevelName != m_SceneToLoad && */Application.CanStreamedLevelBeLoaded(m_SceneToLoad)){
			Application.LoadLevel(m_SceneToLoad);
		} 
		else{
			Debug.LogWarning ("You cant load this name or scene.");
		}
	}
	
	public void saveStates(){
		List<string> inventoryItems = Game.getGameData().inventoryItems;
		inventoryItems.Clear();
		foreach(string item in InventoryLogic.Instance.getItems()){
			inventoryItems.Add(item);
		}
		//inventoryItems.Add("CubeKeyA");
		//Debug.Log("Inventory items: "+inventoryItems.Count);
		
		List<SerializablePair<int, string>> interactableStateData = Game.getGameData().interactableStates;
		interactableStateData.Clear();
		int idx = 0;
		foreach(Interactable inter in GameObject.FindObjectsOfType<Interactable>()){
			//Debug.Log("Saving: "+inter+", state: "+inter.getPuzzleState());
			//Debug.Log("id: "+idx);
			interactableStateData.Add(new SerializablePair<int, string>(idx++, inter.getPuzzleState()));
		}
	}
	
	void OnTriggerEnter(Collider col){
		Vector3 pos = gameObject.transform.position;
		pos.y += 1.0f;
		setSpawnPosition(pos);
		setSceneToLoad(Application.loadedLevelName);
		setUniqueID(gameObject.name);
		// For debugging usage. Doesn't affect release version.
		Debug.Log("Use this? "+m_UseThis.ToString());
		if(!Game.doesSavegameExist()){
			Game.createSavegame();
		}
		//
		if(col.tag == "Player" && !Game.hasCheckpointBeenUsed(gameObject.GetComponent<Checkpoint>())){
			Debug.Log("Save checkpoint "+getUniqueID());
			saveStates();
			Game.setCurrentSavegameCheckpoint(getUniqueID());
			Game.save();
		}
	}
	
	public string getUniqueID(){
		return m_UniqueID;
	}
	
	public string getSceneToLoad(){
		//return Application.loadedLevelName;
		return m_SceneToLoad;
	}
	
	public string getLoadingMessage(){
		return m_LoadingMessage;
	}
	
	public Vector3 getSpawnPosition(){
		return m_SpawnPosition;
	}
	
	public float getSpawnRotation(){
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
	
	public void setSpawnRotation(float spawnRotation){
		m_SpawnRotation = spawnRotation;
	}
}
