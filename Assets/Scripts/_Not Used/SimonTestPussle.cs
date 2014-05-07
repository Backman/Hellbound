using UnityEngine;
using System.Collections;

public class SimonTestPussle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onPickUpKey", onPickup);
		Messenger.AddListener<GameObject, bool>("onOpenDoorFromLocked", onOpenFromLocked);
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
			TweenRotation t = obj.AddComponent<TweenRotation>();
			t.from = obj.transform.rotation.eulerAngles;
			t.to = t.from;
			t.to.x += 90;
			t.Play();

			inter.setPuzzleState("open");

			
		}
	}
}
