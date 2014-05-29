using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VideoPlayer : MonoBehaviour {
	public MovieTexture m_Video;
	public string m_FirstLevel;
	public float m_Delay = 0.0f;

	public UILabel m_EscapeLabel;
	public UITweener m_FadeWindowTweener;
	bool m_Done = false;
	AudioSource m_Audio;
	AsyncOperation m_AsyncOperation = null;
	public bool m_UseToLoadLevel = true;
	// Use this for initialization
	void Start () {
		if(m_FadeWindowTweener != null) {
			m_FadeWindowTweener.PlayReverse();
			m_FadeWindowTweener.ResetToBeginning();
			m_FadeWindowTweener.PlayReverse();
		}
		StartCoroutine("startVideo");
	}

	IEnumerator startVideo() {
		yield return new WaitForSeconds(m_Delay);
		m_Audio = audio;
		if(m_Video != null) {
			renderer.material.mainTexture = (Texture)m_Video;
			m_Video.Play();
			if(m_Audio.clip == null) {
				m_Audio.clip = m_Video.audioClip;
			}
			m_Audio.Play();
		}

		if(m_UseToLoadLevel) {
			StartCoroutine("startLoadLevel");
			StartCoroutine("waitForVideo");
		}
	}

	IEnumerator startLoadLevel() {
		if(string.IsNullOrEmpty(m_FirstLevel)) {
			yield break;
		}
		
		Debug.Log ("Loading started!");
		m_AsyncOperation = Application.LoadLevelAsync(m_FirstLevel);
		m_AsyncOperation.allowSceneActivation = false;

		while(m_AsyncOperation.progress < 0.9f) {
			m_Done = true;
		}

		Debug.Log ("Loading completed!");
	}

	IEnumerator waitForVideo() {
		while(m_Video.isPlaying) {
			yield return null;
		}
		
		StartCoroutine("fadeWindow");
	}

	IEnumerator fadeWindow() {
		if(m_FadeWindowTweener == null) {
			yield break;
		}
		m_FadeWindowTweener.PlayForward();
		float t = m_FadeWindowTweener.duration;
		while(t >= 0.0f) {
			m_Audio.volume -= (1.0f / m_FadeWindowTweener.duration) * Time.deltaTime;
			t -= Time.deltaTime;

			yield return null;
		}

		yield return new WaitForSeconds(m_FadeWindowTweener.duration);
		
		m_AsyncOperation.allowSceneActivation = true;
		Destroy(gameObject);

	}

	void Update() {
		if(m_AsyncOperation != null) {
			if(m_Done) {
				if(m_EscapeLabel != null && !m_EscapeLabel.gameObject.activeSelf) {
					m_EscapeLabel.gameObject.SetActive(true);
				}
				if(InputManager.getButton(InputManager.Button.Pause)) {
					StartCoroutine("fadeWindow");
				}
			}
		}
	}
}
