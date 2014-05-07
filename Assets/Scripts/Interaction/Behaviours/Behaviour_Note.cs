using UnityEngine;
using System.Collections;

/// <summary>
/// Behaviour_ note.
/// 
/// The behaviour script for notes.
/// A note can be examined. If it is,
/// a sprite will be displayed with text
/// written on it.
/// 
/// Created by Simon
/// </summary>
public class Behaviour_Note : Interactable {

	public NoteState m_State = new NoteState();
	public MyGUI.NoteSettings m_NoteSettings;
	
	private StateMachine<Behaviour_Note> m_FSM;

	protected override void Start(){
		m_FSM = new StateMachine<Behaviour_Note>(this, m_State);
	}

	public override void activate(){
		base.activate();
		m_FSM.CurrentState.activate(this);
	}

	public override void examine ()
	{
		base.examine ();
		m_FSM.CurrentState.examine(this);
	}

}
