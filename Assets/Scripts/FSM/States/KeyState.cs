using UnityEngine;
using System.Collections;

[System.Serializable]
public class KeyState : State<KeyInteractable> {
	public override void pickUp (KeyInteractable entity)
	{
//TODO: INV_	Inventory.getInstance().addInteractable(entity);

		entity.gameObject.SetActive (false);
		Messenger.Broadcast("clear focus");
	}
}

