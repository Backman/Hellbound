using UnityEngine;
using System.Collections;

public class HUDControl : UISprite {

	private UISprite m_Sprite;
	private bool m_Visible;
	[SerializeField] bool m_HideOnStart = true;
	// Use this for initialization
	void Start () {
		m_Sprite = gameObject.GetComponent<UISprite>();
		if(m_HideOnStart){
			hide();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void show(){
		string spriteName = "";
		switch(MenuController.getInstance().getState()){
			case MenuController.State.START:
				spriteName = "StartMenu";
				break;
			case MenuController.State.PAUSE:
				spriteName = "PauseMenu";
				break;
		}
		Debug.Log ("Hide previous: "+spriteName);
		UISprite sprite = GameObject.FindGameObjectWithTag(spriteName).GetComponent<UISprite>();
		sprite.alpha = 0.0f;
		
		m_Sprite.alpha = 1.0f;
		m_Visible = true;
	}
	
	public void hide(){
		m_Sprite.alpha = 0.0f;
		m_Visible = false;
	}
}
