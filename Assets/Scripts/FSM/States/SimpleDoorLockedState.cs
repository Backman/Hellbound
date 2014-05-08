using UnityEngine;
using System.Collections;

[System.Serializable]
public class SimpleDoorLockedState : State<Behaviour_DoorSimple>
{
	[Multiline]
	public string m_PromptIfUsed;

	public override void activate (Behaviour_DoorSimple entity)
	{
		base.activate (entity);
		GUIManager.Instance.simpleShowText(m_PromptIfUsed, "Use", true );
	}
}

