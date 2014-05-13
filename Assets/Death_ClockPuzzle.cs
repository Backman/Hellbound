using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Death_ClockPuzzle : MonoBehaviour {

	public List<BoxCollider> m_Triggers = new List<BoxCollider>();

	private int m_CurrentIndex = 0;

	void Awake() {
		Messenger.AddListener<GameObject, bool>("onPickupClock", onPickupClock);
		Messenger.AddListener<GameObject, bool>("onClockPickedUp", onClockPickedUp);
		Messenger.AddListener<GameObject, bool>("onPickUpKey", onPickUpKey);
		Messenger.AddListener<GameObject, bool>("onKillNPC", onKillNPC);
	}

	public void onPickupClock(GameObject go, bool tr) {
		Interactable inter = go.GetComponent<Interactable>();
		if(inter != null) {
			inter.setPuzzleState("pickedUp");
		}
	}

	public void onClockPickedUp(GameObject go, bool tr) {
		Behaviour_DoorSimple door = go.GetComponent<Behaviour_DoorSimple>();
		if(door != null) {
			door.unlockAndOpen();
		}
	}

	public void onPickUpKey(GameObject go, bool tr) {
		Interactable inter = go.GetComponent<Interactable>();
		if(inter != null) {
			inter.setPuzzleState("pickedUp");

		}
	}

	public void onKillNPC(GameObject go, bool tr) {
		Debug.Log("Play the pancy scream/kill sound");
		KillNPCTrigger trigger = go.GetComponent<KillNPCTrigger>();
		if(trigger != null) {
			FMODAsset asset = trigger.m_FMODAsset;
			Transform emitterPos = trigger.m_EmitterPosition;
			if(asset != null) {
				FMOD_StudioSystem.instance.PlayOneShot(asset, emitterPos.position);
			}
			go.SetActive(false);
			trigger.Used = true;
		}
	}
}
