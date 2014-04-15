using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	static GameObject r_StartMenu;
	void Awake () {
		r_StartMenu = gameObject;
	}
	
	public void show(){
		SettingsMenu.setVisible(false);
		setVisible(true);
	}
	
	public static void setVisible(bool visible){
		r_StartMenu.SetActive(visible);
	}
}
