using UnityEngine;
using System.Collections;

public class GoodState : State<PressurePlate> {

	public override void enter (PressurePlate entity)
	{
		entity.gameObject.renderer.material.color = Color.green;
	}

	public override void reason (PressurePlate entity) {	
	}

	public override void execute (PressurePlate entity, float deltaTime) {
	}

	public override void exit (PressurePlate entity) {
	}

	public override void activate (PressurePlate entity) {
		Messenger.Broadcast("plate triggered", entity.m_Symbol);
	}
}
