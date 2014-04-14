using UnityEngine;
using UnityEditor;

/// <summary>
/// A more streamlined way of generating
/// Interactables in the game.
/// 
/// Created by Simon 3/4 -14
/// </summary>

public class InteractableTool : EditorWindow {
	Vector2		m_ScrollPosition = Vector2.zero;
	Vector2 	m_DescriptionScroll = Vector2.zero;

	bool  		m_FocusEnabled = true;
	Component	m_FocusScript  = null;

	bool 		m_ActivateEnabled  = false;
	Component	m_ActivateScript   = null;
	Component	m_ActReqConditions = null;
	Component	m_ActSetConditions = null;

	bool 		m_ExamineEnabled = true;
	bool 		m_HasDescription = false;
	string		m_Description 	 = "";
	Component	m_ExamineScript	 = null;
	Texture2D	m_ExamineImage	 = null;


	bool 		m_PickUp 		 = false;
	Component   m_PickUpItem	 = null;

	//Add a menu item named "MyWindow" to the Window Menu
	[MenuItem("Hellbound/New Interactable")]
	public static void ShowWindow(){
		//Show existing window instance. If one doesn't exist, create one.
		EditorWindow.GetWindow( typeof(InteractableTool) );
		EditorStyles.textField.wordWrap = true;
	}


	//TODO: Continue with the window layout.
	//TODO: Check with designers what an interactable requieres
	//TODO: Add scrollview to the textarea	
	void OnGUI(){
		minSize = new Vector2(320, 600);
		m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition); {
			GUILayout.Label("New Interactable", EditorStyles.boldLabel);
			GUILayout.Space(10);

			m_FocusEnabled = EditorGUILayout.BeginToggleGroup("Can be in focus", m_FocusEnabled); {
				m_FocusScript  = EditorGUILayout.ObjectField("Script", m_FocusScript, typeof(MonoBehaviour), false ) as Component; 
			} EditorGUILayout.EndToggleGroup();

			GUILayout.Space(10);

			m_ActivateEnabled = EditorGUILayout.BeginToggleGroup("Can be activated", m_ActivateEnabled);{
				m_ActivateScript  = EditorGUILayout.ObjectField("Script", m_ActivateScript, typeof(MonoBehaviour), false ) as Component; 	
				EditorGUI.BeginDisabledGroup(true); {	//TODO: This part is not done yet
					m_ActSetConditions  = EditorGUILayout.ObjectField("Sets conditions", m_ActSetConditions, typeof(MonoBehaviour), false ) as Component; 	
					m_ActReqConditions  = EditorGUILayout.ObjectField("Requires conditions", m_ActReqConditions, typeof(MonoBehaviour), false ) as Component; 	
				} EditorGUI.EndDisabledGroup();
			}EditorGUILayout.EndToggleGroup();

			GUILayout.Space(10);

			m_ExamineEnabled = EditorGUILayout.BeginToggleGroup("Can be examined", m_ExamineEnabled); {					
				m_ExamineScript = EditorGUILayout.ObjectField("Script", m_ExamineScript, typeof(MonoBehaviour)) as Component;
				m_HasDescription = EditorGUILayout.BeginToggleGroup( "Description" , m_HasDescription );{
					m_DescriptionScroll = EditorGUILayout.BeginScrollView( m_DescriptionScroll, GUILayout.Height(120));{
						m_Description = EditorGUILayout.TextArea( m_Description, GUILayout.Width(298) );
					} EditorGUILayout.EndScrollView();
				}EditorGUILayout.EndToggleGroup();

				m_ExamineImage = EditorGUILayout.ObjectField("Thumbnail:", m_ExamineImage, typeof(Texture2D)) as Texture2D;

			} EditorGUILayout.EndToggleGroup();
		} EditorGUILayout.EndScrollView();
	}	
}
















