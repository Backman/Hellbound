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
/// 
/// Added eventsound to interactable, it will play a specifik sound
/// if "EventSound" is added to the object
/// 
/// _minor_ editing by Anton Thorsell
/// </summary>
public abstract class Interactable : MonoBehaviour{

	public EventSound m_EventSound;

	[HideInInspector] public string m_Description;
	[HideInInspector] public string m_UseText;
	[Tooltip("This field is best left alone. The programmers reserve this field to themselves.\n" +
	         "Unless you have explicit knowledge about this field, DO NOT MODIFY.")]
	public string m_PuzzleState = "";

	protected bool m_Usable = false;
	public bool Usable {
		get { return m_Usable; }
	}

	protected virtual void Awake() {
		m_EventSound = gameObject.GetComponent<EventSound> ();
		if (m_EventSound == null) {
			Debug.Log ("No event sound. Creating a new one!");
			m_EventSound = gameObject.AddComponent<EventSound>();
		}
	}
		
	protected virtual void Start() {}
	
	public virtual void pickUp()  { 
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
		Debug.Log("Gaining focus: " + gameObject.name );
		Messenger.Broadcast<GameObject> ("onFocus", gameObject);

		if (m_EventSound != null) {
			if (m_EventSound.m_GainFocus) { 
				FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathGainFocus, gameObject.transform.position);
			}
		}
	}
	
	
	public virtual void loseFocus(){
		//Remove light
		Debug.Log("Leaving focus: " + gameObject.name );
		Messenger.Broadcast ("leaveFocus");

		if (m_EventSound != null) {
			if (m_EventSound.m_LoseFocus) { 
				FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathLoseFocus, gameObject.transform.position);
			}
		}
	}

	public string getPuzzleState(){
		return m_PuzzleState;
	}

	public void setPuzzleState(string state) {
		m_PuzzleState = state;
	}
}
