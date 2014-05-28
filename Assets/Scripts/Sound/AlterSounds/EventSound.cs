using UnityEngine;
using System.Collections;

public class EventSound : MonoBehaviour {

	/// <summary>
	/// EventSound was written by Anton Thorsell
	/// 
	/// This "Script" is actually just a collection of variables that can (dont dont have to)
	/// be attached to interactable objects
	/// 
	/// The Idea with EventSound is that it will be attached to interactable objects
	/// and with the help of the bool variables it can figure out what sounds
	/// that should be played (the strings) when specific functions are called.
	/// 
	/// (see the script Interactable where the logic is) 
	/// 
	/// </summary>

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