using UnityEngine;
using System.Collections;

public class LoadLastCheckpointTrigger : MonoBehaviour {
	public string m_LoadingMessage = "";
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			GUIManager.Instance.loadLastCheckPoint(m_LoadingMessage);
		}
	}
}
