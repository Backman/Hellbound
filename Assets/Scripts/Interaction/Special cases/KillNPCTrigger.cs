using UnityEngine;
using System.Collections;

/// <summary>
/// This script will trigger an event that
/// when the player enters it's collider
/// By Arvid Backman
/// </summary>

public class KillNPCTrigger : MonoBehaviour {

	public string m_ColliderTag = "Player";

	public FMODAsset m_FMODAsset = null;
	public GameObject m_Coffin;

	[HideInInspector]
	public AlterSoundPlay m_Sound = null;
	
	void Awake() {
		m_Sound = GetComponent<AlterSoundPlay>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == m_ColliderTag) {
			PuzzleEvent.trigger ("onTriggerEnter", gameObject, true);
		}
	}
}
