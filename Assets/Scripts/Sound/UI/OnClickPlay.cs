using UnityEngine;
using System.Collections;

public class OnClickPlay : MonoBehaviour {

	/// <summary>
	/// This needs to be placed on buttons
	/// This is a REALLY small script, basically it plays a sound when the event "OnClick" happens
	/// Anton Thorsell
	/// </summary>

	public string m_Path = "event:/";
	
	void OnClick()
	{
		FMOD_StudioSystem.instance.PlayOneShot (m_Path, transform.position);
	}
}
