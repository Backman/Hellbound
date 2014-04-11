﻿using UnityEngine;
using System.Collections;

public class FootStepsBack : MonoBehaviour {

	/// <summary>
	/// This script is to be placed on the colliders on the back of the foot(feet).
	/// Actually there's barely anything here (but it is VITAL)
	/// Anton Thorsell
	/// </summary>

	//bool to figure out if we are hitting anything 
	public bool b_IsHitting = false;


	//if we hit anything, the bool becomes true
	void OnTriggerEnter(Collider other)
	{
		b_IsHitting = true;
	}

	//if we leave the thing we hit, the bool becomes false (durr)
	void OnTriggerExit(Collider other)
	{
		b_IsHitting = false;
	}
}
