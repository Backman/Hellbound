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


	// TODO: objects get removed when not actually exiting.
	// I know what to do (anton)

	//pointers to the other backcolliders
	public GameObject FootBack;
	public GameObject OtherFootBack;

	private FootStepsHitBoxes FBack;
	private FootStepsHitBoxes OtherFBack;
	
	//the sound emitter (the source of the sound)
	private FMOD_StudioEventEmitter m_Emitter;

	private GetDominantTexture surfaceTexture;

	private List<GameObject> m_StandingOn = new List<GameObject>();
	private int m_HighestPriority;

	//a pointer to the sound-parameter (this can change the sound the emitter makes,
	//for example footsteps on wood or stone)
	private FMOD.Studio.ParameterInstance m_Parameter = null;

	//we only want one footstepsound for every step
	private bool m_Once = true;
	private float m_UseThisValue = 0f;

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

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<FootstepSurface> () != null) {
			if(!m_StandingOn.Contains(other.gameObject)){
				m_StandingOn.Add (collider.gameObject);
				updatePrioritySurface();
			}
		}
	}

	void OnTriggerStay(Collider other){
		//is the object we collided with have a footstepsurface?
		if (other.GetComponent<FootstepSurface> () != null) {
			if (surfaceTexture != null) {
				m_Parameter.setValue (surfaceTexture.m_SurfaceType);
			}
			//if our whole foot is placed on the ground, we havent played a sound this
			//"step" and the other foots backcollider isnt hitting anything we can play a sound
			//(this means that we are still moving foward)
			if (FBack.b_IsHitting && m_Once && !OtherFBack.b_IsHitting) {
					m_Once = false;
					m_Emitter.Stop ();
					m_Emitter.Play ();
			}
			//if both backcolliders are hitting something we know we have stopped moving
			//(we can add a sound for "footstepstop" or something here)
			if (FBack.b_IsHitting && OtherFBack.b_IsHitting) {
					m_Once = false;
			}
		}
	}


	//when the foot leaves the ground we can once again take a step
	void OnTriggerExit(Collider other){
		if(other.GetComponent<FootstepSurface>() != null){
			m_Once = true;
			if (m_StandingOn.Contains (other.gameObject)) {
				m_StandingOn.Remove(other.gameObject);
			}
		}
	}


	private void updatePrioritySurface(){
		int highest = 0;
		foreach (GameObject g in m_StandingOn) {
			FootstepSurface fss = g.GetComponent<FootstepSurface>();
			if(fss.m_Priority > highest){ //---------------------------------
				highest = fss.m_Priority;
				if(fss.m_UseFootstepSurface){
					m_UseThisValue = fss.m_Surface;
				}
				else{
					m_UseThisValue = surfaceTexture.m_SurfaceType;
				}
			}
		}
	}
}