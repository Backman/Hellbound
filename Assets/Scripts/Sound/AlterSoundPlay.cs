using UnityEngine;
using System.Collections;

public class AlterSoundPlay : MonoBehaviour {

	public bool b_PlayExit = false;
	public bool b_PlayEnter = false;

	public bool b_Once = false;
	private bool b_HavePlayedOnce = false;

	private FMOD_StudioEventEmitter m_Emitter;

	void Start () 
	{
		m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if(b_Once && b_HavePlayedOnce)
		{
			Destroy(gameObject);
		}
		if(b_PlayEnter)
		{
			m_Emitter.Stop();
			m_Emitter.Play();
			b_HavePlayedOnce = true;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(b_Once && b_HavePlayedOnce)
		{
			Destroy(gameObject);			
		}
		if(b_PlayExit)
		{
			m_Emitter.Stop();
			m_Emitter.Play();
			b_HavePlayedOnce = true;
		}
	}
}
