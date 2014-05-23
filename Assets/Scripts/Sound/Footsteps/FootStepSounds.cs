using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootStepSounds : MonoBehaviour {
	
	/// <summary>
	/// FootstepSounds was written by Anton Thorsell
	/// 
	/// The script FootstepSounds handles when the footstep sounds should play
	/// and what type of footstep sounds that should be played
	/// 
	/// FootstepSounds with the help of the scripts footstepsurface, footstepHitBoxes and 
	/// GetDominantTexture can figure out what type of footstep sound that should be played
	/// (theese scripts are mandatory for footstepsounds)
	/// 
	/// note: the functions AttachFmodEmitter and TryGetParameterAgain
	/// can in theory fix a few errors (if they appear) but do not do so.
	/// This can either be beacuse of errors in FMOD (have had problems with fmod before)
	/// or inproper use of how you attach FMOD_StudioEventEmitter to an object inside a script
	/// 
	/// 
	/// 
	/// </summary>
	
	//pointers to the other backcolliders
	public GameObject FootBack;
	public GameObject OtherFootBack;
	
	private FootStepsHitBoxes FBack;
	private FootStepsHitBoxes OtherFBack;
	
	//the sound emitter (the source of the sound)
	private FMOD_StudioEventEmitter m_Emitter;
	
	private GetDominantTexture surfaceTexture;
	
	private FootstepSurface m_StandingOn;
	
	//a pointer to the sound-parameter (this can change the sound the emitter makes,
	//for example footsteps on wood or stone)
	private FMOD.Studio.ParameterInstance m_Parameter;
	
	//we only want one footstepsound for every step
	private bool m_Once = true;
	
	//the start access and saves a few "pointers" to the necessary scrips and variables
	//(this is to provide shortcuts to what we want to access and/or change)
	void Start(){
		try{
		m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		}catch{
			Debug.LogWarning("No Emitter attached to this object, i guess i have to do EVERYTHING myself");
			//AttachFmodEmitter();
		}
		
		FBack = FootBack.GetComponent<FootStepsHitBoxes> ();
		OtherFBack = OtherFootBack.GetComponent<FootStepsHitBoxes> ();

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
	
	void OnTriggerStay(){

		//if our whole foot is placed on the ground, we havent played a sound this
		//"step" and the other foots backcollider isnt hitting anything we can play a sound
		//(this means that we are still moving foward)
		if(m_StandingOn != null){
			
			if(FBack.b_IsHitting && m_Once && !OtherFBack.b_IsHitting){
				m_Once = false;
				m_Emitter.Play();
				m_StandingOn = null;
				Debug.Log("PLAY");
			}
			//if both backcolliders are hitting something we know we have stopped moving
			//(we can add a sound for "footstepstop" or something here)
			if(FBack.b_IsHitting && OtherFBack.b_IsHitting){
				m_Once = false;
			}
		}
	}
	
	//when the foot leaves the ground we can once again take a step
	void OnTriggerExit(Collider other){
		if(other.GetComponent<FootstepSurface>() != null){
			m_Once = true;
			if(m_StandingOn == other.GetComponent<FootstepSurface>())
			{
				m_StandingOn = null;
			}
		}
	}

	//This function is obsolete and do not function the way i want
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
	
	//This function is obsolete and do not function the way i want
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