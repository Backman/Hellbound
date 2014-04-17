using UnityEngine;
using System.Collections;

public class LanternHookOpenState : State<LanternHook> {

	public override void enter (LanternHook entity){
		entity.m_ObjectToOpen.useWith(entity.gameObject);
	}
}

