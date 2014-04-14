using UnityEngine;
using System.Collections;

public class AlterSoundPlay : MonoBehaviour {
	/// <summary>
	/// Altersoundplay is to be placed on a collider (also requires a FMOD_StudioEventEmitter)
	/// This script will start or stop playing sounds depending if the player entered the collider
	/// You can also specify if the sound only should be played once (if so it will destroy itself when it is done)
	/// 
	/// Anton Thorsell
	/// </summary>

	public GameObject[] r_GameObjects;

	//variables if something should happen when the player enters/exits the collider
	public bool b_Enter = false;
	public bool b_Exit = false;

	//should it play once?
	public bool b_Once = false;
	private bool b_HavePlayedOnce = false;

	//the soundSource
	private FMOD_StudioEventEmitter m_Emitter;

	//makes a pointer to the emitter that is on this object
	void Start () 
	{
		m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
	}

	//if something enters the collider, depending on the variables 
	//stuff happens
	void OnTriggerEnter(Collider other)
	{
		if(b_Enter)
		{
			if(b_Once && b_HavePlayedOnce){
				Destroy(gameObject);
			}
			else{				
				m_Emitter.Stop();
				m_Emitter.Play();
				b_HavePlayedOnce = true;
			}
		}
	}

	//see the comments above
	void OnTriggerExit(Collider other)
	{
		if(b_Exit)
		{
			if(b_Once && b_HavePlayedOnce)
			{
				Destroy(gameObject);			
			}
			else
			{
				m_Emitter.Stop();
				m_Emitter.Play();
				b_HavePlayedOnce = true;
			}
		}
	}
}
