using UnityEngine;
using System.Collections;

public class SoundTesting : MonoBehaviour {
	/// <summary>
	/// This script will change numerous times, no need to explain anything.
	/// variable and function names should still be understandable and logical.
	/// 
	/// Anton Thorsell
	/// </summary>

	public float volume = 0.5f;

	public string Tag = "Master";

	private SoundControl m_VC;

	void Start()
	{
		m_VC = SoundControl.GetInstance();
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.O))
		{
			m_VC.ChangeVolume(volume, Tag);
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			m_VC.ChangeVolume(0f, "Master");
		}
	}
}
