using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class ExecutionType<T> : MonoBehaviour {
	private FiniteStateMachine<T> m_FSM = null;

	protected void notifyFSM() {
		m_FSM.notify();
	}
}

namespace Executions {
	public enum Type {
		Update,
		LeftClick,
		TriggerEnter,
		TriggerLeave
	}

	public class OnUpdate <T> : ExecutionType <T>  {
		void Update() {
			notifyFSM();
		}
	}

	public class OnClick <T> : ExecutionType <T> {
		void Update() {
			if(Input.GetButtonDown("Fire1")) {
				notifyFSM();
			}
		}
	}

	public class OnEnter <T> : ExecutionType <T> {
		void OnTriggerEnter(Collider other){
			notifyFSM();
		}
	}

	public class OnLeave <T> : ExecutionType <T> {
		void OnTriggerLeave(Collider other) {
			notifyFSM();
		}
	}
}

