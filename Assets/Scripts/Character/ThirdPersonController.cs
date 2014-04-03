using UnityEngine;
using System.Collections;

/// <summary>
/// Third person controller.
/// Component that will take user input and send it to the Third Person Character component
/// </summary>

// By Arvid Backman 2014-04-03

[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonController : MonoBehaviour {

	public bool m_WalkByDefault = false;				// toggle for walking state
	public bool m_LookInCameraDirection = true;			// should the character be looking in the same direction that the camera is facing
	
	private Vector3 m_LookDirection;					// The position that the character should be looking towards
	private ThirdPersonCharacter r_Character;			// A reference to the ThirdPersonCharacter on the object
	private Transform r_Camera;							// A reference to the main camera in the scenes transform
	private Vector3 m_CameraForward;					// The current forward direction of the camera
	private Vector3 m_Move;								// the world-relative desired move direction, calculated from the camForward and user input.

	// Use this for initialization
	void Start () {
		// get the transform of the main camera
		if (Camera.main)
		{
			r_Camera = Camera.main.transform;
		} else {
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		r_Character = GetComponent<ThirdPersonCharacter>();	
	}
	
	// FixedUpdate is called in sync with physics
	void FixedUpdate () {
		// Read inputs
		bool crouch = Input.GetKey(KeyCode.C);

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		// Calculate move direction to pass to third person character
		if(r_Camera){
			m_CameraForward = Vector3.Scale(r_Camera.forward, new Vector3(1.0f, 0.0f, 1.0f)).normalized;
			m_Move = v * m_CameraForward + h * r_Camera.right;
		} else {
			// we use world-relative directions in the case of no main camera
			m_Move = v * Vector3.forward + h * Vector3.right;
		}

		if(m_Move.magnitude > 1.0f){
			m_Move.Normalize();
		}

		// Walk/Run speed is modified by a key press.
		bool walkToggle = Input.GetKey(KeyCode.LeftShift);
		// We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
		float walkMultiplier = (m_WalkByDefault ? walkToggle ? 0.8f : 0.5f : walkToggle ? 0.5f : 0.8f);
		m_Move *= walkMultiplier;

		m_LookDirection = m_LookInCameraDirection && r_Camera ? transform.position + r_Camera.forward * 100.0f : transform.position + transform.forward * 100.0f;

		r_Character.move (m_Move, crouch, m_LookDirection);
	}
}

