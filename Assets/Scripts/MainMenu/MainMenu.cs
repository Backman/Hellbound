using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnClickButton(){
		Debug.Log ("Clicked on button!");
		Application.LoadLevel("Antons_Scene");
	}

	void OnClickOptionsButton(){
		Debug.Log ("Clicked on Options button");
	}

	void OnClickExitButton(){
		Debug.Log ("Clicked on Exit button");
	}
}
