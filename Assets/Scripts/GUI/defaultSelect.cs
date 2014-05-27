using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIKeyNavigation))]
public class defaultSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.getButtonDown(InputManager.Button.Backward) || 
		   InputManager.getButtonDown(InputManager.Button.Forward) ||
		   InputManager.getButtonDown(InputManager.Button.Left) ||
		   InputManager.getButtonDown(InputManager.Button.Right) ||
		   InputManager.getButtonDown(InputManager.Button.Use)){
			Debug.Log("Pressed button!");
			bool condition = false;
			if(UICamera.selectedObject != null){
				if(!(UICamera.selectedObject.GetComponent<UIKeyNavigation>() != null)){
					condition = true;
				}
			}
			else{
				condition = true;
			}
			if (condition){
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.selectedObject = gameObject;
			}
		}
	}
}
