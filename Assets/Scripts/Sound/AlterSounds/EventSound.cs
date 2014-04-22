using UnityEngine;
using System.Collections;

public class EventSound : MonoBehaviour {

	public string m_PathPickUp = "event:/";
	public bool m_PickUp = false;
	
	public string m_PathExamine = "event:/";
	public bool m_Examine = false;
	
	public string m_PathActivate = "event:/";
	public bool m_Activate = false;
	
	public string m_PathGainFocus = "event:/";
	public bool m_GainFocus = false;
	
	public string m_PathLoseFocus = "event:/";
	public bool m_LoseFocus = false;
}