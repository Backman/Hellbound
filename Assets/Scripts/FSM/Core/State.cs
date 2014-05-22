using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract base class for all the behaivour states
/// By Arvid Backman and Simon Jonasson
/// </summary>

[System.Serializable]
public abstract class State <T> {

	[Tooltip("The hover text for the 'Use' button")]
	public string m_UseText;
	[Multiline]
	public string m_ExamineText;

	[Tooltip("Decides which animation should be played when Activate is called on this object")]
	public HbClips.Animation m_AnimationClip = HbClips.Animation.None;
	

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

	public virtual void activate(T entity) { }
	public virtual void examine(T entity) { if(m_ExamineText.Trim () != string.Empty) GUIManager.Instance.simpleShowText(m_ExamineText); }
//	public virtual void pickUp(T entity) {}


}
