using UnityEngine;
using System.Collections;

/// <summary>
/// Simon test pussle.
/// 
/// A script for handeling a test of our puzzle logic.
/// 
/// The player needs to pick upp a few objects to open a door.
/// Behind that door, there is a few new object, which are needed to open the second door.
/// When the second door is opened, a message is printed.
/// 
/// Created by Simon Jonasson
/// </summary>
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
		Debug.Log("Test completed");

	}
}
