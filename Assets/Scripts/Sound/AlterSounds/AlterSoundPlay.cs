using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlterSoundPlay : MonoBehaviour {
	/// <summary>
	/// AlterSoundPlay was written by Anton Thorsell
	/// 
	/// Altersoundplay is to be placed on a collider (also requires a pointer to an 
	/// object with an FMOD_StudioEventEmitter)
	/// This script will start or stop playing sounds depending if the player entered/exited the collider
	/// You can also specify if the sound only should be played once (if so it will destroy itself when it is done)
	/// 
	/// </summary>

	public GameObject[] r_GameObjects;


	public string m_Path = "event:/";
	public bool m_PlayOneShot = false;

	public Vector3 m_Position;
	//variables if something should happen when the player enters/exits the collider
	public bool m_Enter = false;
	public bool m_Exit = false;
	public bool m_Once = false;

	private List<FMOD_StudioEventEmitter> r_EmitterList = new List<FMOD_StudioEventEmitter>();
	private bool m_HavePlayedOnce = false;
	private bool m_Inside = false;

	void Start(){
		foreach (GameObject g in r_GameObjects) {
			if(g.GetComponent<FMOD_StudioEventEmitter>() != null){
				r_EmitterList.Add(g.GetComponent<FMOD_StudioEventEmitter>());
			}
		}
	}



	//if something enters the collider, depending on the variables 
	//stuff happens
	void OnTriggerEnter(Collider other){
		if(other.GetComponent<Rigidbody>() != null){

			if(m_Enter && !m_Inside)
			{
				if(m_Once && m_HavePlayedOnce){
					Destroy(gameObject);
				}
				else if(m_PlayOneShot){
					PlayShot();
					m_HavePlayedOnce = true;
				}
				else if(!m_PlayOneShot){
					PlayEvent();
					m_HavePlayedOnce = true;
				}
			}
			m_Inside = true;
		}
	}

	//see the comments above
	void OnTriggerExit(Collider other){
		if(other.GetComponent<Rigidbody>() != null){
			
			if(m_Exit && m_Inside){
				if(m_Once && m_HavePlayedOnce){
					Destroy(gameObject);
				}
				else if(m_PlayOneShot){
					PlayShot();
					m_HavePlayedOnce = true;
				}
				else if(!m_PlayOneShot){
					PlayEvent();
					m_HavePlayedOnce = true;
				}
			}
			m_Inside = false;
		}
	}


	private void PlayShot(){
		FMOD_StudioSystem.instance.PlayOneShot(m_Path, m_Position);
	}


	private void PlayEvent(){
		foreach (FMOD_StudioEventEmitter e in r_EmitterList) {
			e.Play();
		}
	}

}
