/*
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Tool for creating interactable objects
/// </summary>

public class CreateInteractable : EditorWindow {
	[MenuItem("Hellbound/New Interactable Object")]
	public static void ShowWindow(){
		//Show existing window instance. If one doesn't exist, create one.
		EditorWindow.GetWindow( typeof(CreateInteractable) );
		EditorStyles.textField.wordWrap = true;
	}

	string m_Name;
	
	Vector2		m_ScrollPosition = Vector2.zero;
	Vector2 	m_DescriptionScroll = Vector2.zero;
	
	bool 		m_ActivateEnabled  = false;
	
	bool 		m_ExamineEnabled = true;
	bool 		m_HasDescription = false;
	string		m_Description 	 = "";

	string 		m_IconName;

	GameObject 	m_Object;
	Texture2D	m_ExamineImage	 = null;
	
	bool 		m_PickUp 		 = false;

	UIAtlas m_Atlas;

	#region Helper functions
	/// <summary>
	/// Draw an enlarged sprite within the specified texture atlas.
	/// </summary>
	
	public Rect DrawSprite (Texture2D tex, Rect sprite, Material mat, bool addPadding)
	{
		return DrawSprite(tex, sprite, mat, addPadding, 0);
	}
	
	/// <summary>
	/// Draw an enlarged sprite within the specified texture atlas.
	/// </summary>
	
	public Rect DrawSprite (Texture2D tex, Rect sprite, Material mat, bool addPadding, int maxSize)
	{
		float paddingX = addPadding ? 4f / tex.width : 0f;
		float paddingY = addPadding ? 4f / tex.height : 0f;
		float ratio = (sprite.height + paddingY) / (sprite.width + paddingX);
		
		ratio *= (float)tex.height / tex.width;
		
		// Draw the checkered background
		Color c = GUI.color;
		Rect rect = NGUIEditorTools.DrawBackground(tex, ratio);
		GUI.color = c;
		
		if (maxSize > 0)
		{
			float dim = maxSize / Mathf.Max(rect.width, rect.height);
			rect.width *= dim;
			rect.height *= dim;
		}
		
		// We only want to draw into this rectangle
		if (Event.current.type == EventType.Repaint)
		{
			if (mat == null)
			{
				GUI.DrawTextureWithTexCoords(rect, tex, sprite);
			}
			else
			{
				// NOTE: DrawPreviewTexture doesn't seem to support BeginGroup-based clipping
				// when a custom material is specified. It seems to be a bug in Unity.
				// Passing 'null' for the material or omitting the parameter clips as expected.
				UnityEditor.EditorGUI.DrawPreviewTexture(sprite, tex, mat);
				//UnityEditor.EditorGUI.DrawPreviewTexture(drawRect, tex);
				//GUI.DrawTexture(drawRect, tex);
			}
			rect = new Rect(sprite.x + rect.x, sprite.y + rect.y, sprite.width, sprite.height);
		}
		return rect;
	}
	#endregion

	//TODO: Continue with the window layout.
	//TODO: Check with designers what an interactable requieres
	//TODO: Add scrollview to the textarea	
	void OnGUI(){
		//NGUIEditorTools.SetLabelWidth(80f);
		NGUIEditorTools.DrawSeparator();


		m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition); {
			GUILayout.Label("New Interactable", EditorStyles.boldLabel);
			GUILayout.Space(10);
			
			m_Atlas = EditorGUILayout.ObjectField("Icon Atlas", m_Atlas, typeof(UIAtlas), false) as UIAtlas;
			
			GUILayout.Space(10);
			NGUIEditorTools.DrawSeparator();
			GUILayout.Space(10);

			GUILayout.BeginHorizontal(); {
				m_Name = EditorGUILayout.TextField("Item Name", m_Name);
				
				GUI.backgroundColor = Color.red;

				GUI.backgroundColor = Color.white;

			} GUILayout.EndHorizontal();

			m_Description = GUILayout.TextArea(m_Description, 200, GUILayout.Height(100f));

			float iconSize = 64f;
			bool drawIcon = false;
			float extraSpace = 0f;

			if(m_Atlas != null) {
				BetterList<string> sprites = m_Atlas.GetListOfSprites();
				sprites.Insert(0, "<None>");

				string spriteName = (m_IconName != null) ? m_IconName : sprites[0];
				int index = 0;
				if (!string.IsNullOrEmpty(spriteName)) {
					for (int i = 1; i < sprites.size; ++i) {
						if (spriteName.Equals(sprites[i], System.StringComparison.OrdinalIgnoreCase)) {
							index = i;
							break;
						}
					}
				}

				// Draw the sprite selection popup
				index = EditorGUILayout.Popup("Icon", index, sprites.ToArray());
				UISpriteData sprite = (index > 0) ? m_Atlas.GetSprite(sprites[index]) : null;
				if (sprite != null) {
					m_IconName = sprite.name;
					
					Material mat = m_Atlas.spriteMaterial;
					
					if (mat != null) {
						Texture2D tex = mat.mainTexture as Texture2D;
						
						if (tex != null) {
							drawIcon = true;
							Rect rect = new Rect(sprite.x, sprite.y, sprite.width, sprite.height);
							rect = NGUIMath.ConvertToTexCoords(rect, tex.width, tex.height);
							
							GUILayout.Space(4f);
							GUILayout.BeginHorizontal(); {
								GUILayout.Space(Screen.width - iconSize);
								DrawSprite(tex, rect, null, false);
							} GUILayout.EndHorizontal();
							
							extraSpace = iconSize * (float)sprite.height / sprite.width;
						}
					}
				}
			}

			// Game Object attachment field, left of the icon
			GUILayout.BeginHorizontal();{
				m_Object = (GameObject)EditorGUILayout.ObjectField("Attachment", m_Object, typeof(GameObject), false);
				if (drawIcon) {
					GUILayout.Space(iconSize);
				}
			}GUILayout.EndHorizontal();

			// Calculate the extra spacing necessary for the icon to show up properly and not overlap anything
			if (drawIcon) {
				extraSpace = Mathf.Max(0f, extraSpace - 60f);
				GUILayout.Space(extraSpace);
			}

		} EditorGUILayout.EndScrollView();
	}
}
#endif
*/