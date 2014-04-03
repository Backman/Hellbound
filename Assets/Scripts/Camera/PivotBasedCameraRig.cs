using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

/// <summary>
/// This script is designed to be placed on the root object of a camera rig,
/// 
/// </summary>

// Camera Rig
//		Pivot
//			Camera

public class PivotBasedCameraRig : TargetFollower {

	protected Transform m_Camera;			// The transform of the camera
	protected Transform m_Pivot;			// The point which the camera pivots around
	protected Vector3 m_LastTargetPosition;

	[SerializeField] protected bool m_FollowTargetInEditMode = true;
	public string Warning { get; private set; }

	protected virtual void Awake() {
		// Find the camera in the object hierarchy
		m_Camera = GetComponentInChildren<Camera>().transform;

		// Find the pivot in the object hierarchy, should ALWAYS be the parent to the camera
		m_Pivot = m_Camera.parent;
	}

	protected override void Start() {
		base.Start();
	}

	virtual protected void Update() {
		#if UNITY_EDITOR
		if(!Application.isPlaying && m_FollowTargetInEditMode) {
			if(m_Target) {
				float delta = (m_Target.position - transform.position).magnitude;
				if(delta > 0.1f && m_LastTargetPosition == m_Target.position) {
					Warning = "The Rig's position is automatically locked to the target's position. You can use the child objects (the Pivot and the Camera) to adjust the view.";
					transform.position = m_Target.position;
				} else {
					Warning = "";
				}

				followTarget(999);
				m_LastTargetPosition = Target.position;
			}

			if(Mathf.Abs(m_Camera.localPosition.x) > 0.5f || Mathf.Abs (m_Camera.localPosition.y) > 0.5f) {
				EditorUtility.DisplayDialog("Camera Rig Warning", "You should only adjust this Camera's Z position. The X and Y values must remain zero. Instead, move the Camera's parent (the \"Pivot\") to adjust the camera view", "OK");
				m_Camera.localPosition = Vector3.Scale(m_Camera.localPosition, Vector3.forward);
				EditorUtility.SetDirty(m_Camera);
			}

			m_Camera.localPosition = Vector3.Scale(m_Camera.localPosition, Vector3.forward);

			return;
		} else {
			Warning = "";
		}
		#endif
	}

	protected override void followTarget (float deltaTime)
	{
		// Must be overridden
	}

	void OnDrawGizmos() {
		if(m_Pivot && m_Camera) {
			Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
			Gizmos.DrawLine(transform.position, m_Pivot.position);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(m_Pivot.position, m_Camera.position);
		}
	}
}

