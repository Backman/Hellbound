using UnityEngine;
using System.Collections;

/// <summary>
/// Logic to pick up and use the player lantern
/// By Arvid Backman
/// </summary>

 public class GraveyardLanternLogic : MonoBehaviour {

	void Start() {
		Messenger.AddListener<GameObject, bool>("pickUpLantern", pickUpLantern);
	}

	public void pickUpLantern(GameObject obj, bool tr){
		Interactable inter = obj.GetComponent<Interactable>();
		if(inter != null){
			Destroy (obj);
			GetComponent<Light>().intensity = 3.0f;
			Messenger.Broadcast("clear focus");
		}
	}
}
