using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UICamera))]
public class Peter_FindSelectedObj : MonoBehaviour {
	public GameObject m_Selected = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject hovered = UICamera.hoveredObject;
		if (hovered != null) {
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = hovered;
		}
	}
}
