using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterOtherSoundParameters : MonoBehaviour {

	/// <summary>
	/// This script will allow hitboxes to alter other objects emitters parameters
	/// (the number of objects can be decided by the editor)
	/// Anton Thorsell
	/// </summary>

	public GameObject[] g_GameObjects;
	public string s_Parameter;

	public float f_InsideParameter = 0f;

	public float f_OutsideParameter = 0f;

	private List<FMOD.Studio.ParameterInstance> m_Parameters = new List<FMOD.Studio.ParameterInstance>();
	
	void Start () 
	{
		foreach(GameObject g in g_GameObjects)
		{
			m_Parameters.Add(g.GetComponent<FMOD_StudioEventEmitter>().getParameter(s_Parameter));
			m_Parameters[m_Parameters.Count-1].setValue(f_OutsideParameter);
		}
	}


	void OnTriggerEnter(Collider other)
	{
		FadeIn ();
	}

	void OnTriggerExit(Collider other)
	{
		FadeOut ();
	}

	private IEnumerator FadeIn()
	{
		//this fade is currently retarded
		float newValue = f_OutsideParameter;
		while(newValue != f_InsideParameter)
		{
			foreach(FMOD.Studio.ParameterInstance f in m_Parameters)
			{
				f.setValue(newValue);
			}
		}
	}

	private IEnumerator FadeOut()
	{
		//this fade is currently retarded
		float newValue = f_InsideParameter;
		float addThis = (f_InsideParameter - f_OutsideParameter) / 100f;
		while(newValue != f_OutsideParameter)
		{

			foreach(FMOD.Studio.ParameterInstance f in m_Parameters)
			{
				f.setValue(newValue);
			}			
		}
	}
}