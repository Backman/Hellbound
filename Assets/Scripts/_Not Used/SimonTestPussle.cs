using UnityEngine;
using System.Collections;

public class SimonTestPussle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onPickUpKey", onPickup);
		Messenger.AddListener<GameObject, bool>("onOpenDoorFromLocked", onOpenFromLocked);
		Messenger.AddListener<GameObject, bool>("onOpenBackDoor", openBackDoor);
	}
	
	void onPickup(GameObject obj, bool tr){
		Interactable inter = obj.GetComponent<Interactable>();
		if( inter != null ){
			inter.setPuzzleState("pickedUp");
		}
	}

	void onOpenFromLocked(GameObject obj, bool tr){
		Interactable inter = obj.GetComponent<Interactable>();

		if( inter != null && inter.getPuzzleState() != "open" ){
			inter.setPuzzleState("open");
			obj.SetActive(false);

		}
	}

	void openBackDoor(GameObject obj, bool tr){
		Debug.Log("Hejs");

	}
}
