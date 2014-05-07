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
/// Created by Simon
/// </summary>
public class Behaviour_SimpleDoor : MonoBehaviour {

	/*	public DoorStateOpen   m_Open = new DoorStateOpen();
		public DoorStateClosed m_Closed = new DoorStateClosed();
	[SerializeField]
	private bool m_BeginClosed = true;
	[SerializeField]
	private bool m_Locked = false;
	public bool Locked{
		get{ return m_Locked; }
		set{ m_Locked = value;}
	}
	
	private StateMachine<Behaviour_SimpleDoor> m_FSM;
	
	protected override void Start(){
		if( m_BeginClosed ) {
			m_FSM = new StateMachine<Behaviour_SimpleDoor>(this, m_Closed);
			m_FSM.addState( m_Open ); 
		} else {
			m_FSM = new StateMachine<Behaviour_SimpleDoor>(this, m_Open);
			m_FSM.addState( m_Closed ); 
		}
	}
	
	public override void activate(){
		base.activate();
		m_FSM.CurrentState.activate(this);
	}
	
	public override void examine ()	{
		base.examine ();
		m_FSM.CurrentState.examine(this);
	}
*/
}
