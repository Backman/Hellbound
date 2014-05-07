using UnityEngine;
using System;
using System.Collections;

public class MonologTrigger : MonoBehaviour {

	public MyGUI.SubtitlesSettings[] m_Subtitles;

	private bool m_ShouldDisplayText = true;
	private bool m_HasBeenEntered = false;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" && !m_HasBeenEntered){
			m_HasBeenEntered = true;
			GUIManager.Instance.showSubtitles( m_Subtitles );
		}
	}
}
