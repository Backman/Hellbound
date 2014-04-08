using UnityEngine;
using System.Collections;

public class SoundObject : MonoBehaviour {

	public float f_Volume = 100f;
	public string s_Tag;

	public FMOD_StudioEventEmitter FMOD_Emitter;

	void Start ()
	{
		FMOD_Emitter = gameObject.GetComponent<FMOD_StudioEventEmitter> ();
	}
}
