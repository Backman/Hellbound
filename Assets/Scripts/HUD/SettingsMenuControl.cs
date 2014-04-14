using UnityEngine;
using System.Collections;

public class SettingsMenuControl : UISprite {
	
	private UISprite m_Sprite;
	private bool m_Visible;
	// Use this for initialization
	void Start() {
		m_Sprite = gameObject.GetComponent<UISprite>();
		hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Called by Unity
	public void show(){
		foreach(var obj in GameObject.FindGameObjectsWithTag("SettingsMenu")){
			SettingsMenuControl control = obj.GetComponent<SettingsMenuControl>();
			control.setVisible(false);
		}
		m_Sprite.alpha = 1.0f;
		m_Visible = true;
	}
	
	// Called by Unity
	public void hide(){
		m_Sprite.alpha = 0.0f;
		m_Visible = false;
	}
	
	public void setVisible(bool visible){
		if(visible){
			m_Sprite.alpha = 1.0f;
		}
		else{
			m_Sprite.alpha = 0.0f;
		}
		m_Visible = visible;
	}
	
	public bool isVisible(){
		return m_Visible;
	}
}
