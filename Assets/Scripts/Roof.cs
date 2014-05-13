﻿using UnityEngine;
using System.Collections;

public class Roof : MonoBehaviour {
	[SerializeField][Tooltip("The tween-player for lowering the roof")]
	private UIPlayTween m_Tweener;

	private enum Phase {Start, InMotion, Wait, Kill};
	Phase m_Phase = Phase.Start;

	// Use this for initialization
	void Start () {
		Messenger.AddListener ("start roof", startRoof );
		Messenger.AddListener ("lower roof", lowerRoof );
	}

	

	public void startRoof(){
		if( m_Phase == Phase.Start ){
			Debug.Log("Starting roof");
			m_Tweener.Play(true);
			m_Phase = Phase.InMotion;
		} else {
			Debug.Log("Error! Invalid tween state of the roof");
		}
	}

	public void startDone(){
		if( m_Phase == Phase.InMotion ){
			m_Tweener.tweenGroup = 1;
			m_Phase = Phase.Wait;
		} else {
			Debug.Log("Error! Invalid tween state of the roof");
		}
	}

	public void lowerRoof(){
		if( m_Phase == Phase.Wait ) {
			Debug.Log("Killing player");
			m_Tweener.Play(true);
			m_Phase = Phase.InMotion;
		} else {
			Debug.Log("Error! Invalid tween state of the roof");
		}
	}
}
