#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
/// <summary>
/// Tooltip drawer.
/// 
/// A small utility for drawing tooltips if variables are hovered
/// 
/// Created by Simon Jonasson, inspired by 
/// </summary>
[CustomPropertyDrawer(typeof(TooltipAttribute))]
public class TooltipDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		
		var atr = (TooltipAttribute) attribute;
		var content = new GUIContent(label.text, atr.text);

		EditorGUI.PropertyField(position, prop, content, true);

	}

	
}
#endif