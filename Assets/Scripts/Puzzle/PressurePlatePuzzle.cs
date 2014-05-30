using UnityEngine;
using System.Collections;

/// <summary>
/// Puzzle logic to set the correct
/// plate formations.
/// By Aleksi Lindeman
/// </summary>

public class PressurePlatePuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("requestSetFireFormation", requestSetFireFormation);
		Messenger.AddListener<GameObject, bool>("requestSetEarthFormation", requestSetEarthFormation);
		Messenger.AddListener<GameObject, bool>("requestSetWindFormation", requestSetWindFormation);
		Messenger.AddListener<GameObject, bool>("requestSetWaterFormation", requestSetWaterFormation);
		Messenger.AddListener<GameObject, bool>("requestLowerCeiling", requestLowerCeiling);	
		Messenger.AddListener<GameObject, bool>("requestStartRoof", requestStartRoof);
	}

	public void requestSetFireFormation(GameObject obj, bool tr){
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Fire" || name == "Water"){
				interObj.setPuzzleState("bad");
			}
			else{
				interObj.setPuzzleState("good");
			}
			if(interObj.gameObject == obj) interObj.setPuzzleState("good");
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}

	public void requestSetEarthFormation(GameObject obj, bool tr){
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Fire" || name == "Earth"){
				interObj.setPuzzleState("bad");
			}
			else{
				interObj.setPuzzleState("good");
			}
			if(interObj.gameObject == obj) interObj.setPuzzleState("good");
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}

	public void requestSetWindFormation(GameObject obj, bool tr){
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Earth" || name == "Fire"){
				interObj.setPuzzleState("bad");
			}
			else{
				interObj.setPuzzleState("good");
			}
			if(interObj.gameObject == obj) interObj.setPuzzleState("good");
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}
	
	public void requestSetWaterFormation(GameObject obj, bool tr){
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Earth" || name == "Water"){
				interObj.setPuzzleState("bad");
			}
			else{
				interObj.setPuzzleState("good");
			}
			if(interObj.gameObject == obj) interObj.setPuzzleState("good");
		}

		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}
	
	public void requestLowerCeiling(GameObject obj, bool tr){
		Messenger.Broadcast ("lower roof");
		PuzzleEvent.cancel("onTriggerEnter");
	}

	public void requestStartRoof(GameObject obj, bool tr){
		Messenger.Broadcast ("start roof");
		PuzzleEvent.cancel("onTriggerEnter");
	}
}
