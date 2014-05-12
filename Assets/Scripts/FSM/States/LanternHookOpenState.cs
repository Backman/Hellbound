using UnityEngine;
using System.Collections;

[System.Serializable]
public class LanternHookOpenState : State<LanternHook> {
	public GameObject[] m_ObjectsToActivate; 

	public override void enter (LanternHook entity){
		Debug.Log ("LanternHook is Open");

		if( m_ObjectsToActivate.Length > 0){
			foreach( GameObject g in m_ObjectsToActivate ){
			g.SetActive(true);
			}
		}
	}
}

