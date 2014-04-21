using UnityEngine;
using System.Collections;

public class AIWaypoint : MonoBehaviour {

	public string m_CallFunction;

	// Use this for initialization
	void Start () {
		Messenger.AddListener<MoveObject>(m_CallFunction, addWaypoint);
	}

	public void addWaypoint(MoveObject moveObject) {
		moveObject.addWaypoint(transform.position);
	}
}
