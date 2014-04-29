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

	protected override void Start ()
	{
		base.Start ();
		m_FSM = new StateMachine<Behaviour_OnlyUse>(this, m_State);
	}

	public override void activate ()
	{
		base.activate ();
		m_FSM.CurrentState.activate (this);
	}

	public override void examine ()
	{
		base.examine ();
		m_FSM.CurrentState.examine (this);
	}
}
