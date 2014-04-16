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
	public float m_FadeSpeed = 1f;

	public bool m_DestroyOnExit = false;

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


	void OnTriggerEnter(Collider other){
		if (m_UseFade && !m_Inside) {
			m_Inside = true;
			StartCoroutine("FadeIn");
		}
		else if (!m_UseFade && !m_Inside){
			m_Inside = true;
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(r_ParameterCollection != null){
					p.setValue(m_InsideParameter);
				}
			}
		}
	}


	void OnTriggerExit(Collider other){
		if (m_UseFade && m_Inside) {
			m_Inside = false;
			StartCoroutine("FadeOut");
		}
		else if (!m_UseFade && m_Inside){
			m_Inside = false;
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(r_ParameterCollection != null){
					p.setValue(m_OutsideParameter);
				}
			}
			if(m_DestroyOnExit){
				r_ParameterCollection.Clear();
			}
		}
	}


	private IEnumerator FadeIn()
	{
		StopCoroutine("FadeOut");
		
		float startvalue = getCurrentValue (r_ParameterCollection[0]);
		float newValue = startvalue;
		
		while (newValue != m_InsideParameter) {
			
			newValue = moveTo(startvalue, newValue, m_InsideParameter);
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(r_ParameterCollection != null){
					p.setValue(newValue);
				}
			}
		}
		
		yield return 0;
	}


	private IEnumerator FadeOut(){
		StopCoroutine("FadeIn");

		float startvalue = getCurrentValue (r_ParameterCollection[0]);
		float newValue = startvalue;

		while (newValue != m_OutsideParameter) {

			newValue = moveTo(startvalue, newValue, m_OutsideParameter);
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(r_ParameterCollection != null){
					p.setValue(newValue);
				}
			}
		}

		if(m_DestroyOnExit){
			r_ParameterCollection.Clear();
		}
		yield return 0;
	}
	
	private float getCurrentValue(FMOD.Studio.ParameterInstance parameter){
	
		float Current = 0f;
		float noPointerPlease = 0f;
		
		r_ParameterCollection[0].getValue(out noPointerPlease);
		Current = noPointerPlease;
		
		return Current;
	}

	private float moveTo(float beginning, float at, float to){

		float speed = Time.deltaTime / (m_FadeSpeed * (to - beginning));
		at += speed;
		return at;
	}
}