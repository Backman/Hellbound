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

		GameObject hovered = UICamera.hoveredObject;
		if (hovered != null) {
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = hovered;
		} else if (mouseSelected != null) {
			UICamera.selectedObject = null;
		}
		mouseSelected = hovered;
	}
}
