using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootStepsFront : MonoBehaviour {

	/// <summary>
	/// FootstepsFront will need to be placed on the front collider on both feet
	/// This script also requires an "FMOD_StudioEventEmitter", a collider and
	/// pointers to the colliders on the back of the feet
	/// Anton Thorsell
	/// </summary>

	//pointers to the other backcolliders
	public GameObject FootBack;
	public GameObject OtherFootBack;

	//the sound emitter (the source of the sound)
	private FMOD_StudioEventEmitter f_Emitter;

	//all we need from the colliders on the back of the feed are
	//the scrips (so we save them separately)
	private FootStepsBack BackScript;
	private FootStepsBack OtherFootScript;

	private GetDominantTexture surfaceTexture;

	//a pointer to the sound-parameter (this can change the sound the emitter makes,
	//for example footsteps on wood or stone)
	private FMOD.Studio.ParameterInstance f_Parameter = null;

	//we only want one footstepsound for every step
	private bool m_Once = true;

	//the start acceses and saves a few "pointers" to the necessary scrips and variables
	//(this is to provide shortcuts to what we want to access and/or change)
	void Start(){
		f_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
		BackScript = FootBack.GetComponent<FootStepsBack> ();
		OtherFootScript = OtherFootBack.GetComponent<FootStepsBack> ();
		f_Parameter = f_Emitter.getParameter("Surface");
		surfaceTexture = gameObject.GetComponent<GetDominantTexture> ();
	}

	//
	void OnTriggerStay(Collider other){
		//is the object we collided with have a footstepsurface?
		if(other.GetComponent<FootstepSurface>() != null){

			//if our whole foot is placed on the ground, we havent played a sound this
			//"step" and the other foots backcollider isnt hitting anything we can play a sound
			//(this means that we are still moving foward)
			if(BackScript.b_IsHitting && m_Once && !OtherFootScript.b_IsHitting){
				m_Once = false;
				f_Emitter.Stop();
				f_Emitter.Play();
			}
			//if both backcolliders are hitting something we know we have stopped moving
			//(we can add a sound for "footstepstop" or something here)
			if(BackScript.b_IsHitting && OtherFootScript.b_IsHitting){
				m_Once = false;
			}

		}
	}

	//when the foot leaves the ground we can once again take a step
	void OnTriggerExit(Collider other){
		m_Once = true;
	}



}