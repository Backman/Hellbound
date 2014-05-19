using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmashWalls : MonoBehaviour {
	public string m_LevelToLoad;
	public string m_DeathText;
	public List<GameObject> m_Walls = new List<GameObject>();
	public float m_Timer = 3.0f;

	private int m_CurrentIndex = 0;
	void Start() {
		Messenger.AddListener<GameObject, bool>("onStartSmashWallsTimer", onStartSmashWallsTimer);
		Messenger.AddListener<GameObject, bool>("stopWalls", stopWalls);
	}

	public void onStartSmashWallsTimer(GameObject obj, bool tr) {
		StartCoroutine("startSmashing");
	}

	public void stopWalls(GameObject go, bool tr) {
		StartCoroutine("stopSmashing");
	}

	IEnumerator startSmashing() {
		if(m_CurrentIndex == 0) {
			m_Walls[m_CurrentIndex++].GetComponent<TweenPosition>().PlayForward();
			m_Walls[m_CurrentIndex++].GetComponent<TweenPosition>().PlayForward();
		}
		if(m_CurrentIndex < m_Walls.Count) {
			float t = m_Timer;


			while(t >= 0.0f) {


				t -= Time.deltaTime;
				yield return null;
			}
			m_Walls[m_CurrentIndex++].GetComponent<TweenPosition>().PlayForward();
			if(m_CurrentIndex == m_Walls.Count - 1) {
				//GUIManager.Instance.loadLevel( m_LevelToLoad, m_DeathText );
			}
			m_Walls[m_CurrentIndex++].GetComponent<TweenPosition>().PlayForward();

			StartCoroutine("startSmashing");
		}
	}

	IEnumerator stopSmashing() {
		int idx = 0;
		while(idx < m_Walls.Count) {
			m_Walls[idx++].GetComponent<TweenPosition>().enabled = false;

			yield return null;
		}
	}
}
