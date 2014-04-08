using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

<<<<<<< HEAD
[System.Serializable]
public class ConditionDictionary {
	[SerializeField] private List<Condition> m_Conditions;
	[SerializeField] private List<bool> m_Values;

	public ConditionDictionary() {
		m_Conditions = new List<Condition>();
		m_Values = new List<bool>();
	}

	public Condition[] Keys
	{
		get { return m_Conditions.ToArray(); }
	}

	public bool[] Values
	{
		get { return m_Values.ToArray(); }
	}
}

[System.Serializable]
public class State {
	public bool		m_CanBeFocused = true;
	public MonoBehaviour m_FocusScript 	= null;

	public bool m_CanBeUsed = false;
	public MonoBehaviour m_ActivateScript = null;

	public bool m_CanBeExamined = true;
	public MonoBehaviour m_ExamineScript = null;

	public bool		 m_CanBePickedUp = false;
	public MonoBehaviour m_PickUpScript = null;

	public void pickUp()  { 
		if(m_CanBePickedUp && m_PickUpScript != null) {
			/*m_PickUpScript.action();*/
		}
	}
	public void examine() {
		if(m_CanBeExamined && m_ExamineScript != null) {
			/*m_ExamineScript.action();*/
		}
	}
	public void activate(){
		if(m_CanBeUsed && m_ActivateScript != null) {
			/*m_ActivateScript.action();*/
		}
	}

	public void gainFocus(){ 
		if( m_FocusScript != null ){
			//TODO: Run focus script
			// m_FocusScript.action();
			Debug.Log("Run focus script");
		}
	}

	public void loseFocus(){ }
}

[System.Serializable]
public class PrerequisiteState : State {
	public ConditionDictionary m_Prerequisites;
	// public Dictionary<Condition, bool> m_Prerequisites;

	public bool conditionsMet() {
		int i = 0;
		foreach(Condition c in m_Prerequisites.Keys) {
			if(c.isMet != m_Prerequisites.Values[i]){
				return false;
			}
			i++;
		}
		return true;
	}
}

[ExecuteInEditMode]
=======
>>>>>>> c6171193ef3d67ff225c1fe400623d2a2700db9c
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
