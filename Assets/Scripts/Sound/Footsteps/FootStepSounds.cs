using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootStepSounds : MonoBehaviour {
	
	/// <summary>
	/// FootstepsFront will need to be placed on the front collider on both feet
	/// This script also requires an "FMOD_StudioEventEmitter", a collider and
	/// pointers to the colliders on the back of the feet
	/// Anton Thorsell
	/// </summary>
	
	//the sound emitter (the source of the sound)
	private FMOD_StudioEventEmitter m_Emitter;
	
	private GetDominantTexture surfaceTexture;
	
	private FootstepSurface m_StandingOn;
	
	//a pointer to the sound-parameter (this can change the sound the emitter makes,
	//for example footsteps on wood or stone)
	private FMOD.Studio.ParameterInstance m_Parameter;
	
	//the start acceses and saves a few "pointers" to the necessary scrips and variables
	//(this is to provide shortcuts to what we want to access and/or change)
	void Start(){
		try{
		m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		}catch{
			Debug.LogWarning("No Emitter attached to this object, i guess i have to do EVERYTHING myself");
			//AttachFmodEmitter();
		}

		surfaceTexture = gameObject.GetComponent<GetDominantTexture> ();

		if(m_Parameter == null){
			try{
				m_Parameter = m_Emitter.getParameter("Surface");
			} catch  {
				Debug.LogWarning("FMOD parameter failed, will try again");
				//TryGetParameterAgain();
			}
		}
	}

	void OnTriggerEnter(Collider other){
		
		FootstepSurface newobject = other.GetComponent<FootstepSurface> ();
		
		if(newobject != null){
			
			if(m_StandingOn == null){
				m_StandingOn = newobject;
			}
			if(m_StandingOn.m_Priority <= newobject.m_Priority){
				m_StandingOn = newobject;
			}
			
			if(m_StandingOn.m_UseFootstepSurface){
				m_Parameter.setValue(m_StandingOn.m_Surface);
			}

			else if (surfaceTexture != null) {
				m_Parameter.setValue(surfaceTexture.m_SurfaceType);
			}
		}
	}

	public void PlayFootstepSound()
	{

			m_Emitter.Play();
	}
	
	//when the foot leaves the ground we can once again take a step
	void OnTriggerExit(Collider other){
		if(other.GetComponent<FootstepSurface>() != null){
			if(m_StandingOn == other.GetComponent<FootstepSurface>())
			{
				m_StandingOn = null;
			}
		}
	}


	private void TryGetParameterAgain()
	{
		Debug.Log ("TryGetParameterAgain");
		AttachFmodEmitter ();
		try{
			m_Parameter = m_Emitter.getParameter("Surface");
		}catch{
			Debug.Log("Get parameter failed Again");
		}
	}

	private void AttachFmodEmitter()
	{
		Debug.Log ("attachFMODEmitter");
		if(GetComponents<FMOD_StudioEventEmitter>().Length != 0)
		{
			foreach(FMOD_StudioEventEmitter see in GetComponents<FMOD_StudioEventEmitter>())
			{
				Destroy(see);
			}
		}
		if(GetComponent<FMOD_StudioEventEmitter>() == null)
		{
			gameObject.AddComponent<FMOD_StudioEventEmitter>();

			FMOD_StudioEventEmitter emitter = gameObject.GetComponent<FMOD_StudioEventEmitter>();
			emitter.startEventOnAwake = false;

			m_Emitter = emitter;
		}
	}
}