using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {
	public Transform m_From;
	public Transform m_To;
	TweenPosition r_TweenPosition;

	// Use this for initialization
	void Start () {
		r_TweenPosition = (TweenPosition)GetComponent(typeof(TweenPosition));
		r_TweenPosition.from = transform.position;
		//r_TweenPosition.to = m_To.position;
		r_TweenPosition.PlayForward();
	}
	
	public void destroy(){
		Destroy(gameObject);
	}
}
