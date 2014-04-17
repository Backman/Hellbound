using UnityEngine;
using System.Collections;

[System.Serializable]
public class ImmovableState : State<MovableInteractable> {
	public override void enter (MovableInteractable entity) {
		entity.rigidbody.isKinematic = true;
	}
}
