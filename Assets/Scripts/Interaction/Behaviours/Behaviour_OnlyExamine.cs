using UnityEngine;
using System.Collections;

public class Behaviour_OnlyExamine : Interactable {

	public ExamineState m_State = new ExamineState();

	private StateMachine<Behaviour_OnlyExamine> m_FSM;

	protected override void Start(){
		m_FSM = new StateMachine<Behaviour_OnlyExamine>(this, m_State);
	}

	public override void examine(){
		m_FSM.CurrentState.examine( this );
	}
}
