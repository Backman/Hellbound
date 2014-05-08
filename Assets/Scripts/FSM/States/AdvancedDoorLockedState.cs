using UnityEngine;
using System.Collections;

[System.Serializable]
public class AdvancedDoorLockedState : State<Behaviour_DoorAdvanced>
{
	[Multiline]
	public string m_PromptIfUsed;

	public override void activate (Behaviour_DoorAdvanced entity)
	{
		base.activate (entity);
		GUIManager.Instance.simpleShowText(m_PromptIfUsed, "Use", true );
	}
}

