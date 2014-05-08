using UnityEngine;
using System.Collections;

/// <summary>
/// Simple door opened state.
/// 
/// This state represents a simple door which is opened
/// and can be swung close.
/// 
/// Created by Simon
/// </summary>
[System.Serializable]
public class AdvancedDoorOpenedState : State<Behaviour_DoorAdvanced> {

	public override void activate (Behaviour_DoorAdvanced entity)
	{
		base.activate (entity);
		entity.m_Moving = true;
		entity.Tweener.PlayReverse();
	}
}

