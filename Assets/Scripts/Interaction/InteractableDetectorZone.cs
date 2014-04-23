using UnityEngine;
using System.Collections;

/// <summary>
/// Interactable detector zone.
/// This class handles the logic for the zone infront of the player that scans for interactable objects
/// 
/// Created by Simon
/// </summary>
public class InteractableDetectorZone : Singleton<InteractableDetectorZone> {

	private Interactable r_InFocus = null;
	private GUIManager r_GUIManager;

	void Start () {
		//TODO: Find the avatar and position this in the appropriate spot
		Messenger.AddListener("clear focus", clearFocus);
		r_GUIManager = GUIManager.Instance;
	}

	void Update () {
	
		if( r_InFocus != null ) {

			if( Input.GetButtonDown( "Use" ) ){
				r_InFocus.activate();
			}
			
			if( Input.GetButtonDown( "Examine" ) ){
				r_InFocus.examine();
			}
			
			if( Input.GetButtonDown( "Pickup" ) ){
				r_InFocus.pickUp();
			}
		}
	}

	/// <summary>
	/// This function is called on the Interactable as the interactable enters the players interact zone
	/// </summary>
	void OnTriggerEnter( Collider col ){
		//TODO: Add detection for which interactable is in focus.

		Interactable ii = col.gameObject.GetComponent<Interactable>();
		if( ii != null ){
			r_InFocus = ii;
			r_InFocus.gainFocus();

			setupInteractText();

			r_GUIManager.m_InteractText.active(true);
		}
	}

	/// <summary>
	///This function is called on the Interactable as it leaves the players interact zone
	/// </summary>
	void OnTriggerExit( Collider col  ){
		//TODO: Detect wheter the left  trigger was the interactalbe actually in focus 

		if( r_InFocus != null && col.gameObject == r_InFocus.gameObject ){
			r_InFocus.loseFocus();
			r_InFocus = null;
			r_GUIManager.m_InteractText.active(false);
		}
	}

	/// <summary>
	/// Returns the interactable in focus.
	/// This must be controlled, might be null.
	/// </summary>
	/// <returns>The interactable in focus.</returns>
	public Interactable getInteractableInFocus(){
		return r_InFocus;
	}

	private void setupInteractText() {
		r_GUIManager.m_InteractText.reposition();

		if(r_InFocus.m_Thumbnail == null) {
			r_GUIManager.m_InteractText.HasPickup = false;
		} else {
			r_GUIManager.m_InteractText.HasPickup = true;
		}
//		if(!r_InFocus.Usable) {
//			r_GUIManager.m_InteractText.CanBeUsed = false;
//		} else {
//			r_GUIManager.m_InteractText.CanBeUsed = true;
//		}
		r_GUIManager.m_InteractText.CanBeUsed = true;
		if(r_InFocus.m_Description.Trim () == "") {
			r_GUIManager.m_InteractText.HasExamine = false;
		} else {
			r_GUIManager.m_InteractText.HasExamine = true;
		}
		
		r_GUIManager.m_InteractText.reposition();
	}

	public void clearFocus(){
		Messenger.Broadcast ("leaveFocus");
		r_InFocus = null;		
		r_GUIManager.m_InteractText.active(false);
	//	r_GUIManager.m_InteractText.gameObject.SetActive(false);
	}
}