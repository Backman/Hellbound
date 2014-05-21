using UnityEngine;
using System.Collections;

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
