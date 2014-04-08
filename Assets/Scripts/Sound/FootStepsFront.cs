using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootStepsFront : MonoBehaviour {

	/// <summary>
	/// FootSteps will need to placed on the player foot colliders
	/// </summary>

	public GameObject FootBack;
	public GameObject OtherFootBack;

	private FMOD_StudioEventEmitter f_Emitter;
	private FMOD.Studio.ParameterInstance FMOD_Parameter;
	private FootStepsBack BackScript;
	private FootStepsBack OtherFootScript;

	private bool m_Once = true;

	void Start()
	{
		f_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		FMOD_Parameter = f_Emitter.getParameter ("Surface");
		BackScript = FootBack.GetComponent<FootStepsBack> ();
		OtherFootScript = OtherFootBack.GetComponent<FootStepsBack> ();
	}

	void OnTriggerStay(Collider other)
	{
		FMOD_Parameter.setValue (other.gameObject.GetComponent<FootstepSurface> ().f_Surface);
		if(BackScript.b_IsHitting && m_Once && !OtherFootScript.b_IsHitting)
		{
			m_Once = false;
			f_Emitter.Stop();
			f_Emitter.Play();
		}
		if(BackScript.b_IsHitting && OtherFootScript.b_IsHitting)
		{
			m_Once = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		m_Once = true;
	}

}