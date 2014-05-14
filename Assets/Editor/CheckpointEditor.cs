using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Class to draw inspector layout for Checkpoint
/// </summary>
[CustomEditor(typeof(Checkpoint))]
public class CheckpointEditor : Editor {
	private int mIndex = 0;
	
	public void OnEnable(){
		hideFlags = HideFlags.HideAndDontSave;
	}
	
	public override void OnInspectorGUI(){
		Checkpoint checkpoint = target as Checkpoint;
		List<ObjectState> objectStates = checkpoint.getObjectStates();
		List<InventoryItemSaver> inventoryItems = checkpoint.getInventoryItems();
		
		NGUIEditorTools.SetLabelWidth(80f);
		NGUIEditorTools.DrawSeparator();
		
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Unique ID", GUILayout.Width(100f));
			string uniqueID = EditorGUILayout.TextField("", checkpoint.getUniqueID());
			checkpoint.setUniqueID(uniqueID);
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Scene to load", GUILayout.Width(100f));
			string sceneToLoad = EditorGUILayout.TextField("", checkpoint.getSceneToLoad());
			checkpoint.setSceneToLoad(sceneToLoad);
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("Loading message", GUILayout.Width(150f));
		string loadingMessage = EditorGUILayout.TextArea(checkpoint.getLoadingMessage());
		checkpoint.setLoadingMessage(loadingMessage);
		
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Spawn position", GUILayout.Width(100f));
			Vector3 spawnPosition = EditorGUILayout.Vector3Field("", checkpoint.getSpawnPosition());
			checkpoint.setSpawnPosition(spawnPosition);
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Spawn rotation", GUILayout.Width(100f));
			Vector3 spawnRotation = EditorGUILayout.Vector3Field("", checkpoint.getSpawnRotation());
			checkpoint.setSpawnRotation(spawnRotation);
		}
		GUILayout.EndHorizontal();
		
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add object")){
			objectStates.Add(new ObjectState());
		}
		GUI.backgroundColor = Color.white;
		
		int index = 0;
		foreach(ObjectState objectState in objectStates){
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Set", GUILayout.Width(30f));
				Interactable inter = null;
				if(objectState.getObject() != null){
					inter = objectState.getObject().GetComponent<Interactable>();
				}
				Interactable obj = (Interactable)EditorGUILayout.ObjectField(inter, typeof(Interactable));
				GUILayout.Label("'s state to", GUILayout.Width(60f));
				string state = EditorGUILayout.TextField("", objectState.getState());
				
				GUI.backgroundColor = Color.red;
				if(GUILayout.Button("Delete")){
					objectStates.RemoveAt(index);
					GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					break;
				}
				GUI.backgroundColor = Color.white;
				
				objectState.setObject(obj == null ? null : obj.gameObject);
				objectState.setState(state);
			}
			GUILayout.EndHorizontal();
			++index;
		}
		
		NGUIEditorTools.DrawSeparator();
		
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add inventory item")){
			inventoryItems.Add(new InventoryItemSaver());
		}
		GUI.backgroundColor = Color.white;
		
		index = 0;
		foreach(InventoryItemSaver inventoryItem in inventoryItems){
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Pickup item", GUILayout.Width(80f));
				Behaviour_PickUp pickupItem = (Behaviour_PickUp)EditorGUILayout.ObjectField(inventoryItem.getPickupItem(), typeof(Behaviour_PickUp));
				GUILayout.Label("amount", GUILayout.Width(60f));
				int amount = EditorGUILayout.IntField(inventoryItem.getAmount(), GUILayout.Width(50f));
				
				GUI.backgroundColor = Color.red;
				if(GUILayout.Button("Delete")){
					inventoryItems.RemoveAt(index);
					GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					break;
				}
				GUI.backgroundColor = Color.white;
				
				inventoryItem.setPickupItem(pickupItem);
				inventoryItem.setAmount(amount);
			}
			GUILayout.EndHorizontal();
			++index;
		}
		
		// I think this is used to force unity to save(serialize)... or something?
		EditorUtility.SetDirty(target);
	}
}
