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
		StartCoroutine("startSickness", gameObject );
	}

	IEnumerator startSickness(GameObject doorToOpen) {
		yield return new WaitForSeconds (1.0f);
		gameObject.GetComponent<Behaviour_DoorSimple>().unlockAndOpen();

		float t = 0.0f;
		MotionBlur motionBlur = Camera.main.GetComponent<MotionBlur>();
		motionBlur.enabled = true;
		 
		CameraBlur blur = Camera.main.GetComponent<CameraBlur> ();
		blur.enabled = true;

		while(t < 1.0f) {
			motionBlur.blurAmount = t * 0.8f;
			blur.setBlurPercentage(t);
			
			t += Time.deltaTime;

			yield return null;
		}
		motionBlur.blurAmount = 0.8f;
		blur.setBlurPercentage (1.0f);
	}
}
