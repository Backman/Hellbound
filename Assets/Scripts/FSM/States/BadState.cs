using UnityEngine;
using System.Collections;

public class BadState : State<PressurePlate> {

	public override void enter (PressurePlate entity) {
		entity.gameObject.renderer.material.color = Color.red;
	}

	public override void activate (PressurePlate entity) {
		Debug.Log ("The ceiling is falling towards you...");
	}
}
