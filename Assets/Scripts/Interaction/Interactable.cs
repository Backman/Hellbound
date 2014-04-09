using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class Interactable : MonoBehaviour{
	public enum ActivateType{ OnTrigger, OnClick };
	public ActivateType m_ActivateType = ActivateType.OnClick;


//	public virtual void Start() {
//	}
//
//	public virtual void Update() {
//
//	}

	public void componentAction(string componentType) {
		//m_CurrentState.componentAction(componentType);

	}

	public virtual void pickUp()  { 
		//Object is picked up
		Debug.Log("Is picked up: " + gameObject.name );
	}

	public virtual void examine() { 
		//Object is examined
		Debug.Log("Is examined: " + gameObject.name );
	}

	public virtual void activate(){ 
		//Object is activated
	}
	
	public virtual void gainFocus(){
		//Apply light
		Debug.Log("Gaining focus: " + gameObject.name );
	}

	public virtual void loseFocus(){
		//Remove light
		Debug.Log("Leaving focus: " + gameObject.name );
	}
}
