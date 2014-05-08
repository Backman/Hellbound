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
public class Behaviour_DoorSimple : Interactable {
	#region Variables
	/******************** State Logic***************************************/
	public  SimpleDoorLockedState m_Locked = new SimpleDoorLockedState();
	public  SimpleDoorClosedState m_Closed = new SimpleDoorClosedState();
	public  SimpleDoorOpenedState m_Opened = new SimpleDoorOpenedState();
	
	public enum CurrentState{ Locked, Open, Closed };
	public CurrentState m_CurrentState = CurrentState.Closed;	
	
	private StateMachine<Behaviour_DoorSimple> m_FSM;
	/**********************************************************************/
	
	/********** Manipulations *******************************************/
	private TweenRotation m_Tweener; 
	public  TweenRotation Tweener{
		get{ return m_Tweener; } 
	}
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
			m_FSM.changeState<SimpleDoorOpenedState>();
			m_CurrentState = CurrentState.Open;
		} else if( m_CurrentState == CurrentState.Open) {
			m_FSM.changeState<SimpleDoorClosedState>();
			m_CurrentState = CurrentState.Closed;
		}
		
		Messenger.Broadcast("clear focus");
	}

	public bool close(){
		if( m_CurrentState == CurrentState.Open ){
			m_FSM.CurrentState.activate(this);
			m_FSM.changeState<SimpleDoorClosedState>();
			return true;
		} else {
			Debug.Log("You are trying to close an illigal door. "+gameObject.name + " "+gameObject.transform.position);
			return false;
		}
	}

	public bool open(){
		if( m_CurrentState == CurrentState.Closed ){
			m_FSM.CurrentState.activate(this);
			m_FSM.changeState<SimpleDoorOpenedState>();
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
}
