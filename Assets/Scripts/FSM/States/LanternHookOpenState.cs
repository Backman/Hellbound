using UnityEngine;
using System.Collections;

[System.Serializable]
public class LanternHookOpenState : State<LanternHook> {


	public override void enter (LanternHook entity){
		Debug.Log ("LanternHook is Open");
		entity.m_ObjectToOpen.useWith(entity.gameObject);
	}
}

