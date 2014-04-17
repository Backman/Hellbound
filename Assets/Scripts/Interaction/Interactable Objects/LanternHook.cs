using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanternHook : Interactable {
	
	public LanternHookOpenState m_LockedState;
	public LanternHookCloseState m_OpenedState;

	public Interactable m_ObjectToOpen;

	public List<Interactable> m_Keys = new List<Interactable>();
	private Dictionary<Interactable, bool> m_KeyState = new Dictionary<Interactable, bool>();

	private StateMachine<LanternHook> m_FSM;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		initializeKeyState();
		m_FSM = new StateMachine<LanternHook>(this, m_LockedState);
		m_FSM.addState(m_OpenedState);
	}
	
	public override void useWith (GameObject obj) {
		Interactable key = obj.GetComponent<Interactable>();
		if(m_Keys.Contains (key)){
			m_KeyState[key] = true;
			if( allKeys() ){
				m_FSM.changeState<LanternHookOpenState>();
			}
		}
	}
	
	private void initializeKeyState(){
		foreach(Interactable obj in m_Keys){
			m_KeyState.Add (obj, false);
		}
	}
	
	private bool allKeys() {
		foreach( bool b in m_KeyState.Values){
			if( b == false ) return b;
		}
		return true;
	}
	
	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}
