using UnityEngine;
using System.Collections;

public class MovableInteractable : Interactable {
	public MovableState m_MovableState;
	public ImmovableState m_ImmovableState;

	private StateMachine<MovableInteractable> m_FSM;
	private bool m_Movable = false;
	void Start(){
		m_FSM = new StateMachine<MovableInteractable>(this, m_ImmovableState);
		m_FSM.addState (m_MovableState);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			m_Movable = !m_Movable;
		}

		if(m_Movable && m_FSM.CurrentState != m_MovableState) {
			m_FSM.changeState<MovableState>();
		} else if(!m_Movable && m_FSM.CurrentState != m_ImmovableState){
			m_FSM.changeState<ImmovableState>();
		}
	}

	public override void examine () {
		m_FSM.CurrentState.examine(this);
	}
}

