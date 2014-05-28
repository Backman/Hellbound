using UnityEngine;
using System.Collections;
//peter
//generel menu script för alla menyer som gör det flexiblare att skapa nya menyer
public class Menu : MonoBehaviour {
	public bool Active = false;
	// Use this for initialization
	void Awake () {
		gameObject.SetActive(Active);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show(){
		gameObject.SetActive (true);
	}

	public void dontShow(){
		gameObject.SetActive (false);
	}
}
