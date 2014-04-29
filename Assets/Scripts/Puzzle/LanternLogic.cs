using UnityEngine;
using System.Collections;

public class LanternLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onLanternPickup", onLanternPickup);
	}

	public void onLanternPickup(GameObject obj, bool tr){
		Interactable interObj = obj.GetComponent<Interactable>();
		interObj.setPuzzleState("pickedUp");
		Debug.Log("Picked up lantern");
	}
}
