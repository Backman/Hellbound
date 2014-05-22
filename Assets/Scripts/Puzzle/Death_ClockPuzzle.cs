using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that handles the logic in the
/// "clock" puzzle in the death crypt
/// 
/// By Arvid Backman
/// </summary>

public class Death_ClockPuzzle : MonoBehaviour {

	public List<BoxCollider> m_Triggers = new List<BoxCollider>();

	private int m_CurrentIndex = 0;

	void Awake() {
		Messenger.AddListener<GameObject, bool>("onPickupClock", onPickupClock);
		Messenger.AddListener<GameObject, bool>("onClockPickedUp", onClockPickedUp);
		Messenger.AddListener<GameObject, bool>("onKillNPC", onKillNPC);
	}

	public void onPickupClock(GameObject go, bool tr) {
		Interactable inter = go.GetComponent<Interactable>();
		if(inter != null) {
			//TODO Start ticking sound
			inter.setPuzzleState("pickedUp");
		}
	}

	public void onClockPickedUp(GameObject go, bool tr) {
		Behaviour_DoorSimple door = go.GetComponent<Behaviour_DoorSimple>();
		if(door != null) {
			door.unlockAndOpen();
		}
	}

	public void onKillNPC(GameObject go, bool tr) {

		KillNPCTrigger trigger = go.GetComponent<KillNPCTrigger>();
		if(trigger != null) {
			GameObject coffin = trigger.m_Coffin;
			Transform coffinPos = coffin.transform;

			FMODAsset asset = trigger.m_FMODAsset;
			if(asset != null) {
				FMOD_StudioSystem.instance.PlayOneShot(asset, coffinPos.position);
			}
			coffin.rigidbody.useGravity = true;
			go.SetActive(false);
		}
	}
}
