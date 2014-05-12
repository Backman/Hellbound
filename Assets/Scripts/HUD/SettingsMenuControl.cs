using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for switching between settings panels (graphics, sounds, etc?...).
/// </summary>
public class SettingsMenuControl : MonoBehaviour {//UISprite {
	/// <summary>
	/// Lazy solution to not having to create another class to hold list
	/// </summary>
	private static List<SettingsMenuControl> m_Settings = new List<SettingsMenuControl>();
	public bool Active = false;
	// Use this for initialization
	void Start() {
		m_Settings.Add(this);
		//gameObject.SetActive(true);
		gameObject.SetActive(Active);
		//gameObject.GetComponent<UISprite>().alpha = 1.0f;
	}
	
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
