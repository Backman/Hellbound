using UnityEngine;
using System.Collections;

public class AlterSoundPlay : MonoBehaviour {

	public bool b_StartOnAwake = false;

	public bool b_AlterWhenExit = false;
	public bool b_PlayExit = false;

	public bool b_AlterWhenEnter = false;
	public bool b_PlayEnter = false;

	private FMOD_StudioEventEmitter m_Emitter;

	void Start () 
	{
		m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		if(b_StartOnAwake)
		{
			m_Emitter.Play();
		}
	}
	

	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (b_AlterWhenEnter)
		{
			if(b_PlayEnter)
			{
				Debug.Log("enter play");
				m_Emitter.Play();
			}
			else
			{
				Debug.Log("enter stop");
				m_Emitter.Stop();
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (b_AlterWhenExit)
		{
			if(b_PlayExit)
			{
				Debug.Log("exit play");
				m_Emitter.Play();
			}
			else
			{
				Debug.Log("exit stop");
				m_Emitter.Stop();
			}
		}
	}
}
