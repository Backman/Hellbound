using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Checks if the camera is colliding with something and handles it.
/// </summary>

public class CameraCollision : MonoBehaviour {

	public float m_ClipMoveTime = 0.05f;
	public float m_ReturnTime = 0.4f;
	public float m_SphereCastRadius = 0.1f;
	public bool m_VisualiseInEditor;
	public float m_ClosestDistance = 0.5f;
	public bool Protecting { get; private set; }
	public string m_DontClipTag = "Player";

	private Transform m_Camera;
	private Transform m_Pivot;
	private float m_OriginalDistance;
	private float m_MoveVelocity;
	private float m_CurrentDistance;
	private Ray m_Ray;
	private RaycastHit[] m_Hits;
	private RayHitComparer m_RayHitComparer;

	// Use this for initialization
	void Start () {
		// Find the camera in the object hierarchy
		m_Camera = GetComponentInChildren<Camera>().transform;

		// Find the pivot in the object hierarchy, should ALWAYS be the parent to the camera
		m_Pivot = m_Camera.parent;

		m_OriginalDistance = m_Camera.localPosition.magnitude;
		m_CurrentDistance = m_OriginalDistance;

		m_RayHitComparer = new RayHitComparer();
	}

	void LateUpdate() {
		if(m_Camera.localPosition.magnitude != m_OriginalDistance) {
			m_OriginalDistance = m_Camera.localPosition.magnitude;
		}
		// Set target distance
		float targetDistance = m_OriginalDistance;

		m_Ray.origin = m_Pivot.position + m_Pivot.forward * m_SphereCastRadius;
		m_Ray.direction = -m_Pivot.forward;

		// Initial check to see if start of sphere cast intersects anything
		Collider[] cols = Physics.OverlapSphere(m_Ray.origin, m_SphereCastRadius);

		bool initialIntersect = false;
		bool hitSomething = false;

		// Loop through all the collisions to check if something we care about
		foreach(Collider col in cols) {
			if(!col.isTrigger && !(col.attachedRigidbody != null && col.attachedRigidbody.CompareTag(m_DontClipTag))) {
				initialIntersect = true;
				break;
			}
		}

		// If there is a collision
		if(initialIntersect) {
			m_Ray.origin += m_Pivot.forward * m_SphereCastRadius;
		
			// Do a raycast and gather all the intersections
			m_Hits = Physics.RaycastAll(m_Ray, m_OriginalDistance - m_SphereCastRadius);
		} else {
			// If there was no collision do a sphere cast to see if there were any other collisions
			m_Hits = Physics.SphereCastAll(m_Ray, m_SphereCastRadius, m_OriginalDistance + m_SphereCastRadius);
		}

		// Sort the collisions by distance
		Array.Sort(m_Hits, m_RayHitComparer);

		float nearest = Mathf.Infinity;

		// Loop through all the collisions
		foreach(RaycastHit hit in m_Hits) {
			// Only deal with the collision if it was closer than the previous one,
			// not a trigger, and not attatched to a rigidbody tagget with the dontClipTag
			if(hit.distance < nearest && (!hit.collider.isTrigger) && !(hit.collider.attachedRigidbody != null && hit.collider.attachedRigidbody.CompareTag(m_DontClipTag))) {
				// Change the nearest collision to the latest
				nearest = hit.distance;
				targetDistance = -m_Pivot.InverseTransformPoint(hit.point).z;
				hitSomething = true;
			}
		}

		// Visualise the camera clip effect in the editor
		if(hitSomething) {
			Debug.DrawRay(m_Ray.origin, -m_Pivot.forward * (targetDistance + m_SphereCastRadius), Color.red);
			m_Camera.localPosition = -Vector3.forward * m_CurrentDistance;
		}

		// Hit something so move the camera to a better position
		Protecting = hitSomething;
		m_CurrentDistance = Mathf.SmoothDamp(m_CurrentDistance, targetDistance, ref m_MoveVelocity, m_CurrentDistance > targetDistance ? m_ClipMoveTime : m_ReturnTime);
		m_CurrentDistance = Mathf.Clamp(m_CurrentDistance, m_ClosestDistance, m_OriginalDistance);
	}

	public class RayHitComparer : IComparer {
		public int Compare(object x, object y) {
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}
}

