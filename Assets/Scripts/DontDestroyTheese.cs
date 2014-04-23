using UnityEngine;
using System.Collections;

public class DontDestroyTheese : MonoBehaviour {

	public GameObject[] m_GameObjects;

	void Start () {
		foreach (GameObject g in m_GameObjects) {
			DontDestroyOnLoad(g);
		}
	}
}
