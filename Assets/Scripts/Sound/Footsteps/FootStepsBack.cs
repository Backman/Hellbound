using UnityEngine;
using System.Collections;

public class FootStepsBack : MonoBehaviour {

	/// <summary>
	/// This script is to be placed on the colliders on the back of the foot(feet).
	/// Actually there's barely anything here (but it is VITAL)
	/// Anton Thorsell
	/// </summary>

	//bool to figure out if we are hitting anything 
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
			/*Texture tex1 = other.renderer.material.GetTexture("_MainTex");
			Debug.Log ("terrain: " + tex1);
			Texture tex2 = other.renderer.material.GetTexture("_BumpMap");
			Debug.Log ("BumpMap: " + tex2);
			Texture tex3 = other.renderer.material.GetTexture("_Cube");
			Debug.Log ("Cube: " + tex3);
			*/b_IsHitting = false;
		}
	}
}
