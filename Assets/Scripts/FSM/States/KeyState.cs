using UnityEngine;
using System.Collections;

[System.Serializable]
public class KeyState : State<KeyInteractable> {
	public override void pickUp (KeyInteractable entity)
	{
		Inventory.getInstance().addInteractable(entity.m_InventoryItem, entity);
		entity.gameObject.SetActive (false);
	}
}

