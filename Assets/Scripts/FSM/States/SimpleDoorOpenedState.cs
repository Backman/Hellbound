using UnityEngine;
using System.Collections;

/// <summary>
/// Simple door opened state.
/// 
/// This state represents a simple door which is opened
/// and can be swung close.
/// 
/// Created by Simon Jonasson
/// </summary>
[System.Serializable]
public class SimpleDoorOpenedState : State<Behaviour_DoorSimple> {

	public override void activate (Behaviour_DoorSimple entity)
	{
		base.activate (entity);
		entity.m_Moving = true;
		entity.Tweener.playDirection = AnimationOrTween.Direction.Reverse;
		entity.Tweener.Play(true);
	}
}

