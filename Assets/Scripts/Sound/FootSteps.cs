using UnityEngine;
using System.Collections;

public class FootSteps : MonoBehaviour {

	/// <summary>
	/// FootSteps will need to placed on the player object.
	/// </summary>
	
	private FMOD_StudioEventEmitter m_Emitter;

	void Start()
	{
		m_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
	}
}
