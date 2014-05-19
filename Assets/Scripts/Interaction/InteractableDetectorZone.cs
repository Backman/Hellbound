using UnityEngine;
using System.Collections;

/// <summary>
/// Interactable detector zone.
/// This class handles the logic for the zone infront of the player that scans for interactable objects
/// 
/// Created by Simon
/// </summary>
public class InteractableDetectorZone : MonoBehaviour {

	private Interactable r_CachedFocus = null;
	private Interactable r_InFocus = null;
	private GUIManager   r_GUIManager;

	void Start () {
		Messenger.AddListener("clear focus", clearFocus);
		Messenger.AddListener<Interactable>("add focus", addFocus);
		Messenger.AddListener("update focus", updateFocus);
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
			
/*			Depricated behaviour. Everything is done through the "Use" function nowadays
 			if( Input.GetButtonDown( "Pickup" ) ){
				r_InFocus.pickUp();
			}	*/
		}
	}

	/// <summary>
	/// This function is called on the Interactable as the interactable enters the players interact zone
	/// </summary>
	void OnTriggerEnter( Collider col ){
		//TODO: Add detection for which interactable is in focus if several objects are within the zone.
		Interactable ii = col.gameObject.GetComponent<Interactable>();
		if( ii != null && ii.enabled ){
			r_InFocus = ii;
			r_CachedFocus = ii;
			r_InFocus.gainFocus();
			setupInteractText();

			r_GUIManager.interactTextActive( true );
		}
	}

	/// <summary>
	///This function is called on the Interactable as it leaves the players interact zone
	/// </summary>
	void OnTriggerExit( Collider col  ){

		if( r_InFocus != null && col.gameObject == r_InFocus.gameObject ){
			r_InFocus.loseFocus();
			r_InFocus = null;
			r_CachedFocus = null;

			r_GUIManager.interactTextActive( false );
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

		string useText 		= r_InFocus.m_UseText.Trim();
		string examineText 	= r_InFocus.m_Description.Trim();

		r_GUIManager.setupInteractionTexts( examineText, useText );

	}

	public void clearFocus(){
		if (r_InFocus != null) {	
			r_InFocus.loseFocus ();
			r_InFocus = null;		
			r_GUIManager.interactTextActive (false);	
		}
	}
	
	public void addFocus(Interactable inter) {
		r_InFocus = inter;
		setupInteractText();
		r_GUIManager.interactTextActive( true );
	}

	public void updateFocus(){
		r_InFocus = r_CachedFocus;
		if( r_InFocus != null ){
			setupInteractText();
			r_GUIManager.interactTextActive( true );
		}
	}
}























