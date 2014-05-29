using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns a specific GameObject at a position
/// when a GameObject with a specific tag enters it's collision box.
/// By Arvid Backman, edited by Simon Jonason
/// </summary>

public class SpawnObject : MonoBehaviour {
	[TooltipAttribute("The object that should be spawned. Note that this should be an object in the world, NOT a prefab.\n" +
					  "The object will be disabled upon startup and enabled when this zone is enterd")]
	public GameObject m_SpawnObject;
	[TooltipAttribute("If this variable is set, the object will be moved to this position when the trigger zone is entered")]
	public Transform m_Position;
	public string m_ColliderTag = "Player";
	public float m_Delay = 0.0f;

	private bool m_Triggered = false;

	void Awake() {
		if(m_SpawnObject == null){
			Debug.LogWarning("Warning. No GameObject assigned in " + gameObject.name +". Disabling self");
			gameObject.SetActive(false);
		} else {
			m_SpawnObject.SetActive (false);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag){
			if(m_SpawnObject != null && !m_Triggered){
				StartCoroutine(spawnObject());
				m_Triggered = true;
			}
		}
	}

	//This name is misleading. We don't spawn objects, we simply reenable them
	IEnumerator spawnObject() {
		yield return new WaitForSeconds(m_Delay);

		if( m_SpawnObject != null )
			m_SpawnObject.SetActive( true );

		if( m_Position != null )
			m_SpawnObject.transform.position = m_Position.transform.position;
	}
}
