using UnityEngine;
using System.Collections;

/// <summary>
/// Note state.
/// 
/// The state to inject into the FSM to 
/// represent paper notes
/// 
/// Created by Simon
/// </summary>
[System.Serializable]
public class NoteState : State<Behaviour_Note> 
{ 

	public override void activate (Behaviour_Note entity)
	{
		base.activate (entity);
		GUIManager.Instance.showNote(entity.m_NoteSettings);
	}
}

