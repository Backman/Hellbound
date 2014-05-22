using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmashWalls : MonoBehaviour {
	public string m_LevelToLoad;
	public string m_DeathText;
	public List<GameObject> m_Walls = new List<GameObject>();
	public float m_Timer = 3.0f;
	public float m_DeadIn = 16f;

	private int m_CurrentIndex = 0;
	void Start() {
		Messenger.AddListener<GameObject, bool>("onStartSmashWallsTimer", onStartSmashWallsTimer);
		Messenger.AddListener<GameObject, bool>("stopWalls", stopWalls);
	}

	public void onStartSmashWallsTimer(GameObject obj, bool tr) {
		StartCoroutine("startSmashing");
		StartCoroutine ("startKilling");
	}

	public void stopWalls(GameObject go, bool tr) {
		StartCoroutine("stopSmashing");
		StopCoroutine ("StartKilling");
	}

	IEnumerator startKilling(){

		float t = m_DeadIn;
		while(t >= 0.0f) {
			
			t -= Time.deltaTime;
			yield return null;
		}
		GUIManager.Instance.loadLastCheckPoint(m_DeathText);
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

			m_Walls[m_CurrentIndex++].GetComponent<TweenPosition>().PlayForward();

			StartCoroutine("startSmashing");
		}
	}

	IEnumerator stopSmashing() {
		int idx = 0;
		while(idx < m_Walls.Count) {
			LoadLevelTrigger trigger = m_Walls[idx].GetComponent<LoadLevelTrigger>();
			m_Walls[idx++].GetComponent<TweenPosition>().enabled = false;

			if(trigger != null) {
				trigger.enabled = false;
			}

			yield return null;
		}
	}
}
