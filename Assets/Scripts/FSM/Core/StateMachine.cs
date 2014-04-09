using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class StateMachine <T> {
	protected T m_Owner;

	private State<T> m_CurrentState;
	public State<T> CurrentState { get { return m_CurrentState; } }
	private Dictionary<System.Type, State<T>> m_States = new Dictionary<System.Type, State<T>>();

	public StateMachine(T owner, State<T> startState) {
		m_Owner = owner;

		addState(startState);
		m_CurrentState = startState;
	}

	public void addState(State<T> state) {
		state.setMachineAndOwner(this, m_Owner);
		m_States[state.GetType()] = state;
	}

	public void update(float deltaTime) {
		m_CurrentState.reason(m_Owner);
		m_CurrentState.execute(m_Owner, deltaTime);
	}

	public S changeState<S>() where S : State<T> {
		var newType = typeof(S);
		if(m_CurrentState.GetType() == newType) {
			return m_CurrentState as S;
		}

		if(m_CurrentState != null) {
			m_CurrentState.exit(m_Owner);
		}

#if UNITY_EDITOR
		if(!m_States.ContainsKey(newType)) {
			var error = "ERROR: State " + newType + " does not exist in the state list..."+
				"Have you added it by calling addState?";
			Debug.LogError(error);
			throw new UnityException(error);
		}
#endif
		m_CurrentState = m_States[newType];
		m_CurrentState.enter(m_Owner);

		return m_CurrentState as S;
	}
}
