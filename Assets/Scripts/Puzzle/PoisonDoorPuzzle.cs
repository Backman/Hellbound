using UnityEngine;
using System.Collections;

public class PoisonDoorPuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onDoorPoisonDrunk", onDoorPoisonDrunk);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void onDoorPoisonDrunk(GameObject thisObject, bool triggerOnlyForThis){
		gameObject.SetActive(false);
	}
}
