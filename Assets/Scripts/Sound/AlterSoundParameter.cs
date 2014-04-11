using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterSoundParameter : MonoBehaviour {
	/// <summary>
	/// Altersoundparameter is to be placed on a collider
	/// This script will alter existing sounds on the object that entered the collider
	/// and return the value it had before entering the collider
	/// (it will only alter the parameter specified by the editor)
	/// 
	/// Anton Thorsell
	/// </summary>

	//the parameter to be changed
	public string s_Parameter = "";

	//what the parameter should be inside
	public float f_InsideValue = 0f;

	//a list of all emitters that the object had
	private List<FMOD_StudioEventEmitter> m_Emitters = new List<FMOD_StudioEventEmitter>();
	private float f_OutValue = 0f;

	//when something enters the script will save all emitters and the value it had before
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

	//when something exits this function will return the value it originally had 
	// and remove the pointers to it.
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
