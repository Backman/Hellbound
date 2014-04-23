using UnityEngine;
using System.Collections;

[System.Serializable]
public class LanternHookCloseState : State<LanternHook> {

	public override void activate (LanternHook entity)
	{
		foreach(Interactable item in Inventory.getInstance ().Items) {
			if(item.m_Thumbnail.name.Contains ("Lantern")) {
				entity.open ();
				Inventory.getInstance ().removeItem(item);
				break;
			}
		}
	}

	public override void reason (LanternHook entity) {

		if(entity.m_IsOpen){
			entity.m_FSM.changeState<LanternHookOpenState>();
		}
	}
}

