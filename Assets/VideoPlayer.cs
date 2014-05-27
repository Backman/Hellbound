using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VideoPlayer : MonoBehaviour {
	public MovieTexture m_Video;

	// Use this for initialization
	void Start () {
		if(m_Video != null) {
			renderer.material.mainTexture = (Texture)m_Video;
			m_Video.Play();
			audio.Play();
		}
	}

	void Update() {
		if(!m_Video.isPlaying) {
			Debug.Log("Load THE FIRST FUCKING LEVEL!");
		}
	}
}
