using UnityEngine;
using System.Collections;

/// <summary>
/// This scirpt is attached to a trigger object and when the player
/// enters the trigger zone, this script will trigger a level load.
/// 
/// Created by Simon
/// </summary>
public class LoadLevelTrigger : MonoBehaviour {
	[SerializeField][Tooltip("Which level to load when this zone is triggered.\nIf this field is left blank, this zone will reload the current level.")]
	private string m_LevelToLoad = "";
	private bool m_Used = false;
	public bool useNumber = false;
	public int sceneNumber;

	[Multiline] [SerializeField]
	private string m_LoadMessage;

	void Awake(){
		if( m_LevelToLoad.Trim() == "" ){
			m_LevelToLoad = Application.loadedLevelName;
		}
		Debug.Log ("LoadLevelTrigger Awake Function");
	}

	void OnTriggerEnter( Collider col ){
		if( col.tag == "Player" && !m_Used){
			if(useNumber){
				GUIManager.Instance.loadLevel( sceneNumber , m_LoadMessage );
				m_Used = true;
			}
			else{
				GUIManager.Instance.loadLevel( m_LevelToLoad , m_LoadMessage );
				m_Used = true;
			}
		}
	}
}
