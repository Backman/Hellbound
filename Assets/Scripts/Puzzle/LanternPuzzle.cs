using UnityEngine;
using System.Collections;

/// <summary>
/// Logic to open the door in the war crypt
/// by hanging lanterns on correct hooks
/// By Aleksi Lindeman
/// Modified by Arvid Backman
/// </summary>

public class LanternPuzzle : MonoBehaviour {

	private int lanternsPlaced = 0;

	void Start () {
		Messenger.AddListener<GameObject, bool>("onPickupLantern", onPickupLantern);
		Messenger.AddListener<GameObject, bool>("onLanternPlaced", onLanternPlaced);
		Messenger.AddListener<GameObject, bool>("removeLanternFromInventory", removeLanternFromInventory);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void onPickupLantern(GameObject obj, bool tr){
		Interactable inter = obj.GetComponent<Interactable>();
		if(inter != null){
			inter.setPuzzleState("readyToUse");
		}
	}
	
	public void onLanternPlaced(GameObject obj, bool tr){
		Interactable inter = obj.GetComponent<Interactable>();
		if(inter != null && inter.getPuzzleState() != "used"){
			inter.setPuzzleState("used");
			((LanternHook)inter).open ();
			++lanternsPlaced;
			PuzzleEvent.trigger("requestRemoveLanternFromInventory", obj, false);
			if(lanternsPlaced == 2){
				Behaviour_DoorSimple[] doors = GetComponentsInChildren<Behaviour_DoorSimple>();
				foreach(Behaviour_DoorSimple door in doors) {
					door.unlockAndOpen();
				}
			}
		}
	}

	public void removeLanternFromInventory(GameObject obj, bool tr){
		Behaviour_PickUp inter = obj.GetComponent<Behaviour_PickUp>();
		InventoryLogic.Instance.removeItem(inter.m_ItemName);
		inter.setPuzzleState("used");
		PuzzleEvent.cancel("requestRemoveLanternFromInventory");
	}
}
