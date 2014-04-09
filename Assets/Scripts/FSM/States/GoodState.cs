using UnityEngine;
using System.Collections;

public class GoodState : State<PressurePlate> {

	public override void enter (PressurePlate entity)
	{
		entity.gameObject.renderer.material.color = Color.green;
		/*foreach(PressurePlate obj in entity.m_ObjectsToAffect){
			if(obj.m_Symbol == PressurePlate.Symbol.Fire) {
				obj.m_Symbol = PressurePlate.Symbol.Wind;
				obj.gameObject.renderer.material.color = Color.green;
			} else {
				Debug.Log (obj.name + " is changing state to BadState");
				obj.changeState<BadState>();
				obj.m_Symbol = PressurePlate.Symbol.Fire;
				obj.gameObject.renderer.material.color = Color.cyan;
			}
		}*/
		//Debug.Log("Entering Goodstate!");
	}

	public override void reason (PressurePlate entity) {	
		/*if(entity.m_Symbol == PressurePlate.Symbol.Earth) {
			m_Machine.changeState<BadState>();
		}*/
	}

	public override void execute (PressurePlate entity, float deltaTime) {
		//Debug.Log ("Is in Goodstate!");
	}

	public override void exit (PressurePlate entity) {
		//Debug.Log("Leaving Goodstate...");
	}

	public override void activate (PressurePlate entity) {
		Messenger.Broadcast("plate triggered", entity.m_Symbol);
		Debug.Log ("You're good mate!");
	}
}
