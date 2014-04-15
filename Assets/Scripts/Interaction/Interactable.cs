using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class Interactable : MonoBehaviour{
	public enum ActivateType{ OnTrigger, OnClick };
	public ActivateType m_ActivateType = ActivateType.OnClick;
	private EventSound m_EventSound;

	public virtual void Start() {
		m_EventSound = gameObject.GetComponent<EventSound> ();
		if (m_EventSound == null) {
			m_EventSound = new EventSound();
		}
	}

//	public virtual void Update() {
//
//	}

	public void componentAction(string componentType) {
		//m_CurrentState.componentAction(componentType);

	}

	public virtual void pickUp()  { 
		//Object is picked up
		Debug.Log("Is picked up: " + gameObject.name );
		if (m_EventSound.m_PickUp) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathPickUp, gameObject.transform.position);
		}
	}

	public virtual void examine() { 
		//Object is examined
		Debug.Log("Is examined: " + gameObject.name );
		if (m_EventSound.m_Examine) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathExamine, gameObject.transform.position);
		}
	}

	public virtual void activate(){ 
		//Object is activated
		if (m_EventSound.m_Activate) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathActivate, gameObject.transform.position);
		}
	}
	
	public virtual void gainFocus(){
		//Apply light
		Debug.Log("Gaining focus: " + gameObject.name );
		if (m_EventSound.m_GainFocus) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathGainFocus, gameObject.transform.position);
		}
	}

	public virtual void loseFocus(){
		//Remove light
		Debug.Log("Leaving focus: " + gameObject.name );
		if (m_EventSound.m_LoseFocus) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathLoseFocus, gameObject.transform.position);
		}
	}
}
