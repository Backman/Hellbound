using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Class to draw inspector layout for CheckpointAutosave
/// </summary>
/// By: Aleksi Lindeman
[CustomEditor(typeof(CheckpointAutosave))]
public class Game : Editor {
	public void OnEnable(){
		hideFlags = HideFlags.HideAndDontSave;
	}
	
	public override void OnInspectorGUI(){
		CheckpointAutosave checkpoint = target as CheckpointAutosave;
		
		// Show text field with unique id, and save if changed
		if(!checkpoint.isIDUnique() || checkpoint.getID() == ""){
			GUI.backgroundColor = Color.red;
		}
		string uniqueID = EditorGUILayout.TextField("Unique ID", checkpoint.getDisplayID());
		checkpoint.setID(uniqueID);
		if(!checkpoint.isIDUnique() || checkpoint.getID() == ""){
			GUI.backgroundColor = Color.white;
		}
		
		// Show text area for description, and save if changed
		EditorGUILayout.PrefixLabel("Description");
		string description = EditorGUILayout.TextArea(checkpoint.getDescription());
		checkpoint.setDescription(description);
		
		// Show text fields for respawn position when loading save using this checkpoint
		Vector3 respawnPosition = EditorGUILayout.Vector3Field("Respawn Position", checkpoint.getRespawnPosition());
		// Show text fields for respawn rotation when loading save using this checkpoint
		Vector3 respawnRotation = EditorGUILayout.Vector3Field("Respawn Rotation", checkpoint.getRespawnRotation());
		checkpoint.setRespawnPosition(respawnPosition);
		checkpoint.setRespawnRotation(respawnRotation);
		
		
		EditorUtility.SetDirty(target);
	}
}
