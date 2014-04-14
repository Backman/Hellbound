using UnityEngine;
using System.Collections;

public class MonologTrigger : MonoBehaviour {
	[Multiline]
	public string m_Text = "Enter text here";

	public bool m_ShouldDisplayText = true;
	public bool m_DoOnlyOnce = false;

	private bool m_HasBeenEntered = false;

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" && !m_HasBeenEntered){
			m_HasBeenEntered = m_DoOnlyOnce;

			if(m_ShouldDisplayText){
				GUIManager.Instance.simpleShowText(m_Text);
			}
		}
	}
}
