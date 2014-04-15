using UnityEngine;
using System.Collections;

public class Combine : MonoBehaviour {
	private static GameObject m_Object;
	// Use this for initialization
	void Start () {
		Debug.Log ("Initialize combine window");
		m_Object = gameObject;
		gameObject.SetActive(false);
	}
	
	public void combine(){
		Inventory.getInstance().combineItems();
	}
	
	public static void show(){
		m_Object.SetActive(true);
	}
	
	public static void hide(){
		m_Object.SetActive(false);
	}
}
