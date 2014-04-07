using UnityEngine;
using System.Collections;

public class AlterSoundParameter : MonoBehaviour {

	public GameObject g_GameObjectToAlter;

	public string s_Parameter;

	public float f_StartValue = 0;
	
	public bool b_Enter = false;
	public float f_Enter = 0f;
	
	public bool b_Exit = false;
	public float f_Exit = 0f;

	private FMOD.Studio.ParameterInstance FMOD_Parameter;
	private FMOD.Studio.EventInstance FMOD_Event;

	void Start()
	{
		FMOD_Event = FMOD_StudioSystem.instance.getEvent ("event:/Footsteps");
		FMOD_Event.getParameter(s_Parameter, out FMOD_Parameter);
	}

	void OnTriggerEnter(Collider other)
	{
		if (b_Enter)
		{
			FMOD_Parameter.setValue(f_Enter);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (b_Exit)
		{
			FMOD_Parameter.setValue(f_Exit);
		}
	}
}
