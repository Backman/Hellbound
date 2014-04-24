using UnityEngine;
using System.Collections;

/// <summary>
/// Class to handle visibility of combine buttons
/// </summary>
public class Combine : MonoBehaviour {
	private static GameObject r_Object;
	// Use this for initialization
	void Start () {
		r_Object = gameObject;
		//gameObject.SetActive(false);
	}
	
	public void combine(){

	}
	
	public static void show(){
		r_Object.GetComponent<UIPlayTween>().Play(true);
		//m_Object.SetActive(true);
	}
	
	public static void hide(){
		r_Object.GetComponent<UIPlayTween>().Play(false);
		//m_Object.SetActive(false);
	}

	public void hideWindow() {
		gameObject.GetComponent<UIPlayTween>().Play(false);
	}
}
