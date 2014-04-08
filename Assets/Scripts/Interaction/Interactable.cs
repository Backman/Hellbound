using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Interactable: MonoBehaviour{
	public enum ActivateType{ OnTrigger, OnClick };
	public ActivateType m_ActivateType = ActivateType.OnClick;

	public State m_DefaultState;
	protected State m_CurrentState;
	[SerializeField] private List<PrerequisiteState> m_PrerequisiteStates;



	protected void Start() {
		m_CurrentState = m_DefaultState;
	}

	protected void Update() {
		foreach(PrerequisiteState ps in m_PrerequisiteStates) {
			if(ps.conditionsMet()) {
				m_CurrentState = ps as State;
				return;
			}
		}
		m_CurrentState = m_DefaultState;
	}

	public void componentAction(string componentType) {
		//m_CurrentState.componentAction(componentType);
	}

	public void pickUp()  { 
		//Object is picked up
		Debug.Log("Is picked up: " + gameObject.name );
		m_CurrentState.pickUp();  
	}

	public void examine() { 
		//Object is examined
		Debug.Log("Is examined: " + gameObject.name );
		m_CurrentState.examine(); 
	}

	public void activate(){ 
		//Object is activated
		Debug.Log("Is activated: " + gameObject.name );
		if(m_CurrentState == m_DefaultState)
			Debug.Log ("Default state!");
		else
			Debug.Log ("Other state!");
		m_CurrentState.activate();
	}
	
	public void gainFocus(){
		//Apply light
		Debug.Log("Gaining focus: " + gameObject.name );
		m_CurrentState.gainFocus();
	}
	public void loseFocus(){
		//Remove light
		Debug.Log("Leaving focus: " + gameObject.name );
		m_CurrentState.loseFocus();
	}

	void OnDrawGizmos() {
		// TODO: Draw gizmos between all conditions
		/*foreach(PrerequisiteState ps in m_PrerequisiteStates) {
			foreach(Condition c in ps.m_Conditions) {
				Gizmos.DrawLine(transform.position, c.transform.position);
			}
		}*/
	}

	void OnTriggerEnter(Collider col){
		//TODO: Detect type of collider
		if( m_ActivateType == ActivateType.OnTrigger ){
			m_CurrentState.activate();
		}
	}

}
