using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FSMStateDictionary {
	[SerializeField] private List<PressurePlate.States> _keys;
	[SerializeField] private List<FSMState<PressurePlate>> _values;

	public FSMStateDictionary(){
		_keys = new List<PressurePlate.States>();
		_values = new List<FSMState<PressurePlate>>();
	}

	public void Add(PressurePlate.States key, FSMState<PressurePlate> value) {
		_keys.Add(key);
		_values.Add (value);
	}
}

public class PressurePlate : MonoBehaviour {

	public enum States {
		Good,
		Bad
	}
	private FiniteStateMachine<PressurePlate> m_FSM;

	//public Dictionary<States, FSMState<PressurePlate>> m_States = new Dictionary<States, FSMState<PressurePlate>>();
	public FSMStateDictionary m_States = new FSMStateDictionary();
	// Use this for initialization
	void Start () {
		m_FSM = new FiniteStateMachine<PressurePlate>(this);
		m_States.Add (States.Bad, new PressurePlate_BadState(Executions.Type.LeftClick));
		m_States.Add (States.Good, new PressurePlate_GoodState(Executions.Type.TriggerLeave));
	}

	// Update is called once per frame
	void Update () {

	}
}

