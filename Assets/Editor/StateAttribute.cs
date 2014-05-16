#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(State<Interactable>))]
public class StateAttribute : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PrefixLabel(position, label);
		EditorGUI.PropertyField(position, property.FindPropertyRelative("m_Text"));
	}

}
#endif