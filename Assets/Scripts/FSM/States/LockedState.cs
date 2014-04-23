using UnityEngine;
using System.Collections;

[System.Serializable]
public class LockedState : State<LockedInteractable> {

	public override void activate (LockedInteractable entity)
	{
		foreach(Interactable obj in entity.m_Keys) {
			if(Inventory.getInstance().containsItem(obj)) {
				entity.KeyState[obj] = true;
				Inventory.getInstance().removeItem (obj);
				if(entity.allKeys()) {
					entity.m_FSM.changeState<OpenedState>();
				}
				break;
			}
		}
	}
}
