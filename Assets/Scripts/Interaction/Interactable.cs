using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


/// <summary>
/// Interactable.
/// Base class for all interactable objects in the game
/// Each interactable overrides the functions it wishes to implement
/// 
/// Created by Simon
/// 
/// 
/// Added eventsound to interactable, it will play a specifik sound
/// if "EventSound" is added to the object
/// 
/// Anton Thorsell
/// </summary>
public abstract class Interactable : MonoBehaviour{
	public enum ActivateType{ OnTrigger, OnClick };
	public ActivateType m_ActivateType = ActivateType.OnClick;
	public InventoryItem m_InventoryItem;
	public EventSound m_EventSound;

	
	public void componentAction(string componentType) {
		//m_CurrentState.componentAction(componentType);
		
		
	}
	
	
	protected virtual void Start() {
		m_EventSound = gameObject.GetComponent<EventSound> ();
		if (m_EventSound != null) {
			m_EventSound = new EventSound();
		}

	}
	
	
	
	
	public virtual void pickUp()  { 
		Debug.Log("Is picked up: " + gameObject.name );
		if (m_EventSound.m_PickUp) { 
				FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathPickUp, gameObject.transform.position);
		}

	}
	
	
	public virtual void examine() {
		if (m_EventSound.m_Examine) { 
			FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathExamine, gameObject.transform.position);
		}
	}
	
	
	public virtual void activate(){ 
		if (m_EventSound.m_Activate) { 
			FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathActivate, gameObject.transform.position);
		}
	}
	
	
	/// <summary>
	/// Use parameter object on the interactable
	/// </summary>
	/// <param name="obj">Object to use on the interactable</param>
	public virtual void useWith(GameObject obj){}
	
	
	public virtual void gainFocus(){
		//Apply light
		Messenger.Broadcast<GameObject> ("onFocus", gameObject);
		Debug.Log("Gaining focus: " + gameObject.name );
		if (m_EventSound.m_GainFocus) { 
			FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathGainFocus, gameObject.transform.position);
		}
	}
	
	
	public virtual void loseFocus(){
		//Remove light
		Messenger.Broadcast ("leaveFocus");
		Debug.Log("Leaving focus: " + gameObject.name );
		if (m_EventSound.m_LoseFocus) { 
			FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_LoseFocus, gameObject.transform.position);
		}
	}
}