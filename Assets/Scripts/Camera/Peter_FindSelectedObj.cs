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
		m_Selected = UICamera.selectedObject;
	}
}
