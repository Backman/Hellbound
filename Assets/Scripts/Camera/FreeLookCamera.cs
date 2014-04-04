using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

/// <summary>
/// Script gives the camera a "Free look"
/// This script is designed to be placed on the root object of a camera rig
/// </summary>

// Camera Rig
//		Pivot
//			Camera

[ExecuteInEditMode]
public class FreeLookCamera : PivotBasedCameraRig {

	[SerializeField] private float m_FollowSpeed = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] private float m_TurnSpeed = 1.5f;
	[SerializeField] private float m_TurnSmoothing = 0.1f;
	[SerializeField] private float m_TiltMax = 75.0f;
	[SerializeField] private float m_TiltMin = 45.0f;
	[SerializeField] private float m_ZoomSpeed = 0.3f;
	[SerializeField] private bool m_LockCursor = false;

	private Vector3 m_CameraOriginPosition;
	private Vector3 m_ZoomPosition;

	private float m_LookAngle;
	private float m_TiltAngle;

	private float m_OriginalFollowSpeed;

	private const float c_LookDistance = 100.0f;
	private float m_SmoothX = 0.0f;
	private float m_SmoothY = 0.0f;
	private float m_SmoothXVelocity = 0.0f;
	private float m_SmoothYVelocity = 0.0f;
	private bool m_Zoomed = false;

	protected override void Awake () {
		base.Awake ();

		// Lock or unlock the cursor
		Screen.lockCursor = m_LockCursor;

		// Find the camera in the object hierarchy
		m_Camera = GetComponentInChildren<Camera>().transform;
		m_CameraOriginPosition = m_Camera.localPosition;
		m_ZoomPosition = m_Camera.localPosition;
		// Find the pivot in the object hierarchy, should ALWAYS be the parent to the camera
		m_Pivot = m_Camera.parent;

		m_OriginalFollowSpeed = m_FollowSpeed;
	}

	protected override void Update() {
		base.Update ();

		handleZoomInput();

		handleRotationMovement();
		if(m_LockCursor && Input.GetMouseButtonUp(0)) {
			Screen.lockCursor = m_LockCursor;
		}
	}

	void OnDisable() {
		Screen.lockCursor = false;
	}

	protected override void followTarget (float deltaTime) {
		transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * m_FollowSpeed);
	}

	void handleRotationMovement() {
		// Read the user input

		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");

		// Smooth the user input
		if(m_TurnSmoothing > 0.0f) {
			m_SmoothX = Mathf.SmoothDamp(m_SmoothX, x, ref m_SmoothXVelocity, m_TurnSmoothing);
			m_SmoothY = Mathf.SmoothDamp(m_SmoothY, y, ref m_SmoothYVelocity, m_TurnSmoothing);
		} else {
			m_SmoothX = x;
			m_SmoothY = y;
		}

		// Adjust the look angle by an amount proportional to the turn speed and horizontal input
		m_LookAngle += m_SmoothX * m_TurnSpeed;

		// Rotate the rig (the root object) around Y axis only
		transform.rotation = Quaternion.Euler(0.0f, m_LookAngle, 0.0f);

		// We adjust the current angle based on Y mouse input and turn speed
		m_TiltAngle -= m_SmoothY * m_TurnSpeed;
		// We make sure the new valuse is within the tilt range
		m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);


		// Tilt input around X is applied to the pivot (the child of this object)
		m_Pivot.localRotation = Quaternion.Euler(m_TiltAngle, 0.0f, 0.0f);
	}

	void handleZoomInput() {
		bool zoom = Input.GetKeyDown(KeyCode.Tab);

		if(zoom && !m_Zoomed) {
			m_ZoomPosition = m_Pivot.localPosition;
			m_Zoomed = true;
		} else if(zoom && m_Zoomed) {
			m_ZoomPosition = m_CameraOriginPosition;
			m_Zoomed = false;
		}
		m_ZoomPosition.y = 0.0f;
		Vector3 newPos = Vector3.MoveTowards(m_Camera.localPosition, m_ZoomPosition, m_ZoomSpeed * Time.deltaTime);
		m_Camera.localPosition = newPos;

		if((m_Camera.localPosition - m_ZoomPosition).magnitude <= 0.4f && m_Zoomed){
			DisableMeshRendererOnTarget();
		}
		if((m_Camera.localPosition - m_ZoomPosition).magnitude >= 0.4f  && !m_Zoomed) {
			EnableMeshRendererOnTarget();
		}
	}

	void DisableMeshRendererOnTarget(){
		SkinnedMeshRenderer[] renderers = m_Target.GetComponentsInChildren<SkinnedMeshRenderer>();

		foreach(SkinnedMeshRenderer m in renderers){
			m.enabled = false;
		}
		m_FollowSpeed = 0.0f;
	}

	void EnableMeshRendererOnTarget(){
		SkinnedMeshRenderer[] renderers = m_Target.GetComponentsInChildren<SkinnedMeshRenderer>();
		
		foreach(SkinnedMeshRenderer m in renderers){
			m.enabled = true;
		}
		m_FollowSpeed = m_OriginalFollowSpeed;
	}
}

