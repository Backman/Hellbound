using UnityEngine;
using System.Collections;

public class AntidotePoisonPuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onDrinkAntidote", onDrinkAntidote);
		Messenger.AddListener<GameObject, bool>("onDrinkPoison", onDrinkPoison);
		Messenger.AddListener<GameObject, bool>("openDoor", openDoor);
	}

	public void onDrinkAntidote(GameObject antidote, bool tr){
		Interactable inter = antidote.GetComponent<Interactable>();
		inter.setPuzzleState("unavailable");
		antidote.SetActive (false);
		Debug.Log("You drank antidote!");
		Messenger.Broadcast ("clear focus");
	}
	
	public void onDrinkPoison(GameObject poison, bool tr){
		Interactable inter = poison.GetComponent<Interactable>();
		inter.setPuzzleState("unavailable");
		poison.SetActive (false);
		Debug.Log("You drank poison!");
		Messenger.Broadcast ("clear focus");
	}

	public void openDoor(GameObject door, bool tr) {
		Debug.Log ("Open door");
		door.GetComponent<Behaviour_DoorSimple>().unlockAndOpen();
	}
}
