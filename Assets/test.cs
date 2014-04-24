using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	void onEnable()
	{
		gameObject.GetComponent<FMOD_StudioEventEmitter> ().Play ();
	}
}
