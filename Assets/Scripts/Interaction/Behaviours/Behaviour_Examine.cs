using UnityEngine;
using System.Collections;

public class Behaviour_Examine : Interactable {

	public ExamineState m_State = new ExamineState();

	private StateMachine<Behaviour_Examine> m_FSM;

	protected override Start(){
		m_FSM = new StateMachine<Behaviour_Examine>(this, m_State1);
		m_FSM.addState(m_State2);
	}
}
