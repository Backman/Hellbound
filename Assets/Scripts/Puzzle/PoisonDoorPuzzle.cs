using UnityEngine;
using System.Collections;

public class PoisonDoorPuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onDoorPoisonDrunk", onDoorPoisonDrunk);
		Messenger.AddListener<GameObject, bool>("onDrinkDoorPoison", onDrinkDoorPoison);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onDrinkDoorPoison(GameObject obj, bool tr) {
		obj.SetActive (false);
		Messenger.Broadcast("clear focus");
	}
	
	public void onDoorPoisonDrunk(GameObject thisObject, bool triggerOnlyForThis){
		gameObject.SetActive(false);
	}
}
