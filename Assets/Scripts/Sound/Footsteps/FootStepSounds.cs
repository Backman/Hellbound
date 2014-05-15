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
	
	//the start acceses and saves a few "pointers" to the necessary scrips and variables
	//(this is to provide shortcuts to what we want to access and/or change)
	void Start(){
		
		while (m_Emitter == null) {
			m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		}
		
		FBack = FootBack.GetComponent<FootStepsHitBoxes> ();
		OtherFBack = OtherFootBack.GetComponent<FootStepsHitBoxes> ();
		
		surfaceTexture = gameObject.GetComponent<GetDominantTexture> ();
		m_Parameter = m_Emitter.getParameter("Surface");
	}
	
	
	void OnTriggerStay(Collider other){
		
		
		if(other.GetComponent<FootstepSurface>() != null){
			
			FootstepSurface newobject = other.GetComponent<FootstepSurface> ();

			if(m_StandingOn == null){
				m_StandingOn = newobject;
			}
			if(m_StandingOn.m_Priority <= newobject.m_Priority){
				m_StandingOn = newobject;
			}
		}
		
		//if our whole foot is placed on the ground, we havent played a sound this
		//"step" and the other foots backcollider isnt hitting anything we can play a sound
		//(this means that we are still moving foward)
		if(m_StandingOn != null){

			if(m_StandingOn.m_UseFootstepSurface){
				m_Parameter.setValue(m_StandingOn.m_Surface);
			}
			else if (surfaceTexture != null) {
				m_Parameter.setValue(surfaceTexture.m_SurfaceType);
			}
			
			if(FBack.b_IsHitting && m_Once && !OtherFBack.b_IsHitting){
				m_Once = false;
				m_Emitter.Play();
				m_StandingOn = null;
				Debug.Log("playing");
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
	
}