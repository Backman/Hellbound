using UnityEngine;
using System.Collections;

public class ExamineTest : Interactable {

	public ExamineState1 state1;
	public ExamineState2 state2;

	private StateMachine<ExamineTest> m_FSM;

	protected override void Start(){
		base.Start();
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

	public override void activate () {
		m_FSM.CurrentState.activate (this);
	}

	public override void pickUp () {
		m_FSM.CurrentState.pickUp(this);
	}
}
