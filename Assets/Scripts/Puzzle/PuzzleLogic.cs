using UnityEngine;
using System.Collections;

public abstract class PuzzleLogic : MonoBehaviour {
	[SerializeField] string m_EventToHandle;
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject>(m_EventToHandle, onEvent);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public abstract void onEvent(GameObject obj);
}
