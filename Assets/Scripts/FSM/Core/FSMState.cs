using UnityEngine;
using System;

[System.Serializable]
public abstract class FSMState <T> {
	protected T m_Entity;
	protected ExecutionType<T> m_ExecutionType;

	public Executions.Type m_ExecutionMode = Executions.Type.Update;

	/*public FSMState(Executions.Type type) {
		m_ExecutionMode = type;
		setUpExecutionType();
	}*/

	public void registerEntity(T entity) {
		m_Entity = entity;
	}

	protected void setUpExecutionType(Executions.Type type) {
		switch(type) {
		case Executions.Type.LeftClick:
			Debug.Log ("ExecutionMode: LeftClick");
			m_ExecutionType = T.AddComponent(Executions.OnClick <T>()) ;
			break;
		case Executions.Type.TriggerEnter:
			Debug.Log ("ExecutionMode: OnEnter");
			m_ExecutionType = T.AddComponent(Executions.OnEnter <T>());
			break;
			
		case Executions.Type.TriggerLeave:
			Debug.Log ("ExecutionMode: OnLeave");
			m_ExecutionType = T.AddComponent(Executions.OnLeave <T>());
			break;
			
		case Executions.Type.Update:
			Debug.Log ("ExecutionMode: OnUpdate");
			m_ExecutionType = T.AddComponent(Executions.OnUpdate <T>());
			break;
		}
	}

	public virtual void enter(T entity){}
	public virtual void execute(T entity){}
	public virtual void exit(T entity){}


}
