using UnityEngine;
using System.Collections;

/// <summary>
/// Behaviour_ simple door.
/// 
/// This behaviour is supposed to represent simple doors
/// which can be activated to open.
/// When they are opened, they rotate around their pivot point
/// along the selected axis, as long as they are not locked.
/// 
/// Doors are "one shot" use only. Thus, after they have been opened
/// they cannot be closed again.
/// 
/// Created by Simon Jonasson
/// </summary>
public class Behaviour_DoorSimple : Interactable {
	#region Variables
	/******************** State Logic***************************************/
//	[Tooltip("The properties for the doors 'Locked' state.\nClick the |> to expand")]
	public  SimpleDoorLockedState m_Locked = new SimpleDoorLockedState();
//	[Tooltip("The properties for the doors 'Closed' state.\nClick the |> to expand")]
	public  SimpleDoorClosedState m_Closed = new SimpleDoorClosedState();
//	[Tooltip("The properties for the doors 'Open' state.\nClick the |> to expand")]
	public  SimpleDoorOpenedState m_Opened = new SimpleDoorOpenedState();
	
	public enum CurrentState{ Locked, Open, Closed };
	[Tooltip("This variable controlls which state the door starts in.")]
	public CurrentState m_CurrentState = CurrentState.Closed;	
	
	private StateMachine<Behaviour_DoorSimple> m_FSM;
	/**********************************************************************/
	
	/********** Manipulations *******************************************/
	[SerializeField][Tooltip("This field points to which tweener the door system should play. " +
							 "If no tweener is present, the script will search for a tweener on this object")]
	private UIPlayTween m_Tweener = null; 
	public  UIPlayTween Tweener{
		get{ return m_Tweener; } 
	}
	/**********************************************************************/

	/************* Behaviours *********************************************/
	[Tooltip("Regulates if the player can activate this door or not.\nRemember to clear the 'Use text' field under every state if this is set")]
	public bool m_UsableByPlayer = true;
	[Tooltip("Regulates if the door can be ACTIVATED BY THE PLAYER more than once")]
	public bool m_OneShot = true;
	private bool m_Used = false;	//This bool is modified by the closed state
	public bool Used{	
		set{ m_Used = value; }
	}	
	[HideInInspector]
	public bool m_Moving = false;
	
	public FMODAsset m_DoorOpenSound = null;
	public FMODAsset m_DoorCloseSound = null;

	protected HbClips.animationCallback[] m_Callbacks = new HbClips.animationCallback[1];	//Delegate for passing the correct callback function to the animator
	/**********************************************************************/
	#endregion


	protected override void Awake(){
		base.Awake();

		if( m_Tweener == null ){ 
			m_Tweener = GetComponent<UIPlayTween>();
			if( m_Tweener == null ){
				Debug.LogWarning("Error! No tweener present! " + gameObject.name + " " + gameObject.transform.position);
			}
		}
		
		m_FSM = new StateMachine<Behaviour_DoorSimple>(this, m_Locked);
		m_FSM.addState(m_Opened);
		m_FSM.addState(m_Closed);
		
		switch( m_CurrentState ){
		case CurrentState.Locked:
			m_FSM.changeState<SimpleDoorLockedState>();
			break;
		case CurrentState.Closed:
			m_FSM.changeState<SimpleDoorClosedState>();
			break;
		case CurrentState.Open:
			m_FSM.changeState<SimpleDoorOpenedState>();
			break;
		}
		
		m_Callbacks[0] = new HbClips.animationCallback (activateCallback);	//Assign the callback func
	}
	
	public override void activate(){
		Messenger.Broadcast ("activate animation", m_FSM.CurrentState.m_AnimationClip, m_Callbacks);
	}
	
	public override void examine ()	{
		base.examine ();
		m_FSM.CurrentState.examine(this);
	}
	
	public void movementDone(){
		m_Moving = false;
		
		if( m_CurrentState == CurrentState.Closed ){
			m_FSM.changeState<SimpleDoorOpenedState>();
			m_CurrentState = CurrentState.Open;
		} else if( m_CurrentState == CurrentState.Open) {
			m_FSM.changeState<SimpleDoorClosedState>();
			m_CurrentState = CurrentState.Closed;
		}

	}
	public void movementDoneUpdateFocus(){
		movementDone();
		Messenger.Broadcast("update focus");
	}

	void activateCallback(){
		PuzzleEvent.trigger("onUseDoor", gameObject, true);
		if( !(m_Used & m_OneShot) && m_UsableByPlayer && !m_Moving  ){
			base.activate();
			m_FSM.CurrentState.activate(this);
			//PuzzleEvent.trigger("onUseDoor", gameObject, true);
		}
	}

	#region Behaviours
	public bool close(){
		if( m_CurrentState == CurrentState.Open ){
			if(m_DoorCloseSound != null) {
				FMOD_StudioSystem.instance.PlayOneShot(m_DoorCloseSound, transform.position);
			}
			m_FSM.CurrentState.activate(this);
			return true;
		} else {
			Debug.Log("You are trying to close an illigal door. "+gameObject.name + " "+gameObject.transform.position);
			return false;
		}
	}

	public bool open(){
		if( m_CurrentState == CurrentState.Closed ){
			if(m_DoorOpenSound != null) {
				FMOD_StudioSystem.instance.PlayOneShot(m_DoorOpenSound, transform.position);
			}
			m_FSM.CurrentState.activate(this);
			return true;
		} else {
			Debug.Log("You are trying to open an illigal door. "+gameObject.name + " "+gameObject.transform.position);
			return false;
		}
	}

	public void toggle(){
		if( m_CurrentState == CurrentState.Open ){
			this.close();
		} else {
			this.open();
		}
	}

	public void lockDoor(){
		this.close();
		m_CurrentState = CurrentState.Locked;
		m_FSM.changeState<SimpleDoorLockedState>();
	}

	public void unlockDoor(){
		if( m_CurrentState == CurrentState.Locked ){
			m_CurrentState = CurrentState.Closed;
			m_FSM.changeState<SimpleDoorClosedState>();
		} else {
			Debug.Log("The door was not locked. " +gameObject.name + " "+gameObject.transform.position);
		}
	}

	public void unlockAndOpen(){
		unlockDoor();
		open();
	}
	#endregion
}
