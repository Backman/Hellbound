using UnityEngine;
using System.Collections;

public class KillNPCTrigger : MonoBehaviour {

	public string m_ColliderTag = "Player";

	public FMODAsset m_FMODAsset = null;
	public Transform m_EmitterPosition;

	[HideInInspector]
	public AlterSoundPlay m_Sound = null;

	public bool Used {
		get; set;
	}

	void Awake() {
		m_Sound = GetComponent<AlterSoundPlay>();
		if(m_EmitterPosition == null) {
			m_EmitterPosition = transform;
		}
		Used = false;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == m_ColliderTag && !Used) {
			PuzzleEvent.trigger ("onTriggerEnter", gameObject, true);
		}
	}
}
