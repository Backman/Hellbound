using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

	private UISprite m_Sprite;
	
	void Awake(){
		PauseMenu.getInstance().add (this);
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
