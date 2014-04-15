using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterSoundParameter : MonoBehaviour {

	/// <summary>
	/// This script will allow hitboxes to alter other objects emitters parameters
	/// (the number of objects can be decided by the editor)
	/// Anton Thorsell
	/// </summary>

	public GameObject[] g_GameObjects;
	public string s_Parameter = "";
	public float m_InsideParameter = 0f;
	public float m_OutsideParameter = 0f;
	public bool m_RevertToOriginalValue = false;
	public bool m_UseFade = false;
	public float m_FadeSpeed = 100f;

	private bool m_Inside = false;
	private List<FMOD.Studio.ParameterInstance> r_ParameterCollection = new List<FMOD.Studio.ParameterInstance>();
	private float m_OriginalValue = 0f;
	
	void Start () 
	{
		foreach(GameObject g in g_GameObjects){
			if(g.GetComponent<FMOD_StudioEventEmitter>() != null)
			r_ParameterCollection.Add(g.GetComponent<FMOD_StudioEventEmitter>().getParameter(s_Parameter));
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (m_UseFade && !m_Inside) {
			m_Inside = true;
			StartCoroutine("FadeIn");
		}
		else if (!m_UseFade && !m_Inside){
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				p.setValue(m_InsideParameter);
			}
		}
	}


	void OnTriggerExit(Collider other)
	{
		if (m_UseFade && m_Inside) {
			m_Inside = false;
			StartCoroutine("FadeOut");
		}
		else if (!m_UseFade && m_Inside){
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				p.setValue(m_OutsideParameter);
			}
		}
	}


	private IEnumerator FadeIn()
	{
		StopCoroutine("FadeOut");

		float speed = (m_InsideParameter - m_OutsideParameter) / m_FadeSpeed;
		float currentValue = m_OutsideParameter;

		if (m_RevertToOriginalValue) {
			float noPointerPlease = 0f;
			r_ParameterCollection[0].getValue(out noPointerPlease);
			m_OriginalValue = noPointerPlease;
			currentValue = m_OriginalValue;
		}
		else{
			float noPointerPlease = 0f;
			r_ParameterCollection[0].getValue(out noPointerPlease);
			currentValue = noPointerPlease;
		}

		while (currentValue != m_InsideParameter) {
			yield return new WaitForSeconds (0.01f);

			currentValue += speed;
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				p.setValue(currentValue);	
			}
		}
		yield return 0;
	}


	private IEnumerator FadeOut()
	{
		StopCoroutine("FadeIn");

		float speed = (m_OutsideParameter - m_InsideParameter) / m_FadeSpeed;
		float currentValue = m_InsideParameter;

		if (m_RevertToOriginalValue) {
			currentValue = m_OriginalValue;
			m_OriginalValue = 0f;
		}
		else{
			float noPointerPlease = 0f;
			r_ParameterCollection[0].getValue(out noPointerPlease);
			currentValue = noPointerPlease;
		}

		while (currentValue != m_OutsideParameter) {
			yield return new WaitForSeconds (0.01f);

			currentValue += speed;
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				p.setValue(currentValue);
			}
		}
		yield return 0;
	}
}