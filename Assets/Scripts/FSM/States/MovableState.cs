using UnityEngine;
using System.Collections;

[System.Serializable]
public class MovableState : State<MovableInteractable> {
	public override void enter (MovableInteractable entity) {
		entity.rigidbody.isKinematic = false;
	}
}
