using UnityEngine;
using System.Collections;

public class BadState : State<PressurePlate> {

	public override void enter (PressurePlate entity) {
		entity.gameObject.renderer.material.color = Color.red;
		/*foreach(PressurePlate obj in entity.m_ObjectsToAffect){
			if(obj.m_Symbol == PressurePlate.Symbol.Wind) {
				obj.m_Symbol = PressurePlate.Symbol.Fire;
				obj.gameObject.renderer.material.color = Color.red;
			} else {
				obj.m_Symbol = PressurePlate.Symbol.Wind;
				obj.gameObject.renderer.material.color = Color.magenta;
			}	
		}*/
	}

	public override void execute (PressurePlate entity, float deltaTime) {
	}

	public override void exit (PressurePlate entity) {
	}

	public override void activate (PressurePlate entity) {
	}
}
