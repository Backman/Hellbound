﻿using UnityEngine;
using System.Collections;
//peter
public class CreditsMenu : MonoBehaviour {
	static GameObject r_SettingsMenu;
	void Awake () {
		r_SettingsMenu = gameObject;
		gameObject.SetActive(false);
	}
	
	public void show(){
		StartMenu.setVisible(false);
		setVisible(true);
	}
	
	public static void setVisible(bool visible){
		r_SettingsMenu.SetActive(visible);
	}
}