using UnityEngine;
using System;
using System.Collections.Generic;

public class FiniteStateMachine <T> {
	private T m_Owner;
	private FSMState<T> m_CurrentState = null;
	private FSMState<T> m_PreviousState = null;
	private FSMState<T> m_GlobalState = null;

	//private Dictionary<FSMState<T>, ExecutionType<T>> m_StateExecutionMethod = new Dictionary<FSMState<T>, ExecutionType<T>>();

	public FiniteStateMachine(T owner) {
		m_Owner = owner;
	}

	public void Update() {
		if(m_GlobalState != null) {
			m_GlobalState.execute(m_Owner);
		}
	}

	public void changeState(FSMState<T> newState) {
		if(m_CurrentState != null) {
			m_CurrentState.exit(m_Owner);
		}
		m_PreviousState = m_CurrentState;
		m_CurrentState = newState;

		if(m_CurrentState != null) {
			m_CurrentState.enter (m_Owner);
		}
	}

	public FSMState<T> registerState(FSMState<T> state) {
		state.registerEntity(m_Owner);
		return state;
	}

	public void unregisterState(FSMState<T> state) {

	}

	public void notify() {
		if(m_CurrentState != null) {
			m_CurrentState.execute(m_Owner);
		}
	}
}
