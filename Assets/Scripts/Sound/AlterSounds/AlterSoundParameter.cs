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


	private IEnumerator FadeIn(){
		StopCoroutine("FadeOut");
		
		float Current = 0f;
		float noPointerPlease = 0f;
		
		foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
			p.getValue(out noPointerPlease);
		}
		
		Current = noPointerPlease;
		
		float startvalue = Current;

		while (Current != m_InsideParameter) {
			
			float speed = Time.deltaTime / (m_FadeSpeed * (m_OutsideParameter - startvalue));
			
			Current += speed;

			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(r_ParameterCollection != null){
					p.setValue(Current);
				}
			}
			Debug.Log(Current);
		}
		yield return 0;
	}


	private IEnumerator FadeOut(){
		StopCoroutine("FadeIn");

		float Current = 0f;
		float noPointerPlease = 0f;
		
		foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
			p.getValue(out noPointerPlease);
		}

		Current = noPointerPlease;

		float startvalue = Current;

		while (Current != m_OutsideParameter) {

			float speed = Time.deltaTime / (m_FadeSpeed * (m_OutsideParameter - startvalue));

			Current += speed;

			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(r_ParameterCollection != null){
					p.setValue(Current);
				}
			}
			Debug.Log(Current);
		}

		if(m_DestroyOnExit){
			r_ParameterCollection.Clear();
		}

		yield return 0;
	}
}