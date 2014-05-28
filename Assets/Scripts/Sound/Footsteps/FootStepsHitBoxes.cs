using UnityEngine;
using System.Collections;

public class FootStepsHitBoxes : MonoBehaviour {

	/// <summary>
	/// This script is to be placed on the colliders on the back of the foot(feet).
	/// Actually there's barely anything here (but it is vital)
	/// Anton Thorsell
	/// </summary>

	//bool to figure out if we are hitting anything 
	[HideInInspector]
	public bool b_IsHitting = false;


	//if we hit anything that have footstepsurface as a component, the bool becomes true
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<FootstepSurface>() != null)
		{
			b_IsHitting = true;
		}
	}

	//if we leave the thing we hit that have footstepsurface as a component, 
	///the bool becomes false (durr)
	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<FootstepSurface>() != null)
		{
			b_IsHitting = false;
		}
	}
}
