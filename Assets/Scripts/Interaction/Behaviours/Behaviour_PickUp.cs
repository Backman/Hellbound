using UnityEngine;
using System.Collections;

public class Behaviour_PickUp : Interactable {
	[Tooltip("This field decides what name this item will be identified with.\nSince items are added to- and removed from- the inventory via name identifying," +
			 "that means that if two items have the same name they will be treated as copies of one another.\n" +
			 "The name only mattes for the inventory logic.")]
	public string   m_ItemName;
	[Tooltip("The item sprite which will be displayed in the inventory after the item's been picked up")]
	public UISprite m_ItemThumbnail;
	
	public KeyState m_State;	
	private StateMachine<Behaviour_PickUp> m_FSM;

	void Start () {
		m_FSM = new StateMachine<Behaviour_PickUp>(this, m_State);
	}
	
	public override void activate () {
		base.activate ();
		PuzzleEvent.trigger ("onPickUpInteractable", gameObject, true);
		m_FSM.CurrentState.activate( this );
	
	}
	
	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}
