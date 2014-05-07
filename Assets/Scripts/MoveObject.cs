using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveObject : MonoBehaviour {
	public string m_CallbackFunction;
	public List<Vector3> m_Waypoints = new List<Vector3>();

	TweenPosition r_TweenPosition;
	private int m_CurrentWaypoint = 0;
	// Use this for initialization
	void Start () {
		Messenger.Broadcast<MoveObject>(m_CallbackFunction, this);
		r_TweenPosition = (TweenPosition)GetComponent(typeof(TweenPosition));
		updateTweenPosition();
	}

	public void addWaypoint(Vector3 waypoint) {
		m_Waypoints.Add(waypoint);
	}

	public void updateTweenPosition() {
		r_TweenPosition.from = m_Waypoints[m_CurrentWaypoint];
		m_CurrentWaypoint++;
		if(m_CurrentWaypoint < m_Waypoints.Count) {
			r_TweenPosition.to = m_Waypoints[m_CurrentWaypoint];
			r_TweenPosition.ResetToBeginning ();
			r_TweenPosition.PlayForward();
		} else {
			destroy ();
		}
	}

	public void destroy(){
		Destroy(gameObject);
	}
}
