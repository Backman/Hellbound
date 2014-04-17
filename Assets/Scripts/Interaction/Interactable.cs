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
/// </summary>
public abstract class Interactable : MonoBehaviour{
	public enum ActivateType{ OnTrigger, OnClick };
	public ActivateType m_ActivateType = ActivateType.OnClick;
<<<<<<< HEAD
	private EventSound m_EventSound;


=======
	public InventoryItem m_InventoryItem;
>>>>>>> 44b51e53a55996846fda79e9161535c88f13fb71

	private Shader m_OutlineShader;
	private Shader m_OriginalShader;
	private Material m_OutlineMaterial;
	private Material m_OriginalMaterial;


	protected virtual void Start(){
		m_OutlineMaterial = new Material(renderer.material);
		m_OutlineMaterial.shader = Shader.Find ("Hidden/Outline");
		m_OriginalMaterial = new Material(renderer.material);
		m_OriginalShader = m_OriginalMaterial.shader;
	}


	public virtual void pickUp()  { 
		Debug.Log("Is picked up: " + gameObject.name );
		if (m_EventSound.m_PickUp  && m_EventSound != null) { 
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathPickUp, gameObject.transform.position);
		}
	}

	public virtual void examine() { 
		//Object is examined
		Debug.Log("Is examined: " + gameObject.name );
		if (m_EventSound.m_Examine  && m_EventSound != null) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathExamine, gameObject.transform.position);
		}
	}

<<<<<<< HEAD
	public virtual void activate(){ 
				//Object is activated
		if (m_EventSound.m_Activate  && m_EventSound != null) {
						FMOD_StudioSystem.instance.PlayOneShot (m_EventSound.m_PathActivate, gameObject.transform.position);
				}
		}
=======
	public virtual void activate(){ }
>>>>>>> 44b51e53a55996846fda79e9161535c88f13fb71

	/// <summary>
	/// Use parameter object on the interactable
	/// </summary>
	/// <param name="obj">Object to use on the interactable</param>
	public virtual void useWith(GameObject obj){}
	
	public virtual void gainFocus(){
		//Apply light
		m_OriginalMaterial = renderer.material;
		renderer.material = m_OutlineMaterial;
		Debug.Log("Gaining focus: " + gameObject.name );
		if (m_EventSound.m_GainFocus  && m_EventSound != null) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathGainFocus, gameObject.transform.position);
		}
	}

	public virtual void loseFocus(){
		//Remove light
		renderer.material = m_OriginalMaterial;
		Debug.Log("Leaving focus: " + gameObject.name );
		if (m_EventSound.m_LoseFocus  && m_EventSound != null) {
			FMOD_StudioSystem.instance.PlayOneShot( m_EventSound.m_PathLoseFocus, gameObject.transform.position);
		}
	}
}
