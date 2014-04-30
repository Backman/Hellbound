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
		StartCoroutine ("waitForParameters");
		if(m_FadeSpeed == 0f){
			m_FadeSpeed = 1;
		}
	}

	private IEnumerator waitForParameters()
	{
<<<<<<< HEAD
=======
		int d = g_GameObjects.Length;


>>>>>>> f0c2db97c357b8d419747579f60a3c791ddb7504
		for (int n = 0; n != g_GameObjects.Length;) {
			yield return new WaitForSeconds(0.01f);
			r_ParameterCollection.Clear();
			foreach(GameObject g in g_GameObjects){
				if(g.GetComponent<FMOD_StudioEventEmitter>() != null){
					r_ParameterCollection.Add(g.GetComponent<FMOD_StudioEventEmitter>().getParameter(s_Parameter));
				}
			}
			n = r_ParameterCollection.Count;
		}
		yield return 0;
	}


	void OnTriggerEnter(Collider other){
		if(other.GetComponent<Rigidbody>() != null){
			if (m_UseFade && !m_Inside) {
				m_Inside = true;
				StartCoroutine("FadeIn");
			}
			else if (!m_UseFade && !m_Inside){
				m_Inside = true;
				foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
<<<<<<< HEAD
					if(p != null){
=======
					if(r_ParameterCollection != null){
>>>>>>> f0c2db97c357b8d419747579f60a3c791ddb7504
						p.setValue(m_InsideParameter);
					}
				}
			}
		}
	}


	void OnTriggerExit(Collider other){
			if(other.GetComponent<Rigidbody>() != null){
		if (m_UseFade && m_Inside) {
			m_Inside = false;
			StartCoroutine("FadeOut");
		}
		else if (!m_UseFade && m_Inside){
			m_Inside = false;
			foreach(FMOD.Studio.ParameterInstance p in r_ParameterCollection){
				if(p != null){
					p.setValue(m_OutsideParameter);
				}
			}
			if(m_DestroyOnExit){
				r_ParameterCollection.Clear();
			}
		}
	}
}


	private IEnumerator FadeIn(){
		StopCoroutine("FadeOut");

		float noPointersPlease = 0f;

		foreach (FMOD.Studio.ParameterInstance p in r_ParameterCollection) {
			p.getValue(out noPointersPlease);
		}
	

		m_OriginalValue = noPointersPlease;
		float beginValue = noPointersPlease;
		float current = noPointersPlease;

		while(current != m_InsideParameter){

			current = MoveTo(current, beginValue, m_InsideParameter);

			foreach (FMOD.Studio.ParameterInstance p in r_ParameterCollection) {
				p.setValue(current);
			}
			Debug.Log(current);
			yield return new WaitForSeconds(0.01f);
		}

		yield return 0;
	}


	private IEnumerator FadeOut(){
		StopCoroutine("FadeIn");

		float noPointersPlease = m_InsideParameter;
		float current = m_InsideParameter;


		foreach (FMOD.Studio.ParameterInstance p in r_ParameterCollection) {
			p.getValue(out noPointersPlease);
		}
		

		float beginValue = noPointersPlease;
		current = noPointersPlease;

		
		float desiredValue = m_OutsideParameter;
		if(m_RevertToOriginalValue){
			desiredValue = m_OriginalValue;
		}
		
		while(current != desiredValue){
			
			current = MoveTo(current, beginValue, desiredValue);
			
			foreach (FMOD.Studio.ParameterInstance p in r_ParameterCollection) {
				p.setValue(current);
			}
			Debug.Log(current);
			yield return new WaitForSeconds(0.01f);
		}

		if (m_DestroyOnExit) {
			Destroy(gameObject);
		}

		yield return 0;
	}


	private float MoveTo(float at, float beginValue, float desiredValue){

		float returnThis = at;

		float oneStep = (desiredValue - beginValue) / (m_FadeSpeed * 50f);
		returnThis += oneStep;

		if(!(returnThis < desiredValue - oneStep) && !(returnThis > desiredValue + oneStep)){
			return desiredValue;
		}
		if (beginValue < desiredValue) {
			if(at > desiredValue){
				return desiredValue;
			}
		}
		else if(beginValue > desiredValue){
			if(at < desiredValue)
				return desiredValue;
		}

		return returnThis;
	}

	void OnDisable()
	{
		r_ParameterCollection.Clear ();
	}
}