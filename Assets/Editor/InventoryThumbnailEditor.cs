using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Class to draw inspector layout for InventoryThumbnailDatabase
/// </summary>
[CustomEditor(typeof(InventoryThumbnailDatabase))]
public class InventoryThumbnailEditor : Editor {
	public void OnEnable(){
		hideFlags = HideFlags.HideAndDontSave;
	}
	
	public override void OnInspectorGUI(){
		InventoryThumbnailDatabase thumbnailDatabase = target as InventoryThumbnailDatabase;
		List<InventoryThumbnailData> thumbnailData = thumbnailDatabase.getThumbnailData();
		
		NGUIEditorTools.SetLabelWidth(80f);
		NGUIEditorTools.DrawSeparator();
		
		GUI.backgroundColor = Color.green;
		if(GUILayout.Button("Add thumbnail")){
			thumbnailData.Add(new InventoryThumbnailData());
		}
		GUI.backgroundColor = Color.white;
		
		int index = 0;
		foreach(InventoryThumbnailData inventoryThumbnailData in thumbnailData){
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Name", GUILayout.Width(40f));
				string name = EditorGUILayout.TextField("", inventoryThumbnailData.getName());
				GUILayout.Label("Sprite", GUILayout.Width(50f));
				UISprite sprite = (UISprite)EditorGUILayout.ObjectField(inventoryThumbnailData.getSprite(), typeof(UISprite));
				
				GUI.backgroundColor = Color.red;
				if(GUILayout.Button("Delete")){
					thumbnailData.RemoveAt(index);
					GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					break;
				}
				GUI.backgroundColor = Color.white;
				
				inventoryThumbnailData.setName(name);
				inventoryThumbnailData.setSprite(sprite);
			}
			GUILayout.EndHorizontal();
			++index;
		}
		
		// I think this is used to force unity to save(serialize)... or something?
		EditorUtility.SetDirty(target);
	}
}
