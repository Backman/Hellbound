using UnityEngine;
using System.Collections;

/// <summary>
/// Pressure plate door.
/// 
/// The logic for the door barring the way to the pressure plate puzzle.
/// 
/// Created by Simon Jonasson
/// </summary>
public class PressurePlateDoor : MonoBehaviour {
	private Interactable r_Key;

	void Start () {
		Messenger.AddListener<GameObject, bool>("onKeyIsPickedUp", onKeyIsPickedUp);
		Messenger.AddListener<GameObject, bool>("requestRemovalKey", requestRemovalKey);
	}

	public void onKeyIsPickedUp	(GameObject obj, bool tr ){
		Interactable inter = obj.GetComponent<Interactable>();

		if(inter != null && inter.getPuzzleState() == "notPickedUp"){
			r_Key = inter;
			r_Key.setPuzzleState("pickedUp");

			gameObject.GetComponentInChildren<Behaviour_DoorSimple>().unlockDoor();
		}
	}

	public void requestRemovalKey(GameObject obj, bool tr){
		if( r_Key.getPuzzleState() == "pickedUp" ){
			Behaviour_PickUp pickUp = r_Key.GetComponent(typeof (Behaviour_PickUp) ) as Behaviour_PickUp;

			if( InventoryLogic.Instance.containsItem( pickUp.m_ItemName ) ){
				InventoryLogic.Instance.removeItem( pickUp.m_ItemName );
				r_Key.setPuzzleState("used");
			}
		}
	}
}
