using UnityEngine;
using System.Collections;

/// <summary>
/// Behaviour_ pick up.
/// The state machine wrapper for object that the player can pick up. This wrapper only gives the object
/// the "pick up-able" property and nothing more.
/// 
/// Created by Simon Jonasson
/// </summary>
public class Behaviour_PickUp : Interactable {
	[Tooltip("This field decides what name this item will be identified with.\nSince items are added to- and removed from- the inventory via name identifying," +
			 "that means that if two items have the same name they will be treated as copies of one another.\n" +
			 "The name only mattes for the inventory logic.")]
	public string   m_ItemName;
	//[Tooltip("The item sprite which will be displayed in the inventory after the item's been picked up")]
	public UISprite m_ItemThumbnail
	{
		get{ return InventoryThumbnailDatabase.getThumbnail(m_ItemName); }
	}
	
	public KeyState m_State;	
	private StateMachine<Behaviour_PickUp> m_FSM;
	HbClips.animationCallback[] m_Callbacks = new HbClips.animationCallback[1];

	void Start () {
		base.Start ();
		m_FSM = new StateMachine<Behaviour_PickUp>(this, m_State);
		
		m_Callbacks[0] = new HbClips.animationCallback (activateCallback);	//Assign the correct callback func
	}

	/// <summary>
	/// Activate this instance.
	/// This function will request that the animator plays an animation (or does not) and then calls 
	/// the supplied callback function at the correct keyframe (or emidietly, of no animation is played).
	/// </summary>
	public override void activate () {	
		Messenger.Broadcast ("activate animation", m_FSM.CurrentState.m_AnimationClip, m_Callbacks);
	}
	
	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}

	private void log( ){
		Debug.Log ("Hej " + gameObject.name);
	}

	void activateCallback(){
		base.activate ();
		PuzzleEvent.trigger ("onPickUpInteractable", gameObject, true);
		m_FSM.CurrentState.activate( this );
	}
}
