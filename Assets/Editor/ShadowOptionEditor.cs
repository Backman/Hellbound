#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Class to draw inspector layout for Checkpoint
/// </summary>
[CustomEditor(typeof(EditorNone))]
public class ShadowOptionEditor : Editor {

	public void OnEnable(){
		hideFlags = HideFlags.HideAndDontSave;
	}
	/*
	public override void OnInspectorGUI(){
		ShadowOption shadowOption = target as ShadowOption;
		List<string> bla = shadowOption.gameObject.GetComponent<UIPopupList>().items;
		NGUIEditorTools.SetLabelWidth(80f);
		foreach (string name in bla) {
			//float temp = GUI.skin.label.CalcSize(new GUIContent(name)).x;
			//GUILayout.Label(name, GUILayout.Width(temp));
			GUILayout.Label(new GUIContent(name));
		}
		EditorUtility.SetDirty(target);
	}
	*/
}
#endif
