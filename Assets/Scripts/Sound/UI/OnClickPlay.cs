using UnityEngine;
using System.Collections;
//anton
public class OnClickPlay : MonoBehaviour {
	/// <summary>
	/// This can be placed on buttons
	/// This is a REALLY small script, basically it plays a sound when the event "OnClick" happens
	/// Anton Thorsell
	/// </summary>

	public string m_Path = "event:/";
	
	void OnClick()
	{
		var reff = FMOD_StudioSystem.instance; 
		reff.PlayOneShot (m_Path, Camera.main.transform.position);
	}
}
