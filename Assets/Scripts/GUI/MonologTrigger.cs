using UnityEngine;
using System.Collections;

public class MonologTrigger : MonoBehaviour {
	[Multiline]
	public string m_Text = "Enter text here";

	[SerializeField][Range (0, 5)]
	public float m_NextFrameSpeed = 0.0f;

	public bool m_ShouldDisplayText = true;
	public bool m_DoOnlyOnce = false;

	private bool m_HasBeenEntered = false;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" && !m_HasBeenEntered){
			m_HasBeenEntered = m_DoOnlyOnce;

			if(m_ShouldDisplayText){
				GUIManager.Instance.simpleShowTextAutoScroll(m_Text, m_NextFrameSpeed);
			}
		}
	}
}
