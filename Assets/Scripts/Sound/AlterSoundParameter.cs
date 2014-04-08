using UnityEngine;
using System.Collections;

public class AlterSoundParameter : MonoBehaviour {

	public FMOD_StudioEventEmitter FMOD_Emitter;

	public string s_Parameter;

	public float f_NewValueWhenInside = 0f;

	public float f_NewValueWhenOutside = 0f;

	private FMOD.Studio.ParameterInstance FMOD_Parameter;


	void Start()
	{
		FMOD_Emitter.Play ();
		FMOD_Parameter = FMOD_Emitter.getParameter(s_Parameter);
		FMOD_Parameter.setValue(f_NewValueWhenOutside);
	}

	void OnTriggerEnter(Collider other)
	{
		FMOD_Parameter.setValue (f_NewValueWhenInside);
	}

	void OnTriggerExit(Collider other)
	{
		FMOD_Parameter.setValue (f_NewValueWhenOutside);
	}
}
