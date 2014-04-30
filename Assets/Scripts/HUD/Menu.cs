using UnityEngine;
using System.Collections;
//peter
//generel menu script för alla menyer som gör det flexiblare att skapa nya menyer
public class Menu : MonoBehaviour {
	private GameObject r_SettingsMenu;
	public bool Active = false;
	// Use this for initialization
	void Awake () {
		r_SettingsMenu = gameObject;
		gameObject.SetActive(Active);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show(){
		r_SettingsMenu.SetActive (true);
	}

	public void dontShow(){
		r_SettingsMenu.SetActive (false);
	}
}
