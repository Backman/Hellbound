using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class UseState : State<Behaviour_OnlyUse> {
	public List<Interactable> m_InteractablesToAffect = new List<Interactable>();

	public override void activate (Behaviour_OnlyUse entity)
	{
		foreach(Interactable obj in m_InteractablesToAffect) {
			obj.useWith (entity.gameObject);
		}

		entity.gameObject.SetActive(false);
		//GameObject.Destroy(entity.gameObject);
	}
}
