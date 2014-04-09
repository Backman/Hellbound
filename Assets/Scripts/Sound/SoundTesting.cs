using UnityEngine;
using System.Collections;

public class SoundTesting : MonoBehaviour {


	public float volume = 0.5f;

	public string Tag = "Master";

	private VolumeControl m_VC;

	void Start()
	{
		m_VC = VolumeControl.getInstance ();
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
