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
	// Use this for initialization
	void Start () {
		StartCoroutine("startVideo");
	}

	IEnumerator startVideo() {
		yield return new WaitForSeconds(m_Delay);
		m_Audio = audio;
		if(m_Video != null) {
			renderer.material.mainTexture = (Texture)m_Video;
			m_Video.Play();
			m_Audio.Play();
		}
		StartCoroutine("startLoadLevel");
	}

	IEnumerator startLoadLevel() {
		if(string.IsNullOrEmpty(m_FirstLevel)) {
			yield break;
		}
		
		Debug.Log ("Loading started!");
		m_AsyncOperation = Application.LoadLevelAsync(m_FirstLevel);
		m_AsyncOperation.allowSceneActivation = false;

		while(!m_AsyncOperation.isDone) {
			float progress = m_AsyncOperation.progress;
			if(progress >= 0.9f) {
				m_Done = true;
				break;
			}
			yield return null;
		}

		Debug.Log ("Loading completed!");
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
			if( m_Done) {
				if(m_EscapeLabel != null && !m_EscapeLabel.gameObject.activeSelf) {
					m_EscapeLabel.gameObject.SetActive(true);
				}
				if(InputManager.getButton(InputManager.Button.Pause)) {
					StartCoroutine("fadeWindow");
				}
				if(!m_Video.isPlaying) {
					StartCoroutine("fadeWindow");
				}
			}
		}
	}
}
