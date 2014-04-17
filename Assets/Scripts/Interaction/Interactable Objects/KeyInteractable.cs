using UnityEngine;
using System.Collections;

public class KeyInteractable : Interactable {

	public KeyState m_State;

	private StateMachine<KeyInteractable> m_FSM;
	// Use this for initialization
	void Start () {
		m_FSM = new StateMachine<KeyInteractable>(this, m_State);
	}

	public override void useWith (GameObject obj) {
		if(obj != null){
			Interactable door = (Interactable)obj.GetComponent(typeof(Interactable));
			if(door != null){
				door.useWith (gameObject);
			}
		}
	}

	public override void pickUp () {
		m_FSM.CurrentState.pickUp(this);
	}

	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}
