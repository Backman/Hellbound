using UnityEngine;
using System.Collections;

public class MusicZone : MonoBehaviour {
	/// <summary>
	/// This needs to be placed on a object with a collider that represents an area where
	/// a specific music thing should be played
	/// Nothing here except a variable.
	/// Anton Thorsell
	/// </summary>

	public FMOD_StudioEventEmitter[] derp;

	public float m_Insidevalue = 0f;
	public float m_Outstidevalue = 0f;
	public bool m_ReturnOutsideValue = false;
	public bool m_FadeSound = true;

	public string m_parametername = "";

	void Start () {
	
	}
}
