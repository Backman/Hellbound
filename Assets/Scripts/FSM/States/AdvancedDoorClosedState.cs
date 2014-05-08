using UnityEngine;
using System.Collections;

/// <summary>
/// Simple door closed state.
/// 
/// This state represents a simple door which is closed
/// and can be swung open.
/// 
/// Created by Simon
/// </summary>
[System.Serializable]
public class AdvancedDoorClosedState : State<Behaviour_DoorAdvanced> {

	public override void activate (Behaviour_DoorAdvanced entity)
	{
		base.activate (entity);
		entity.Used = true;
		entity.m_Moving = true;
		entity.Tweener.PlayForward();
	}
}