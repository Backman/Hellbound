using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	void OnGameStartButtonClicked(){
		Debug.Log("GameStartButton Clicked!");	
		Application.LoadLevel("Scene_01");
	}
	
	void OnOptionsButtonClicked(){
		Debug.Log("OnOptionsButton Clicked");
	}
	
	void OnExitButtonClicked(){
		Debug.Log ("OnExitButton Clicked");	
	}
}
