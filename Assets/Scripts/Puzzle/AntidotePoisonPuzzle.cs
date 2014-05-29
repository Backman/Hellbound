using UnityEngine;
using System.Collections;

/// <summary>
/// Puzzle logic for the Antidote puzzle
/// in the pestilence crypt
/// By Aleksi Lindeman
/// Modified by Arvid Backman
/// </summary>

public class AntidotePoisonPuzzle : MonoBehaviour {

	public string m_DeathText = "You suck";

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onDrinkAntidote", onDrinkAntidote);
		Messenger.AddListener<GameObject, bool>("onDrinkPoison", onDrinkPoison);
		Messenger.AddListener<GameObject, bool> ("onQueuePoisonAnimation", onQueuePoisonAnimation);
		Messenger.AddListener<GameObject, bool>("openDoor", openDoor);
	}

	public void onDrinkAntidote(GameObject antidote, bool tr){
		Messenger.Broadcast ("clear focus");
		Interactable inter = antidote.GetComponent<Interactable>();
		inter.setPuzzleState("unavailable");
		antidote.SetActive (false);
		StartCoroutine("stopSickness");
		Messenger.Broadcast<bool>("set is poisoned", false);
	}

	public void onQueuePoisonAnimation(GameObject poison, bool tr){
		Messenger.Broadcast ("drank poison");

		Messenger.Broadcast ("clear focus");
	}
	
	public void onDrinkPoison(GameObject poison, bool tr){
		Messenger.Broadcast ("clear focus");
		Interactable inter = poison.GetComponent<Interactable>();
		inter.setPuzzleState("unavailable");
		poison.SetActive (false);
		GUIManager.Instance.loadLastCheckPoint(m_DeathText);
	}

	public void openDoor(GameObject door, bool tr) {
		Debug.Log ("Open door");
		door.GetComponent<Behaviour_DoorSimple>().unlockAndOpen();
	}

	IEnumerator stopSickness() {
		float t = 1.0f;
		MotionBlur motionBlur = Camera.main.GetComponent<MotionBlur>();
		CameraBlur blur = Camera.main.GetComponent<CameraBlur> ();
		while(t > 0.0f) {
			motionBlur.blurAmount = t * 0.8f;
			blur.setBlurPercentage(t);
			t -= Time.deltaTime;
			
			yield return null;
		}
		motionBlur.blurAmount = 0.0f;
		motionBlur.enabled = false;
		blur.setBlurPercentage (0.0f);
		blur.enabled = false;
	}
}
