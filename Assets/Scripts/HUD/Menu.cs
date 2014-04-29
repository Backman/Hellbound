using UnityEngine;
using System.Collections;
//peter
//generel menu script för alla menyer som gör det flexiblare att skapa nya menyer
public class Menu : MonoBehaviour {
	private GameObject r_SettingsMenu;
	// Use this for initialization
	void Awake () {
		r_SettingsMenu = gameObject;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show(){
		StartMenu.setVisible(false);
		r_SettingsMenu.SetActive (true);
	}
}
