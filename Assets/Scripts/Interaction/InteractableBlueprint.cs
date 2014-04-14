using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InteractableProperty {
	public bool m_CanBePickedUp = false;
	public bool m_CanBeUsed = false;
	public bool m_CanBeExamined = false;
}

[System.Serializable]
public class InteractableBlueprint {
	public string m_Name;
	public string m_Description;

	public string iconName = "";
	public GameObject m_Attatchement;

	public int m_ID;

	public List<InteractableProperty> m_Properties = new List<InteractableProperty>();

}

