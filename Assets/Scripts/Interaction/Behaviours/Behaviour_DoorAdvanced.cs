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
/// Created by Simon
/// </summary>
public class Behaviour_DoorAdvanced : Interactable {
	#region Variables
	/******************** State Logic***************************************/
	public  AdvancedDoorLockedState  m_Locked = new AdvancedDoorLockedState();
	public  AdvancedDoorClosedState  m_Closed = new AdvancedDoorClosedState();
	public  AdvancedDoorOpenedState  m_Opened = new AdvancedDoorOpenedState();
	
	public enum CurrentState{ Locked, Open, Closed };
	public CurrentState m_CurrentState = CurrentState.Closed;	
	
	private StateMachine<Behaviour_DoorAdvanced> m_FSM;
	/**********************************************************************/
	
	/********** Manipulations *******************************************/
	private TweenRotation m_Tweener; 
	public  TweenRotation Tweener{
		get{ return m_Tweener; } 
	}

	public enum Actions {openThis, closeThis, unlockThis, lockThis, unlockAndOpenThis, closeAndLockThis};
	public Actions m_ActionsAvailable = Actions.openThis;
	/**********************************************************************/
	
	/************* Behaviours *********************************************/
	public bool m_OneShot = true;
	private bool m_Used = false;	//This bool is modified by the closed state
	public bool Used{	
		set{ m_Used = value; }
	}	
	[HideInInspector]
	public bool m_Moving = false;
	/**********************************************************************/
	#endregion
	protected override void Awake(){
		base.Awake();
		
		m_Tweener = GetComponent<TweenRotation>();
		if( m_Tweener == null ){
			Debug.LogError("Error! No tweener present! " + gameObject.name + " " + gameObject.transform.position);
		}
		
		m_FSM = new StateMachine<Behaviour_DoorAdvanced>(this, m_Locked);
		m_FSM.addState(m_Opened);
		m_FSM.addState(m_Closed);
		
		switch( m_CurrentState ){
		case CurrentState.Locked:
			m_FSM.changeState<AdvancedDoorLockedState>();
			break;
		case CurrentState.Closed:
			m_FSM.changeState<AdvancedDoorClosedState>();
			break;
		case CurrentState.Open:
			m_FSM.changeState<AdvancedDoorOpenedState>();
			break;
		}
	}
	
	public override void activate(){
		if( !(m_Used & m_OneShot) && !m_Moving  ){
			base.activate();
			m_FSM.CurrentState.activate(this);
		}
	}
	
	public override void examine ()	{
		base.examine ();
		m_FSM.CurrentState.examine(this);
	}
	
	public void movementDone(){
		m_Moving = false;
		
		if( m_CurrentState == CurrentState.Closed ){
			m_FSM.changeState<AdvancedDoorOpenedState>();
			m_CurrentState = CurrentState.Open;
		} else if( m_CurrentState == CurrentState.Open) {
			m_FSM.changeState<AdvancedDoorClosedState>();
			m_CurrentState = CurrentState.Closed;
		}
		
		Messenger.Broadcast("clear focus");
	}
	
	public bool close(){
		if( m_CurrentState == CurrentState.Open ){
			m_FSM.CurrentState.activate(this);
			m_FSM.changeState<AdvancedDoorClosedState>();
			return true;
		} else {
			Debug.Log("You are trying to close an illigal door. "+gameObject.name + " "+gameObject.transform.position);
			return false;
		}
	}
	
	public bool open(){
		if( m_CurrentState == CurrentState.Closed ){
			m_FSM.CurrentState.activate(this);
			m_FSM.changeState<AdvancedDoorOpenedState>();
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
		m_FSM.changeState<AdvancedDoorLockedState>();
	}
	
	public void unlockDoor(){
		if( m_CurrentState == CurrentState.Locked ){
			m_CurrentState = CurrentState.Closed;
			m_FSM.changeState<AdvancedDoorClosedState>();
		} else {
			Debug.Log("The door was not locked. " +gameObject.name + " "+gameObject.transform.position);
		}
	}
}
