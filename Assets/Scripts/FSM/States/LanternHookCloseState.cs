using UnityEngine;
using System.Collections;

[System.Serializable]
public class LanternHookCloseState : State<LanternHook> {

	public override void activate (LanternHook entity)
	{
//TODO: INV_	if( Inventory.contains( keyItem ) 
	}

	public override void reason (LanternHook entity) {

		if(entity.m_IsOpen){
			entity.m_FSM.changeState<LanternHookOpenState>();
		}
	}
}

