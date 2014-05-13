﻿using UnityEngine;
using System.Collections;

public class Death_KeyPuzzle : MonoBehaviour {
	private string m_KeyItemName;
	void Awake() {
		Messenger.AddListener<GameObject, bool>("onUseKeyWithDoor", onUseKeyWithDoor);
		Messenger.AddListener<GameObject, bool>("onKeyPickup", onKeyPickup);
	}

	public void onUseKeyWithDoor(GameObject go, bool tr) {
		go.GetComponent<Behaviour_DoorSimple>().unlockAndOpen();
		InventoryLogic.Instance.removeItem(m_KeyItemName);
	}

	public void onKeyPickup(GameObject go, bool tr) {
		Interactable inter = go.GetComponent<Interactable>();
		if(inter != null) {
			inter.setPuzzleState("pickedUp");
			m_KeyItemName = ((Behaviour_PickUp)inter).m_ItemName;
		}
	}
}
