using UnityEngine;
using System.Collections;

public class LanternPuzzle : MonoBehaviour {
	
	private int lanternsPlaced = 0;
	// Use this for initialization
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
			++lanternsPlaced;
			PuzzleEvent.trigger("requestRemoveLanternFromInventory", obj, false);
			if(lanternsPlaced == 2){
				Destroy(gameObject);
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
