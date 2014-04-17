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
	public GameObject FootFront;
	public GameObject OtherFootBack;

	private FootStepsHitBoxes FBack;
	private FootStepsHitBoxes FFront;
	private FootStepsHitBoxes OtherFBack;


	//the sound emitter (the source of the sound)
	private FMOD_StudioEventEmitter f_Emitter;

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

		FBack = FootBack.GetComponent<FootStepsHitBoxes> ();
		FFront = FootFront.GetComponent<FootStepsHitBoxes> ();
		OtherFBack = OtherFootBack.GetComponent<FootStepsHitBoxes> ();

		f_Parameter = f_Emitter.getParameter("Surface");
		surfaceTexture = gameObject.GetComponent<GetDominantTexture> ();
	}


	void OnTriggerStay(Collider other){
		//is the object we collided with have a footstepsurface?
		if(other.GetComponent<FootstepSurface>() != null){

			f_Parameter.setValue(surfaceTexture.m_SurfaceType);
			//if our whole foot is placed on the ground, we havent played a sound this
			//"step" and the other foots backcollider isnt hitting anything we can play a sound
			//(this means that we are still moving foward)
			if(FBack.b_IsHitting && m_Once && !OtherFBack.b_IsHitting && !FFront.b_IsHitting){
				m_Once = false;
				f_Emitter.Stop();
				f_Emitter.Play();
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
			coolDown(0.25f);
		}
	}


	private IEnumerator coolDown(float seconds){
		yield return new WaitForSeconds(seconds);
		m_Once = true;
	}

}