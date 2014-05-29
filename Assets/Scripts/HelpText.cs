using UnityEngine;
using System.Collections;

public class HelpText : MonoBehaviour {
	UILabel m_Label;
	TweenAlpha m_AlphaTweener;

	void Awake() {
		m_Label = GetComponent<UILabel>();
		m_AlphaTweener = GetComponent<TweenAlpha>();
	}

	void Start() {
		Messenger.AddListener<bool>("enable help text", enableHelpText);
		Messenger.AddListener<string>("set help text", setHelpText);
	}

	public void enableHelpText(bool value) {
		if(m_AlphaTweener != null) {
			if(value) {
				m_AlphaTweener.PlayForward();
			} else {
				m_AlphaTweener.PlayReverse();
			}
		} else {
			gameObject.SetActive(value);
		}
	}

	public void setHelpText(string text) {
		if(m_Label != null) {
			m_Label.text = text;
		}
	}
}
