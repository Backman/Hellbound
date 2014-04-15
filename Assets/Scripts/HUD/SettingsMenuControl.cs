using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenuControl : UISprite {
	
	private static List<SettingsMenuControl> m_Settings = new List<SettingsMenuControl>();
	// Use this for initialization
	void Start() {
		m_Settings.Add(this);
		gameObject.SetActive(true);
		gameObject.GetComponent<UISprite>().alpha = 1.0f;
		if(gameObject.name != "GraphicsMenu"){
			//hide();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Called by Unity
	public void show(){
		foreach(SettingsMenuControl menuControl in m_Settings){
			Debug.Log ("Hide setting menu: "+menuControl.name);
			menuControl.setVisible(false);
		}
		setVisible(true);
	}
	
	// Called by Unity
	public void hide(){
		gameObject.SetActive(false);
	}
	
	public void setVisible(bool visible){
		if(visible){
			gameObject.SetActive(true);
		}
		else{
			gameObject.SetActive(false);
		}
	}
}
