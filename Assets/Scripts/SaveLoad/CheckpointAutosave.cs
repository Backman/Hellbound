using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoints{
	private static List<CheckpointAutosave> m_Checkpoints = new List<CheckpointAutosave>();
	
	public static void add(CheckpointAutosave checkpoint){
		// Only add checkpoint to list if it doesn't already exist in the list
		if(!m_Checkpoints.Contains(checkpoint)){
			m_Checkpoints.Add(checkpoint);
			//Debug.Log("Added checkpoint, current checkpoints: "+m_Checkpoints.Count);
		}
	}
	
	public static CheckpointAutosave getCheckpointFromID(string id){
		foreach(CheckpointAutosave checkpoint in m_Checkpoints){
			if(checkpoint.getID() == id){
				return checkpoint;
			}
		}
		return null;
	}
}

[ExecuteInEditMode]
public class CheckpointAutosave : MonoBehaviour {
	[SerializeField] string m_Description = "";
	[SerializeField] string m_ID = "";
	[SerializeField] string m_DisplayID = "";
	[SerializeField] Vector3 m_RespawnPosition, m_RespawnRotation;
	[SerializeField] bool m_UniqueID = false;
	//[SerializeField] List<InventoryItem> m_InventoryItems = new List<InventoryItem>();
	
	void OnEnable(){
		Checkpoints.add(gameObject.GetComponent<CheckpointAutosave>());
	}
	
	public string getDescription(){
		return m_Description;
	}
	
	public Vector3 getRespawnPosition(){
		return m_RespawnPosition;
	}
	
	public Vector3 getRespawnRotation(){
		return m_RespawnRotation;
	}
	
	public string getDisplayID(){
		return m_DisplayID;
	}
	
	public string getID(){
		return m_ID;
	}
	
	public bool isIDUnique(){
		return m_UniqueID;
	}
	
	public void setDescription(string description){
		m_Description = description;
	}
	
	public void setRespawnPosition(Vector3 respawnPosition){
		m_RespawnPosition = respawnPosition;
	}
	
	public void setRespawnRotation(Vector3 respawnRotation){
		m_RespawnRotation = respawnRotation;
	}
	
	public void setID(string id){
		if(id == ""){
			m_DisplayID = id;
			m_UniqueID = false;
		}
		else{
			CheckpointAutosave checkpoint = Checkpoints.getCheckpointFromID(id);
			if(checkpoint == null || checkpoint == this){
				m_ID = id;
				m_DisplayID = id;
				m_UniqueID = true;
			}
			else if(checkpoint != null && checkpoint != this){
				m_DisplayID = id;
				m_UniqueID = false;
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "Interactable Detector Zone"){
			Debug.Log("Hit checkpoint: "+m_ID);
			//Debug.Log("Previous inventory count: "+m_InventoryItems.Count);
			//m_InventoryItems = Inventory.getInstance().getInventoryItems();
			/*
			GameData saveData = new GameData();
			saveData.addInventoryItems(Inventory.getInstance().getInventoryItems());
			Game.save(saveData, "test");
			*/
		}
	}
}
