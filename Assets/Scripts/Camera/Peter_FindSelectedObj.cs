using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UICamera))]
public class Peter_FindSelectedObj : MonoBehaviour {
	public GameObject m_Selected = null;
	public GameObject mouseSelected = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_Selected = UICamera.selectedObject;

		bool condition = true;
		GameObject hovered = UICamera.hoveredObject;
		if (hovered != null) {
			if (hovered.GetComponent<UIKeyNavigation>() != null) {
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.selectedObject = hovered;
				condition = false;
			} 
		}
		else if (mouseSelected != hovered && condition){
			UICamera.selectedObject = null;
		}
		mouseSelected = hovered;
	}
}
