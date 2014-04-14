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

	public float m_InsideParameter = 0f;

	public float m_OutsideParameter = 0f;


	public bool m_UseFade = false;
	//the amount of frames untill the fade is complete
	public float m_FadeSpeed = 100f;

	private List<FMOD.Studio.ParameterInstance> r_Parameters = new List<FMOD.Studio.ParameterInstance>();
	
	void Start () 
	{
		foreach(GameObject g in g_GameObjects){
			r_Parameters.Add(g.GetComponent<FMOD_StudioEventEmitter>().getParameter(s_Parameter));
			r_Parameters[r_Parameters.Count-1].setValue(m_OutsideParameter);
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (m_UseFade) {
			FadeIn();
		}
		else{
			foreach(FMOD.Studio.ParameterInstance p in r_Parameters){
				p.setValue(m_InsideParameter);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (m_UseFade) {
			FadeOut();
		}
		else{
			foreach(FMOD.Studio.ParameterInstance p in r_Parameters){
				p.setValue(m_OutsideParameter);
			}
		}
	}

	private IEnumerator FadeIn()
	{
		float speed = (m_InsideParameter - m_OutsideParameter) / m_FadeSpeed;
		float currentValue = m_OutsideParameter;

		while (currentValue == m_InsideParameter) {

			currentValue += speed;
			foreach(FMOD.Studio.ParameterInstance p in r_Parameters){
				p.setValue(currentValue);
			}
		}
		yield return 0;
	}

	private IEnumerator FadeOut()
	{
		float speed = (m_OutsideParameter - m_InsideParameter) / m_FadeSpeed;
		float currentValue = m_OutsideParameter;

		while (currentValue == m_OutsideParameter) {

			currentValue += speed;
			foreach(FMOD.Studio.ParameterInstance p in r_Parameters){
				p.setValue(currentValue);
			}
		}
		yield return 0;
	}
}