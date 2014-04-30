using UnityEngine;
using System.Collections;

public class AntidotePoisonPuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onDrinkAntidote", onDrinkAntidote);
		Messenger.AddListener<GameObject, bool>("onDrinkPoison", onDrinkPoison);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void onDrinkAntidote(GameObject antidote, bool tr){
		GameObject parent = antidote.transform.parent.gameObject;
		foreach(Interactable inter in parent.GetComponentsInChildren<Interactable>()){
			inter.setPuzzleState("unavailable");
		}
		Debug.Log("You drank antidote!");
	}
	
	public void onDrinkPoison(GameObject poison, bool tr){
		GameObject parent = poison.transform.parent.gameObject;
		foreach(Interactable inter in parent.GetComponentsInChildren<Interactable>()){
			inter.setPuzzleState("unavailable");
		}
		Debug.Log("You drank poison!");
	}
}
