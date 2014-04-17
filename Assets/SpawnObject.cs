using UnityEngine;
using System.Collections;

public class SpawnObject : MonoBehaviour {
	public GameObject m_SpawnObject;
	public Transform m_Position;
	public string m_ColliderTag = "Player";
	private bool m_Triggered = false;

	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag){
			if(m_SpawnObject != null && !m_Triggered){
				GameObject obj = (GameObject)Instantiate(m_SpawnObject, m_Position.position, m_Position.rotation);
				m_Triggered = true;
			}
		}
	}
}
