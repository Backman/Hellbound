using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MovableInteractable : Interactable {
	public MovableState m_MovableState;
	public ImmovableState m_ImmovableState;

	public StateMachine<MovableInteractable> m_FSM;
	public bool Movable {
		get; set;
	}
	void Start(){
		Movable = false;
		rigidbody.isKinematic = !Movable;
		m_FSM = new StateMachine<MovableInteractable>(this, m_ImmovableState);
		m_FSM.addState (m_MovableState);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			Movable = !Movable;
		}

		m_FSM.update (Time.deltaTime);
	}

	public override void examine () {
		m_FSM.CurrentState.examine(this);
	}
}

