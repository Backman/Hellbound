using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Interactable that only can be "used"
/// 
/// Created by Arvid
/// </summary>
public class Behaviour_OnlyUse : Interactable {
	public UseState m_State = new UseState();

	private StateMachine<Behaviour_OnlyUse> m_FSM;

	HbClips.animationCallback m_Callback;	//Delegate for passing the correct callback function to the animator

	protected override void Start ()
	{
		base.Start ();
		m_FSM = new StateMachine<Behaviour_OnlyUse>(this, m_State);

		m_Callback = new HbClips.animationCallback (activateCallback);	//Assign the callback func
	}

	public override void activate ()
	{
		Messenger.Broadcast ("activate animation", m_FSM.CurrentState.m_AnimationClip, m_Callback);
	}

	public override void examine ()
	{
		base.examine ();
		m_FSM.CurrentState.examine (this);
	}

	void activateCallback(){
		base.activate ();
		PuzzleEvent.trigger("onUseOnly", gameObject, true);
	}
}
