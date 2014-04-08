using UnityEngine;
using System.Collections;

public class FootSteps : MonoBehaviour {

	/// <summary>
	/// FootSteps will need to placed on the player foot colliders
	/// </summary>
	

	private FMOD_StudioEventEmitter FMOD_Emitter;
	private FMOD.Studio.ParameterInstance FMOD_Parameter;


	void Start()
	{
		FMOD_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		FMOD_Parameter = FMOD_Emitter.getParameter("Surface");
	}

	void OnCollisionEnter(Collision other)
	{
		Debug.Log("got here!");
		if(other.gameObject.GetComponent<FootstepSurface>())
		{
			Debug.Log("got here too!");
			FMOD_Parameter.setValue(other.gameObject.GetComponent<FootstepSurface>().f_Surface);
			FMOD_Emitter.Stop();
			FMOD_Emitter.Play();
		}
	}
}
