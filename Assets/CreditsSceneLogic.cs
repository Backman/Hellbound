using UnityEngine;
using System.Collections;

public class CreditsSceneLogic : MonoBehaviour {

	public string m_LevelToLoad;
	public UIScrollView m_CreditsView;
	public UITweener m_FadeTweener;
	private UIProgressBar m_CreditsViewScroll;

	public float m_ScrollSpeed = 1.0f;
	private float m_Yvalue = 1.0f;
	private bool m_TweenerIsDone = false;
	void Awake() {
		if(m_CreditsView != null) {
			m_CreditsViewScroll = m_CreditsView.verticalScrollBar;
			m_CreditsViewScroll.value = 0.0f;
			UICamera.selectedObject = m_CreditsViewScroll.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.getButtonDown(InputManager.Button.Pause)) {
			if(!string.IsNullOrEmpty(m_LevelToLoad)) {
				if(m_FadeTweener != null) {
					m_TweenerIsDone = false;
					m_FadeTweener.PlayReverse();
					StartCoroutine("startLoadLevel");
				} else {
					Application.LoadLevel(m_LevelToLoad);
				}
			}
		}

		if(m_CreditsViewScroll != null) {
			m_CreditsViewScroll.value += m_ScrollSpeed * Time.deltaTime;
		}
	}

	public void tweenerIsDone() {
		m_TweenerIsDone = true;
	}

	IEnumerator startLoadLevel() {
		AsyncOperation ao = Application.LoadLevelAsync(m_LevelToLoad);
		ao.allowSceneActivation = false;

		while(!m_TweenerIsDone) {
			yield return null;
		}

		ao.allowSceneActivation = true;
	}
}
