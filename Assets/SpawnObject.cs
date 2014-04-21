using UnityEngine;
using System.Collections;

public class SpawnObject : MonoBehaviour {
	public GameObject m_SpawnObject;
	public Transform m_Position;
	public string m_ColliderTag = "Player";
	public float m_Delay = 0.0f;
	private bool m_Triggered = false;

	void Start() {
		if(m_Position == null){
			m_Position = transform;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag){
			if(m_SpawnObject != null && !m_Triggered){
				StartCoroutine(spawnObject());
				m_Triggered = true;
				Debug.Log (m_Triggered);
			}
		}
	}

	IEnumerator spawnObject() {
		yield return new WaitForSeconds(m_Delay);
		GameObject obj = (GameObject)Instantiate(m_SpawnObject, m_Position.position, m_Position.rotation);
	}
}
