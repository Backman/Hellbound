using UnityEngine;
using System.Collections;

public class InteractableDetectorZone : MonoBehaviour {

	private Interactable r_InFocus = null;

	// Use this for initialization
	void Start () {
		//TODO: Find the avatar and position this in the appropriate spot
	}
	
	// Update is called once per frame
	void Update () {
	
		if( r_InFocus != null ) {

			if( Input.GetButtonDown( "Fire1" ) && r_InFocus.m_ActivateType == Interactable.ActivateType.OnClick ){
				// r_InFocus.do("activate");
				r_InFocus.activate();
			}
			
			if( Input.GetButtonDown( "Fire2" ) ){
				r_InFocus.examine();
			}
			
			if( Input.GetKeyDown( KeyCode.E ) ){
				r_InFocus.pickUp();
			}
		}

	}

	//If an interactable is in front of the players, this function is called as the interactable enters the players interact zone
	void OnTriggerEnter( Collider col ){
		//TODO: Add detection for which interactable is in focus.

		Interactable ii = col.gameObject.GetComponent<Interactable>();
		if( ii != null ){
			r_InFocus = ii;
			r_InFocus.gainFocus();
		}
	}
	
	//This function is called as an interactable leaves the players interact zone
	void OnTriggerExit( Collider col  ){
		//TODO: Detect wheter the left  trigger was the interactalbe actually in focus 

		if( col.gameObject == r_InFocus.gameObject ){
				r_InFocus.loseFocus();
				r_InFocus = null;
		}
	}

}