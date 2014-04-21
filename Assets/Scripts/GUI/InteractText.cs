using UnityEngine;
using System.Collections;

public class InteractText : MonoBehaviour {

	public FloatingText m_ExamineText;
	public FloatingText m_PickupText;

	private bool m_HasExamine = true;
	private bool m_HasPickup = true;

	public bool HasExamine {
		get { return m_HasExamine; }
		set {
			m_HasExamine = value;
			if(!m_HasExamine){
				m_ExamineText.gameObject.SetActive(false);
			} else {
				m_ExamineText.gameObject.SetActive(true);
			}
		}
	}

	public bool HasPickup {
		get { return m_HasPickup; }
		set {
			m_HasPickup = value;
			if(!m_HasPickup){
				m_PickupText.gameObject.SetActive(false);
			} else {
				m_PickupText.gameObject.SetActive(true);
			}
		}
	}
}
