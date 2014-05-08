using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Behaviour_ trigger simple door.
/// 
/// A trigger for manipulating simple doors
/// 
/// Created by Simon
/// </summary>
public class Behaviour_TriggerSimpleDoor : MonoBehaviour
{
	public enum Action {Open, Close, Toggle, Lock, Unlock};
	public List<Behaviour_DoorSimple> m_DoorToAffect;

	public Action m_Action = Action.Open;
	public bool m_OneShot = true;
	
	public string m_ColliderTag = "Player";

	private bool m_Used = false;

	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag && !(m_Used & m_OneShot) ){
			PuzzleEvent.trigger("onTriggerEnter", gameObject, true);
			foreach( Behaviour_DoorSimple door in m_DoorToAffect ){
				switch( m_Action ){
				case Action.Close:
					m_Used = door.close();
					break;
				case Action.Open:
					m_Used = door.open();
					break;
				case Action.Toggle:
					door.toggle();
					m_Used = true;
					break;
				case Action.Lock:
					door.lockDoor();
					m_Used = true;
					break;
				case Action.Unlock:
					door.unlockDoor();
					m_Used = true;
					break;
				}				
			}
		}
	}

}

