using UnityEngine;
using System.Collections;

public class ExamineTest : Interactable {

	public ExamineState1 state1;
	public ExamineState2 state2;

	private StateMachine<ExamineTest> m_FSM;

	void Start(){
		//state1 = new ExamineState1();
		//state2 = new ExamineState2();

		m_FSM = new StateMachine<ExamineTest>(this, state1);
		m_FSM.addState(state2);
	}

	public override void examine () {
	
		m_FSM.CurrentState.examine (this);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			m_FSM.changeState<ExamineState2>();
		}
	}
}
