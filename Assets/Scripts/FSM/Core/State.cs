﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class State <T> {

	[Multiline]
	public string m_ExamineText;

	protected StateMachine<T> m_Machine;
	protected T m_Owner;

	public State() {}

	public void setText(string text){
		m_ExamineText = text;
	}

	public void setMachineAndOwner(StateMachine<T> machine, T owner) {
		m_Machine = machine;
		m_Owner = owner;
		initialize();
	}

	public virtual void initialize() {}

	public virtual void enter(T entity) {}
	public virtual void reason(T entity) {}
	public virtual void execute(T entity, float deltaTime) {}
	public virtual void exit(T entity) {}

	public virtual void activate(T entity) {}
	public virtual void examine(T entity) { if(m_ExamineText.Trim () != string.Empty) GUIManager.Instance.simpleShowText(m_ExamineText, true); }
	public virtual void pickUp(T entity) {}
}
