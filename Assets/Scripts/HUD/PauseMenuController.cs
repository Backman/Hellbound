using UnityEngine;
using System.Collections;

/// <summary>
/// Class that is attached to Inventory and Settings panel to show/hide
/// and switch between them
/// </summary>
public class PauseMenuController : MonoBehaviour {
	void Awake(){
		PauseMenu.getInstance().add (this);
	}
	
	public void show(){
		PauseMenu.getInstance().hideAll();
		gameObject.SetActive(true);
	}
	
	public void hide(){
		gameObject.SetActive(false);
	}
}
