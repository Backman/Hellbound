using UnityEngine;
using System.Collections;

[System.Serializable]
public class MovableState : State<MovableInteractable> {
	public override void enter (MovableInteractable entity) {
		entity.rigidbody.isKinematic = false;
	}

	public override void reason (MovableInteractable entity) {
		if(!entity.Movable){
			entity.m_FSM.changeState<ImmovableState>();
		}
	}
}
