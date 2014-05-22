using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Interactable that only can be "used"
/// by the player
/// 
/// Will trigger events when activated
/// 
/// Created by Arvid
/// </summary>
public class Behaviour_OnlyUse : Interactable {
	public UseState m_State = new UseState();

	private StateMachine<Behaviour_OnlyUse> m_FSM;

	HbClips.animationCallback[] m_Callbacks = new HbClips.animationCallback[3];	//Delegate for passing the correct callback function to the animator

	protected override void Start ()
	{
		base.Start ();
		m_FSM = new StateMachine<Behaviour_OnlyUse>(this, m_State);

		m_Callbacks[0] = new HbClips.animationCallback (beginCallback);		
		m_Callbacks[1] = new HbClips.animationCallback (activateCallback);
		m_Callbacks[2] = new HbClips.animationCallback (endCallback);
	}

	public override void activate ()
	{

		Messenger.Broadcast ("activate animation", m_FSM.CurrentState.m_AnimationClip, m_Callbacks);
	}

	public override void examine ()
	{
		base.examine ();
		m_FSM.CurrentState.examine (this);
	}

	//This callback will be called at the beginning of the animation
	void beginCallback(){
		PuzzleEvent.trigger ("onUseInstant", gameObject, true);
	}

	//This callback will be triggered on the given key frame of the animation
	void activateCallback(){
		base.activate ();
		PuzzleEvent.trigger("onUseOnly", gameObject, true);
	}

	//This callback will be triggered on the last frame of the animation
	void endCallback(){
		PuzzleEvent.trigger ("onUseEnd", gameObject, true);
	}
}
