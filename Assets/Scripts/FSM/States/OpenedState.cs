using UnityEngine;
using System.Collections;

[System.Serializable]
public class OpenedState : State<LockedInteractable> {

	public override void enter (LockedInteractable entity)
	{
		GameObject.Destroy(entity.gameObject);
		Messenger.Broadcast("clear focus");
	}

	public override void activate (LockedInteractable entity)
	{
		base.activate (entity);
	}
}
