using UnityEngine;
using System.Collections;

public class PoisonDoorPuzzle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool> ("onBottleRemove", onBottleRemove);
		Messenger.AddListener<GameObject, bool>("onMoveBottleToHand", onMoveBottleToHand);
		Messenger.AddListener<GameObject, bool>("onDoorPoisonDrunk", onDoorPoisonDrunk);
		Messenger.AddListener<GameObject, bool>("onDrinkDoorPoison", onDrinkDoorPoison);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Called at the beginning of the "drink" animation
	public void onMoveBottleToHand(GameObject obj, bool tr) {
		Messenger.Broadcast ("clear focus");

		Interactable inter = obj.GetComponent<Interactable> ();
		inter.setPuzzleState("inUse");

		//Move the bottle to the players hand
		obj.collider.enabled = false;
		obj.transform.parent = GameObject.FindGameObjectWithTag("RightMiddleFinger").transform;
		obj.transform.rotation = new Quaternion(0,0,0,0);
		obj.transform.localPosition = Vector3.zero;

	}
	//Called at the peak of the "drink" animation
	public void onDrinkDoorPoison(GameObject obj, bool tr) {
		Interactable inter = obj.GetComponent<Interactable> ();
		inter.setPuzzleState ("used");
	}

	//Called at the end of the "drink" animation
	public void onBottleRemove(GameObject obj, bool tr){
		GameObject.Destroy (obj);
	}

	//Called at the end of the "drink" animation
	public void onDoorPoisonDrunk(GameObject thisObject, bool triggerOnlyForThis){
		StartCoroutine("startSickness", gameObject );
	}

	//Will gradually apply sickness effect to the camera
	IEnumerator startSickness(GameObject doorToOpen) {
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
		gameObject.GetComponent<Behaviour_DoorSimple>().unlockAndOpen();
	}
}
