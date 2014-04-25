using UnityEngine;
using System.Collections;

public class Behaviour_PickUp : Interactable {
	public string   m_ItemName;
	public UISprite m_ItemThumbnail;
	
	public KeyState m_State;	
	private StateMachine<Behaviour_PickUp> m_FSM;

	void Start () {
		m_FSM = new StateMachine<Behaviour_PickUp>(this, m_State);
	}
	
	public override void pickUp () {
		m_FSM.CurrentState.pickUp( this );
	}
	
	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}
