using UnityEngine;
using System.Collections;

[System.Serializable]
public class SimpleDoorLockedState : State<Behaviour_DoorSimple>
{
	[Multiline][Tooltip("The text that will be displayed if the player tries to 'Use' the object while the door is still locked")]
	public string m_PromptIfUsed;

	public override void activate (Behaviour_DoorSimple entity)
	{
		base.activate (entity);
		GUIManager.Instance.simpleShowText(m_PromptIfUsed, "Use", true );
	}
}
