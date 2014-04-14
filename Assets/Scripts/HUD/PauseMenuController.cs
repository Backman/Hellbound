using UnityEngine;
using System.Collections;

public class PauseMenuController : UISprite {

	private UISprite m_Sprite;
	void Start(){
		PauseMenu.getInstance().add(this);
		m_Sprite = gameObject.GetComponent<UISprite>();
		// Because NGUI is buggy shit
		m_Sprite.alpha = 1.0f;
		//gameObject.SetActive(true);
		Debug.Log("Start: "+gameObject.name);
		if(gameObject.name != "Inventory"){
			gameObject.SetActive(false);
			Debug.Log("Hide: "+gameObject.name);
		}
	}
	
	void Update(){
		
	}
	
	public void show(){
		PauseMenu.getInstance().hideAll();
		Debug.Log("Show: "+gameObject.name);
		gameObject.SetActive(true);
	}
	
	public void hide(){
		gameObject.SetActive(false);
	}
}
