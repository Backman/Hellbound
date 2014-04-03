using UnityEngine;
using System.Collections;

public abstract class TargetFollower : MonoBehaviour {

	public enum EUpdateType {
		Auto,				// Let the script decide how to update
		FixedUpdate,		// Update in FixedUpdate (for tracking rigidbodies)
		LateUpdate			// Update in LateUpdate (for tracking objects thar are moved in Update)
	}

	[SerializeField] protected Transform m_Target;
	[SerializeField] private bool m_AutoTargetPlayer = true;
	[SerializeField] private EUpdateType m_UpdateType;

	// Use this for initialization
	virtual protected void Start () {
		// If auto targeting is used, find the object tagget "Player"
		// Any class inheriting from this should call base.Start() to perform this action!
		if(m_AutoTargetPlayer) {
			findAndTargetPlayer();
		}
	}

	void FixedUpdate() {
		// We update from here if updatetype is set to FixedUpdate, or in auto mode,
		// if the target has a rigidbody, and isn't kinematic
		if(m_AutoTargetPlayer && (!m_Target || !m_Target.gameObject.activeSelf)) {
			findAndTargetPlayer();
		}
		if(m_UpdateType == EUpdateType.FixedUpdate || m_UpdateType == EUpdateType.Auto && (m_Target.rigidbody && !m_Target.rigidbody.isKinematic)) {
			followTarget(Time.deltaTime);
		}
	}

	void LateUpdate() {
		// We update from here if update type is set to LateUpdate, or in auto mode,
		// if the target does not have a rigidbody, or - does have a rigidbody but is set to kinematic
		if(m_AutoTargetPlayer && (!m_Target || !m_Target.gameObject.activeSelf)) {
			findAndTargetPlayer();
		}
		if(m_UpdateType == EUpdateType.LateUpdate || m_UpdateType == EUpdateType.Auto && !m_Target && (!m_Target.rigidbody || m_Target.rigidbody.isKinematic)) {
			followTarget(Time.deltaTime);
		}
	}

	protected abstract void followTarget(float deltaTime);

	public void findAndTargetPlayer() {
		// Only target if we don't already have a target
		if(!m_Target) {
			// Auto target an object tagget "Player", if no target has been assigned
			var targetObj = GameObject.FindGameObjectWithTag("Player");
			if(targetObj) {
				setTarget(targetObj.transform);
			}
		}
	}

	public virtual void setTarget(Transform newTransform) {
		m_Target = newTransform;
	}

	public Transform Target { get { return m_Target; } }
}
