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
		if(gameObject.activeSelf != true){
			List<SettingsMenuControl> newList = new List<SettingsMenuControl> ();

			foreach(SettingsMenuControl menuControl in m_Settings){
				if( menuControl != null ){
					menuControl.setVisible(false);
					newList.Add( menuControl );
				}
			}
			setVisible(true);
			m_Settings = newList;
		}
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
