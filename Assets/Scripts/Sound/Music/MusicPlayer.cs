using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {
	/// <summary>
	/// This script needs to be placed on the character (more specifically the object that
	/// have the player collider)
	/// </summary>


	public float m_FadeSpeed = 100f;

	public GameObject[] r_GameObjects;
	public string[] m_ParameterNames;

	private Dictionary<string, FMOD.Studio.ParameterInstance> r_Parameter;


	private bool b_AlterSound = false;

	void Start () 
	{
		if (m_FadeSpeed == 0)
						m_FadeSpeed = 1;
		foreach (GameObject g in r_GameObjects) {
			foreach(string s in m_ParameterNames){

				if(g.GetComponent<FMOD_StudioEventEmitter>().getParameter(s) != null){
					r_Parameter.Add(s, g.GetComponent<FMOD_StudioEventEmitter>().getParameter(s));
				}
			}
		}
	}


	//have not implemented "save value"
	void OnTriggerEnter(Collider other)
	{
		MusicZone desiredComponent = collider.GetComponent<MusicZone> ();
		if (desiredComponent != null) {
			if(desiredComponent.m_FadeSound){
				FadeIn(desiredComponent);
			}
			else{
				r_Parameter[desiredComponent.m_parametername].setValue(desiredComponent.m_Insidevalue);
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		MusicZone desiredComponent = collider.GetComponent<MusicZone> ();
		if (desiredComponent != null) {
			if(desiredComponent.m_FadeSound){
				FadeOut(desiredComponent);
			}
			else{
				r_Parameter[desiredComponent.m_parametername].setValue(desiredComponent.m_Insidevalue);
			}
		}
	}

	private IEnumerator FadeIn(MusicZone zone)
	{
		/*float speed = (zone.m_Insidevalue - zone.m_Outstidevalue) / m_FadeSpeed;
		float currentValue = zone.m_Outstidevalue;
		
		while (currentValue == zone.m_Insidevalue) {
			
			currentValue += speed;
			foreach(FMOD.Studio.ParameterInstance p in r_Parameter){
				p.setValue(currentValue);
			}
		}*/
		yield return 0;
	}


	private IEnumerator FadeOut(MusicZone zone)
	{
		/*float speed = (0 - 0) / m_FadeSpeed;
		float currentValue = 0;
		
		while (currentValue == 0) {
			
			currentValue += speed;
			foreach(FMOD.Studio.ParameterInstance p in r_Parameter){
				p.setValue(currentValue);
			}
		}*/
		yield return 0;
	}

}
