using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterSoundParameter : MonoBehaviour {

	public string s_Parameter = "";
	public float f_InsideValue = 0f;
	public float f_OutSideValue = 0f;

	private List<FMOD_StudioEventEmitter> m_Emitters = new List<FMOD_StudioEventEmitter>();

	void OnTriggerEnter(Collider other)
	{
		FMOD_StudioEventEmitter[] allEmitters = other.gameObject.GetComponentsInChildren<FMOD_StudioEventEmitter>();

		foreach(FMOD_StudioEventEmitter f in allEmitters)
		{
			m_Emitters.Add(f);
			if(f.getParameter(s_Parameter) != null)
			{
				f.getParameter(s_Parameter).setValue(f_InsideValue);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		FMOD_StudioEventEmitter[] itterate = m_Emitters.ToArray ();
		foreach(FMOD_StudioEventEmitter f in itterate)
		{
			if(f.getParameter(s_Parameter) != null)
			{
				f.getParameter(s_Parameter).setValue(f_OutSideValue);
			}
			m_Emitters.Remove(f);
		}
	}
}
