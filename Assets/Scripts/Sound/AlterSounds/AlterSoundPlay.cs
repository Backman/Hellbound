using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterSoundPlay : MonoBehaviour {
	/// <summary>
	/// Altersoundplay is to be placed on a collider (also requires a FMOD_StudioEventEmitter)
	/// This script will start or stop playing sounds depending if the player entered the collider
	/// You can also specify if the sound only should be played once (if so it will destroy itself when it is done)
	/// 
	/// Anton Thorsell
	/// </summary>

	public string m_Path = "event:/";

	//variables if something should happen when the player enters/exits the collider
	public bool m_Enter = false;
	public bool m_Exit = false;
	public bool m_Once = false;

	private bool m_HavePlayedOnce = false;
	private bool m_Inside = false;

	//if something enters the collider, depending on the variables 
	//stuff happens
	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Rigidbody>() != null){

			if(m_Enter && !m_Inside)
			{
				if(m_Once && m_HavePlayedOnce){
					Destroy(gameObject);
				}
				else{
					FMOD_StudioSystem.instance.PlayOneShot(m_Path, transform.position);
					m_HavePlayedOnce = true;
				}
			}
			m_Inside = true;
		}
	}

	//see the comments above
	void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<Rigidbody>() != null){
			
			if(m_Exit && m_Inside)
			{
				if(m_Once && m_HavePlayedOnce){
					Destroy(gameObject);
				}
				else{
					FMOD_StudioSystem.instance.PlayOneShot(m_Path, transform.position);
					m_HavePlayedOnce = true;
				}
			}
			m_Inside = false;
		}
	}
}
