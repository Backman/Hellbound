using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterSoundParameter : MonoBehaviour {

	public string s_Parameter = "";
	public float f_InsideValue = 0f;

	private List<FMOD_StudioEventEmitter> m_Emitters = new List<FMOD_StudioEventEmitter>();
	private float f_OutValue = 0f;

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Rigidbody>() != null)
		{
			FMOD_StudioEventEmitter[] allEmitters = other.gameObject.GetComponentsInChildren<FMOD_StudioEventEmitter>();
			foreach(FMOD_StudioEventEmitter f in allEmitters)
			{
				m_Emitters.Add(f);
				if(f.getParameter(s_Parameter) != null)
				{
					float noPointerPlease = 0f;
					f.getParameter(s_Parameter).getValue(out noPointerPlease);
					f_OutValue = noPointerPlease;
					f.getParameter(s_Parameter).setValue(f_InsideValue);
				}
			}
		}

	}

	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<Rigidbody>() != null)
		{
			FMOD_StudioEventEmitter[] itterate = m_Emitters.ToArray ();
			foreach(FMOD_StudioEventEmitter f in itterate)
			{
				if(f.getParameter(s_Parameter) != null)
				{
					f.getParameter(s_Parameter).setValue(f_OutValue);
				}
				m_Emitters.Remove(f);
			}
		}
	}
}
