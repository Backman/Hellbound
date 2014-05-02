using UnityEngine;
using System.Collections;

/// <summary>
/// This scirpt is attached to a trigger object and when the player
/// enters the trigger zone, this script will trigger a level load.
/// 
/// Created by Simon
/// </summary>
public class LoadLevelTrigger : MonoBehaviour {
	[SerializeField]
	private string m_LevelToLoad;

	[Multiline] [SerializeField]
	private string m_LoadMessage;

	void OnTriggerEnter( Collider col ){
		if( col.tag == "Player" ){
			LoadingLogic.Instance.loadLevel( m_LevelToLoad, m_LoadMessage );
			gameObject.SetActive(false);
		}
	}
}
