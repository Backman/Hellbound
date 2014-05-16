using UnityEngine;
using System.Collections;

[System.Serializable]
public class KeyState : State<Behaviour_PickUp> {
	public override void activate (Behaviour_PickUp entity)
	{
		base.activate (entity);
		InventoryLogic.Instance.addItem( entity.m_ItemName, entity.m_ItemThumbnail );

		entity.gameObject.SetActive (false);
		Messenger.Broadcast("clear focus");
	}
}

