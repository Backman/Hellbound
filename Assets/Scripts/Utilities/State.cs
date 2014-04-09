using UnityEngine;
using System.Collections;

[System.Serializable]
public class State {
	public bool		m_CanBeFocused = true;
	public Component m_FocusScript 	= null;
	
	public bool m_CanBeUsed = false;
	public Component m_ActivateScript = null;
	
	public bool m_CanBeExamined = true;
	public Component m_ExamineScript = null;
	
	public bool		 m_CanBePickedUp = false;
	public Component m_PickUpScript = null;
	
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