using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundControl {
	/// <summary>
	/// Volumecontrol handles the masterVolume and also the groupVolume (ie "Music", "Voice" and "SFX")
	/// Volumecontrol is a singleton, meaning that only one can exist in the scene at any one time
	/// 
	/// Anton Thorsell
	/// </summary>

	//The current instance is saved in the variable below
	private static readonly SoundControl m_instance = new SoundControl();
	public static SoundControl Instance { get { return m_instance; } }

	public AudioSpeakerMode m_CurrentSpeakerMode;

	[Range(0f,1f)]
	public float Master;

	[Range(0f,1f)]
	public float VO;

	[Range(0f,1f)]
	public float SFX;
	
	[Range(0f,1f)]
	public float Music;

	//we make the constructor private so only Volumecontrol can create a Volumecontrol
	private SoundControl()
	{

	}

	// this variable will contain the volume controllers for the various groups of soundobjects
	Dictionary<string, FMOD.Studio.MixerStrip> m_Volume = new Dictionary<string, FMOD.Studio.MixerStrip>();

	// Start will create volume controllers and connect it with its id or "key"
	void Start ()
	{
		FMOD.Studio.System system = FMOD_StudioSystem.instance.System;

		FMOD.GUID guid1;
		FMOD.Studio.MixerStrip bus1;

		system.lookupID("bus:/", out guid1);
		system.getMixerStrip (guid1, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus1);
		m_Volume.Add ("Master", bus1);
		
		FMOD.GUID guid2;
		FMOD.Studio.MixerStrip bus2;

		system.lookupID ("vca:/VCA_SFX", out guid2);
		system.getMixerStrip (guid2, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus2);
		m_Volume.Add ("SFX", bus2);
		
		FMOD.GUID guid3;
		FMOD.Studio.MixerStrip bus3;

		system.lookupID ("vca:/VCA_VO", out guid3);
		system.getMixerStrip (guid3, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus3);
		m_Volume.Add ("Voice", bus3);
		
		FMOD.GUID guid4;
		FMOD.Studio.MixerStrip bus4;

		system.lookupID ("vca:/VCA_Music", out guid4);
		system.getMixerStrip (guid4, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus4);
		m_Volume.Add ("Music", bus4);

		LoadVolume();
		checkIfCorrect (false);
	}


	//this is where we change the volume for a specified group.
	//there is no need to find every soundobject and change it's volume
	//since fmod already knows about every sound that exists in the scene
	public void ChangeVolume(float newVolume, string tagToBeChanged)
	{
		m_Volume[tagToBeChanged].setFaderLevel (newVolume);
		SaveVolume ();
	}


	//public void Update(){
	//	if(m_CurrentSpeakerMode != AudioSettings.speakerMode){
	//		SetNewAduioSpeakerMode(m_CurrentSpeakerMode);
	//	}
	//	checkIfCorrect (true);
	//}


	private void LoadVolume(){
		ChangeVolume (PlayerPrefs.GetFloat ("Master", 1f), "Master");
		ChangeVolume (PlayerPrefs.GetFloat ("SFX", 1f), "SFX");
		ChangeVolume (PlayerPrefs.GetFloat ("Voice", 1f), "Voice");
		ChangeVolume (PlayerPrefs.GetFloat ("Music", 1f), "Music");
	}


	private void SaveVolume(){
		float setThis = 0f;
		
		m_Volume ["Master"].getFaderLevel(out setThis);
		PlayerPrefs.SetFloat ("Master", setThis);
		m_Volume ["SFX"].getFaderLevel(out setThis);
		PlayerPrefs.SetFloat ("SFX", setThis);
		m_Volume ["Voice"].getFaderLevel(out setThis);
		PlayerPrefs.SetFloat ("Voice", setThis);
		m_Volume ["Music"].getFaderLevel(out setThis);
		PlayerPrefs.SetFloat ("Music", setThis);

	}


	public void SetNewAduioSpeakerMode(AudioSpeakerMode toThis)
	{
		AudioSettings.speakerMode = toThis;
		m_CurrentSpeakerMode = AudioSettings.speakerMode;
	}


	public void UpdateAudioSpeakerMode()
	{
		AudioSettings.speakerMode = m_CurrentSpeakerMode;
	}


	public void checkIfCorrect(bool publicToVolume)
	{
		if (publicToVolume) 
		{
			ChangeVolume (Master, "Master");
			ChangeVolume (VO, "Voice");
			ChangeVolume (SFX, "SFX");
			ChangeVolume (Music, "Music");
		}
		else
		{
			m_Volume["Master"].getFaderLevel(out Master);
			m_Volume["Voice"].getFaderLevel(out VO);
			m_Volume["SFX"].getFaderLevel(out SFX);
			m_Volume["Music"].getFaderLevel(out Music);
		}
	}
}
