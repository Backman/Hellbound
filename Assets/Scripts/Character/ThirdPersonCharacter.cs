using UnityEngine;
using System.Collections;

/// <summary>
/// Third person character.
/// Component that will handle user input and moves the character based
/// on that input
/// </summary>

// By Arvid Backman 2014-04-03

[System.Serializable]
public class AdvancedSettings {
	public float m_StationaryTurnSpeed = 180;		// additional turn speed added when the player is stationary (added to animation root rotation)
	public float m_MovingTurnSpeed = 360;			// additional turn speed added when the player is moving (added to animation root rotation)
	public float m_HeadLookResponseSpeed = 2;		// speed at which head look follows its target
	public float m_CrouchHeightFactor = 0.6f;		// collider height is multiplied by this when crouching
	public float m_CrouchChangeSpeed = 4;			// speed at which capsule changes height when crouching/standing
	public float m_AutoTurnThresholdAngle = 100;	// character auto turns towards camera direction if facing away by more than this angle
	public float m_AutoTurnSpeed = 2;				// speed at which character auto-turns towards cam direction
	public PhysicMaterial m_ZeroFrictionMaterial;	// used when in motion to enable smooth movement
	public PhysicMaterial m_HighFrictionMaterial;	// used when stationary to avoid sliding down slopes
	public float m_GroundStickyEffect = 5f;			// power of 'stick to ground' effect - prevents bumping down slopes.
}

public class ThirdPersonCharacter : MonoBehaviour {
	public float m_AirSpeed = 6.0f;										// Determines the max speed of the character while airborne
	public float m_AirControl = 2.0f;									// Determines the response of controlling the character while airborne
	[Range(1.0f, 4.0f)] public float m_GravityMultiplier = 2.0f;		// Gravity modifier
	[Range(0.1f, 3.0f)] public float m_MoveSpeedMultiplier = 1.0f;		// How much the move speed of the character will be multiplied by
	[Range(0.1f, 3.0f)] public float m_AnimationSpeedMultiplier = 1.0f;	// How much the animation of the character will be multiplied by
	public AdvancedSettings m_AdvancedSettings;							// Container for the advanced settings class , thiss allows the advanced settings to be in a foldout in the inspector

	public Transform lookTarget { get; set; }							// The point where the character will be looking at

	bool m_InAnimation = false;
	bool m_OnGround;
	Vector3 m_LookDirection;
	float m_OriginalHeight;
	Animator r_Animator;
	CapsuleCollider r_Collider;
	const float c_Half = 0.5f;
	Vector3 m_MoveInput;
	bool m_CrouchInput;
	bool m_ZoomInput;
	float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_Velocity;
	IComparer m_RayHitComparer;

	Vector3 m_PreviousPosition;
	Vector3 m_DeltaPosition;
	// Use this for initialization
	public Vector3 LookDirection {
		get { return m_LookDirection; }
		set { m_LookDirection = value; }
	}
	void Start () {
		Messenger.AddListener<HbClips.Animation, HbClips.animationCallback[]> ("activate animation", playAnimationClip);
		Messenger.AddListener ("drank poison", drankPoison);

		r_Animator = GetComponentInChildren<Animator>();
		r_Collider = collider as CapsuleCollider;

		if(r_Collider){
			m_OriginalHeight = r_Collider.height;
			r_Collider.center = Vector3.up * m_OriginalHeight * c_Half;
		} else {
			Debug.LogError("Collider cannot be cast to CapsuleCollider");
		}

		m_PreviousPosition = transform.position;
		m_RayHitComparer = new RayHitComparer();
		setUpAnimator();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.F4))
				r_Animator.SetTrigger ("ActivateLow");
		else if (Input.GetKeyDown (KeyCode.F2))
				r_Animator.SetTrigger ("ActivateHigh");
		else if (Input.GetKeyDown(KeyCode.F3))
			r_Animator.SetTrigger("Drink");


		m_DeltaPosition = transform.position - m_PreviousPosition;
		m_PreviousPosition = transform.position;
	}

	void LateUpdate(){
		m_InAnimation = r_Animator.GetFloat("Used") == 0.0f ? false : true ;
	}

	// This function is designed to be called from a seperate component(script)
	// Handles user input
	public void move(Vector3 move, bool crouch, Vector3 lookDirection){

		if(move.magnitude > 1.0f){
			move.Normalize();
		}
		//Debug.Log ("Move: " + move);
		m_MoveInput = move;
		m_CrouchInput = crouch;
		m_LookDirection = lookDirection;

		m_Velocity = rigidbody.velocity;

		convertMoveInput();

		//turnTowardsCameraForward();

		//preventStandingInLowHeadroom();

		//scaleCapsuleForCrouching();

		applyExtraTurnRotation();

		groundCheck();

		setFriction();

		if(m_OnGround){
			handleGroundVelocities();
		} else {
			//handleAirborneVelocities();
		}

		updateAnimator();

		rigidbody.velocity = m_Velocity;
	}

	public void zoomed(bool zoomed){
		m_ZoomInput = zoomed;
	}

	void convertMoveInput(){
		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.
		Vector3 localMove = transform.InverseTransformDirection(m_MoveInput);
		float a2 = Mathf.Atan2(localMove.x, localMove.z);
		float diff1 = Mathf.Abs(Mathf.PI - a2);
		if(diff1 < 0.001f) {
			m_TurnAmount = 0.0f;
		} else {
			m_TurnAmount = a2;
		}
		m_ForwardAmount = localMove.z;
	}

	void turnTowardsCameraForward(){
		// automatically turn to face camera direction,
		// when not moving, and beyond the specified angle threshold
		if(Mathf.Abs(m_ForwardAmount) < 0.1f){
			Vector3 lookDelta = transform.InverseTransformDirection(m_LookDirection - transform.position);
			float lookAngle = Mathf.Atan2(lookDelta.x, lookDelta.z) * Mathf.Rad2Deg;
			// Are we beyond the threshold so that we need to turn the face of the camera?
			if(Mathf.Abs(lookAngle) > m_AdvancedSettings.m_AutoTurnThresholdAngle){
				m_TurnAmount += lookAngle * m_AdvancedSettings.m_AutoTurnSpeed * Time.deltaTime;
			}
		}
	}

	void preventStandingInLowHeadroom(){
		// Prevent from standing in "crouch-only" zones
		if(!m_CrouchInput){
			Ray crouchRay = new Ray(rigidbody.position + Vector3.up * r_Collider.radius * c_Half, Vector3.up);
			float crouchRayLength = m_OriginalHeight - r_Collider.radius * c_Half;
			if(Physics.SphereCast(crouchRay, r_Collider.radius * c_Half, crouchRayLength)){
				m_CrouchInput = true;
			}
		}
	}

	void scaleCapsuleForCrouching(){
		// Scale the capsule if the user is crouching
		if(m_OnGround && m_CrouchInput && (r_Collider.height != m_OriginalHeight * m_AdvancedSettings.m_CrouchHeightFactor)){
			r_Collider.height = Mathf.MoveTowards(r_Collider.height, m_OriginalHeight * m_AdvancedSettings.m_CrouchHeightFactor, Time.deltaTime * 4.0f);
			r_Collider.center = Vector3.MoveTowards(r_Collider.center, Vector3.up * m_OriginalHeight * m_AdvancedSettings.m_CrouchHeightFactor * c_Half, Time.deltaTime * 2.0f);
		} else {
			// Everything else..
			if(r_Collider.height != m_OriginalHeight && r_Collider.center != Vector3.up * m_OriginalHeight * c_Half){
				r_Collider.height = Mathf.MoveTowards (r_Collider.height, m_OriginalHeight, Time.deltaTime * 4);
				r_Collider.center = Vector3.MoveTowards (r_Collider.center, Vector3.up * m_OriginalHeight * c_Half, Time.deltaTime * 2);
			}
		}
	}

	void applyExtraTurnRotation(){
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp (m_AdvancedSettings.m_StationaryTurnSpeed, m_AdvancedSettings.m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate (0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}

	void groundCheck(){
		Ray ray = new Ray(transform.position + Vector3.up * 0.1f, - Vector3.up);
		RaycastHit[] hits = Physics.RaycastAll(ray, 0.3f);
		System.Array.Sort(hits, m_RayHitComparer);

		if(m_Velocity.y < 5.0f){
			m_OnGround = false;
//			rigidbody.useGravity = true;
			foreach(var hit in hits){
				if(!hit.collider.isTrigger){
					if(m_Velocity.y <= 0.0f){
						//rigidbody.position = Vector3.MoveTowards(rigidbody.position, hit.point, Time.deltaTime * m_AdvancedSettings.m_GroundStickyEffect);

					}

					m_OnGround = true;
//					rigidbody.useGravity = false;
					break;
				}
			}
		}
	}

	void setFriction(){
		if(m_OnGround){
			// Set friction to low or high, depending on if we're moving
			if(m_MoveInput.magnitude == 0.0f){
				// When not moving this helps prevent sliding on slopes:
				collider.material = m_AdvancedSettings.m_HighFrictionMaterial;
			} else {
				// But when moving, we want no friction
				collider.material = m_AdvancedSettings.m_ZeroFrictionMaterial;
			}
		} else {
			// While in air, we want no friction against surfaces (walls, ceilings, etc)
			collider.material = m_AdvancedSettings.m_ZeroFrictionMaterial;
		}
	}

	void handleGroundVelocities(){
		m_Velocity.y = 0.0f;
		if(m_MoveInput.magnitude == 0.0f){
			// When not moving this prevents sliding on slopes
			m_Velocity.x = 0.0f;
			m_Velocity.z = 0.0f;
		}/* else {
			Vector3 v = m_MoveInput * m_ForwardAmount * m_MoveSpeedMultiplier;
			v.y = 0.0f;
			m_Velocity = v  * 5.0f;
		}*/
	}

	void handleAirborneVelocities(){
		// We allow some movement in air, but it's very different to when on ground
		// (typically allowing a small change in trajectory)
		Vector3 airMove = new Vector3 (m_MoveInput.x * m_AirSpeed, m_Velocity.y, m_MoveInput.z * m_AirSpeed);
		m_Velocity = Vector3.Lerp (m_Velocity, airMove, Time.deltaTime * m_AirControl);
		rigidbody.useGravity = true;
		
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		rigidbody.AddForce(extraGravityForce);
	}

	// Updates the animator based on the current states and inputs
	void updateAnimator(){
		// Only use root motion when on ground
		r_Animator.applyRootMotion = m_OnGround;

		// update the animator parameters
		r_Animator.SetFloat ("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		r_Animator.SetFloat ("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
		r_Animator.SetBool ("Crouch", m_CrouchInput);
		r_Animator.SetBool ("OnGround", m_OnGround);
		if (!m_OnGround) {
			r_Animator.SetFloat ("Jump", m_Velocity.y);
		}

		// The anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (m_OnGround && m_MoveInput.magnitude > 0) {
			r_Animator.speed = m_AnimationSpeedMultiplier;
		} else {
			// but we don't want to use that while airborne
			r_Animator.speed = 1;
		}
	}

	// Only useable with Unity Pro, because it can only handle IK rigs
/*	void OnAnimatorIK(int layerIndex)
	{
		// we set the weight so most of the look-turn is done with the head, not the body.
		r_Animator.SetLookAtWeight(1, 0.2f, 2.5f);
		
		// if a transform is assigned as a look target, it overrides the vector lookPos value
		if (lookTarget != null) {
			m_LookDirection = lookTarget.position;
		}
		
		// Used for the head look feature.
		r_Animator.SetLookAtPosition( m_LookDirection );
	}*/

	void setUpAnimator(){
		// this is a ref to the animator component on the root.
		r_Animator = GetComponent<Animator>();
		
		// we use avatar from a child animator component if present
		// (this is to enable easy swapping of the character model as a child node)
		foreach (var childAnimator in GetComponentsInChildren<Animator>()) {
			if (childAnimator != r_Animator) {
				r_Animator.avatar = childAnimator.avatar;
				Destroy (childAnimator);
				break;
			}
		}
	}

	public void OnAnimatorMove(){
		// We implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		rigidbody.rotation = r_Animator.rootRotation;
		if (m_OnGround && !m_ZoomInput && Time.deltaTime > 0) {
			Vector3 v = (r_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
			
			// we preserve the existing y part of the current velocity.
			v.y = rigidbody.velocity.y;
			rigidbody.velocity = v;
		}
	}
	#region Interact animations	
	/// <summary>
	/// Plays an animation clip depending on passed argument. 
	/// The HbClips.Animation argumet decides which clip to play.
	/// The HbClips.animationCallback will be called at the keyframe which specifies tha the keyframe should be called.
	/// </summary>
	private HbClips.animationCallback[] r_Callbacks = new HbClips.animationCallback[0];
	
	private void playAnimationClip( HbClips.Animation clip, HbClips.animationCallback[] callbacks ){

		if( !storeCallbacks (callbacks) ) {
			return;
		}

		//Play the appropriate animation
		string animation = "";
		switch (clip) {
			case HbClips.Animation.ActivateLow:
				animation = "ActivateLow";
				break;
			case HbClips.Animation.ActivateHigh:
				animation = "ActivateHigh";
				break;
			case HbClips.Animation.Drink:
				animation = "Drink";
				break;
			case HbClips.Animation.None:
			default:
				doCallbacks();
				return;
			}

		r_Animator.SetTrigger(animation);
	}

	//Store the callback for the keyCallback, which will be at the key frame in the animation
	private bool storeCallbacks( HbClips.animationCallback[] callbacks){

		r_Callbacks = new HbClips.animationCallback[ callbacks.Length ];
		bool ok = true;

		//Check if all callbacks are ok
		foreach( HbClips.animationCallback callback in callbacks ){
			if (callback == null ) {
				if( callback.Method == null ){
					ok = false;
					break;
				}
			}
		}

		if( ok ){
			r_Callbacks = callbacks;
			return true;
		} else {
			Debug.LogError("Error. Invalid callback method detected in ThirdPersonCharacter");
			return false;
		}
	}

	private void doCallback(int index){
		if (r_Callbacks.Length > index &&  r_Callbacks[index] != null && r_Callbacks[index].Method != null) {
			r_Callbacks[index]();
		} else {
			Debug.LogError("Error! Invalid callback stored in r_Callbacks at index " + index);
		}
	}
	private void doCallbacks(){
		for( int i = 0; i < r_Callbacks.Length; i++ ){
			doCallback(i);
		}
	}

	private void drankPoison(){
		r_Animator.SetBool ("DrankPoison", true);
	}

	/// <summary>
	/// Returns if the avatar is able to move/run
	/// </summary>
	public bool isMovable(){
		//TODO: Toggle OnAnimatorIK	
		return !m_InAnimation ? true : false; 
	}
	#endregion
	class RayHitComparer : IComparer {
		public int Compare(object x, object y){
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
		}
	}
}
