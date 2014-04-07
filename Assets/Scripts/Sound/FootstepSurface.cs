using UnityEngine;
using System.Collections;

public class FootstepSurface : MonoBehaviour {
	/// <summary>
	/// FootstepSurface script will need to be placed on any surface the player can walk on
	/// to change the sound the "footsteps" makes.
	/// Anton Thorsell
	/// </summary>


	public float f_Surface = 0f;

	private FMOD.Studio.ParameterInstance FMOD_Parameter;

	void Start () 
	{
		FMOD_Parameter = GameObject.FindGameObjectWithTag ("Player").GetComponent<FMOD_StudioEventEmitter> ().getParameter ("Surface");
	}

	void OnTriggerEnter(Collider other)
	{
		FMOD_Parameter.setValue (f_Surface);
	}
}
