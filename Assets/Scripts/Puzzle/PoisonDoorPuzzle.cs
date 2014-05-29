using UnityEngine;
using System.Collections;

/// <summary>
/// Logic to open a door and start
/// "sickness" when the poison has been used
/// By Aleksi Lindeman
/// Modified by Arvid Backman, Simon Jonasson
/// </summary>

public class PoisonDoorPuzzle : MonoBehaviour {
	public FMOD_StudioEventEmitter m_MusicEmitter = null;

	private Transform r_Bottle;
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
		Debug.Log ("1");
		Messenger.Broadcast ("clear focus");

		Interactable inter = obj.GetComponent<Interactable> ();
		inter.setPuzzleState("inUse");
		obj.collider.enabled = false;

		//Move the bottle to the players hand
		r_Bottle = obj.transform.GetChild (0).transform;
		r_Bottle.parent = GameObject.FindGameObjectWithTag("RightMiddleFinger").transform;
		r_Bottle.transform.localRotation = Quaternion.Euler (-23.90005f, -137.1001f, -27.20001f);
		r_Bottle.transform.localPosition = new Vector3 (-0.004753934f, -0.07229548f, -0.08756122f);

	}
	//Called at the peak of the "drink" animation
	public void onDrinkDoorPoison(GameObject obj, bool tr) {
		Debug.Log ("2");
		Interactable inter = obj.GetComponent<Interactable> ();
		inter.setPuzzleState ("used");
	}

	//Called at the end of the "drink" animation
	public void onBottleRemove(GameObject obj, bool tr){
		Debug.Log ("3");
		//GameObject.Destroy (r_Bottle.gameObject);
		//GameObject.Destroy (obj);
		r_Bottle.gameObject.SetActive(false);
		obj.SetActive(false);
	}

	//Called at the end of the "drink" animation
	public void onDoorPoisonDrunk(GameObject thisObject, bool triggerOnlyForThis){
		Messenger.Broadcast<bool>("set is poisoned", true);
		StartCoroutine("startSickness", gameObject );
		Debug.Log ("Starting Sickness..");
		if(m_MusicEmitter != null) {
			m_MusicEmitter.Play ();
		}
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
