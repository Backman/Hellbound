using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LockedInteractable : Interactable {

	public LockedState m_LockedState;
	public OpenedState m_OpenedState;
	public List<Interactable> m_Keys = new List<Interactable>();
	private Dictionary<Interactable, bool> m_KeyState = new Dictionary<Interactable, bool>();
	public Dictionary<Interactable, bool> KeyState {
		get { return m_KeyState; }
	}
	public StateMachine<LockedInteractable> m_FSM;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		initializeKeyState();
		m_FSM = new StateMachine<LockedInteractable>(this, m_LockedState);
		m_FSM.addState(m_OpenedState);
	}

	public override void useWith (GameObject obj) {
		Interactable key = obj.GetComponent<Interactable>();
		if(m_Keys.Contains (key)){
			m_KeyState[key] = true;
			if(Inventory.getInstance ().containsItem(key)){
				Inventory.getInstance().removeItem(key);
			}
			if( allKeys() ){
				m_FSM.changeState<OpenedState>();
			}
		}
	}

	public override void activate ()
	{
		base.activate ();
		m_FSM.CurrentState.activate (this);
	}

	private void initializeKeyState(){
		foreach(Interactable obj in m_Keys){
			m_KeyState.Add (obj, false);
		}
		Debug.Log ("Dictionary size: " + m_KeyState.Count);
	}

	public bool allKeys() {
		foreach( bool b in m_KeyState.Values){
			if( b == false ) return b;
		}
		return true;
	}

	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}
