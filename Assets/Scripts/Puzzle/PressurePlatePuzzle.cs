using UnityEngine;
using System.Collections;

public class PressurePlatePuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("requestSetFireFormation", requestSetFireFormation);
		Messenger.AddListener<GameObject, bool>("requestSetEarthFormation", requestSetEarthFormation);
		Messenger.AddListener<GameObject, bool>("requestSetWindFormation", requestSetWindFormation);
		Messenger.AddListener<GameObject, bool>("requestSetWaterFormation", requestSetWaterFormation);
		Messenger.AddListener<GameObject, bool>("requestLowerCeiling", requestLowerCeiling);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void requestSetFireFormation(GameObject obj, bool tr){
		Debug.Log("Set fire formation!");
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Fire" || name == "Water"){
				interObj.setPuzzleState("bad");
				//Debug.Log("Puzzle state for wind set to bad!");
			}
			else{
				interObj.setPuzzleState("good");
			}
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}

	public void requestSetEarthFormation(GameObject obj, bool tr){
		Debug.Log("Set earth formation!");
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Fire" || name == "Earth"){
				interObj.setPuzzleState("bad");
				//Debug.Log("Puzzle state for wind set to bad!");
			}
			else{
				interObj.setPuzzleState("good");
			}
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}

	public void requestSetWindFormation(GameObject obj, bool tr){
		Debug.Log("Set wind formation!");
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Earth" || name == "Fire"){
				interObj.setPuzzleState("bad");
				//Debug.Log("Puzzle state for wind set to bad!");
			}
			else{
				interObj.setPuzzleState("good");
			}
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}
	
	public void requestSetWaterFormation(GameObject obj, bool tr){
		Debug.Log("Set water formation!");
		Interactable[] interactableObjects = gameObject.GetComponentsInChildren<Interactable>();
		foreach(Interactable interObj in interactableObjects){
			string name = interObj.name;
			if(name == "Wind" || name == "Earth" || name == "Water"){
				interObj.setPuzzleState("bad");
				//Debug.Log("Puzzle state for wind set to bad!");
			}
			else{
				interObj.setPuzzleState("good");
			}
		}
		// Cancel event to prevent other logic with different condition to run
		PuzzleEvent.cancel("onTriggerEnter");
	}
	
	public void requestLowerCeiling(GameObject obj, bool tr){
		Debug.Log("Lower ceiling!");
	}
}
