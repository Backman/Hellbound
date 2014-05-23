using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Behaviour_ trigger simple door.
/// 
/// A trigger for manipulating simple doors
/// 
/// Created by Simon Jonasson
/// </summary>
public class Behaviour_TriggerSimpleDoor : MonoBehaviour
{
	public enum Action {Open, Close, Toggle, Lock, Unlock, UnlockAndOpen};
	public List<Behaviour_DoorSimple> m_DoorToAffect;

	[Tooltip("Which behaviour will this zone trigger on the door")]
	public Action m_Action = Action.Open;
	[Tooltip("Regulates if this zone can be triggered more than once")]
	public bool m_OneShot = true;
	[Tooltip("Which tag must the GameObject have to be able to trigger this zone")]
	public string m_ColliderTag = "Player";

	private bool m_Used = false;

	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag && !(m_Used & m_OneShot) ){
			PuzzleEvent.trigger("onTriggerEnter", gameObject, true);
			foreach( Behaviour_DoorSimple door in m_DoorToAffect ){
				m_Used = true;
				switch( m_Action ){
				case Action.Close:
					m_Used = door.close();
					break;
				case Action.Open:
					m_Used = door.open();
					break;
				case Action.Toggle:
					door.toggle();
					break;
				case Action.Lock:
					door.lockDoor();
					break;
				case Action.Unlock:
					door.unlockDoor();
					break;
				case Action.UnlockAndOpen:
					door.unlockAndOpen();
					break;
				}				
			}
		}
	}

}

