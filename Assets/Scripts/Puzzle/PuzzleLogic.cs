using UnityEngine;
using System.Collections;

public abstract class PuzzleLogic : MonoBehaviour {
	[SerializeField] string m_EventToHandle;
	// Use this for initialization
	protected virtual void Start () {
		Messenger.AddListener<GameObject>(m_EventToHandle, onEvent);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public abstract void onEvent(GameObject obj);
}
