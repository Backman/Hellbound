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
	public InventoryItem m_InventoryItem;

	public void componentAction(string componentType) {
		//m_CurrentState.componentAction(componentType);

	}

	protected virtual void Start() {}


	public virtual void pickUp()  { 
		Debug.Log("Is picked up: " + gameObject.name );
	}

	public virtual void examine() {

	}

	public virtual void activate(){ }

	/// <summary>
	/// Use parameter object on the interactable
	/// </summary>
	/// <param name="obj">Object to use on the interactable</param>
	public virtual void useWith(GameObject obj){}
	
	public virtual void gainFocus(){
		//Apply light
		Messenger.Broadcast<GameObject> ("onFocus", gameObject);
		Debug.Log("Gaining focus: " + gameObject.name );
	}

	public virtual void loseFocus(){
		//Remove light
		Messenger.Broadcast ("leaveFocus");
		Debug.Log("Leaving focus: " + gameObject.name );
	}
}
