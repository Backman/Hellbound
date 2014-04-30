using UnityEngine;
using System.Collections;

public class AIWaypoints : MonoBehaviour {
	public string m_CallFunction;
	public Transform[] m_Waypoints;

	// Use this for initialization
	void Start () {
		Messenger.AddListener<MoveObject>(m_CallFunction, addWaypointsToObjet);
	}
	
	public void addWaypointsToObjet(MoveObject moveObject) {
		foreach(Transform t in m_Waypoints) {
			moveObject.addWaypoint(t.position);
		}
	}
}
