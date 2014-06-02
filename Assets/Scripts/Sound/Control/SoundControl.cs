using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundControl : MonoBehaviour {
	/// <summary>
	/// SoundControl handles the masterVolume and also the groupVolume (ie "Music", "Voice" and "SFX")
	/// SoundControl is a singleton, meaning that only one can exist in the scene at any one time
	/// although only if this script is used responsibly (you can make serveral of theese)
	/// 
	/// SoundControl can also alter the AudioSpeakerMode (mono, stereo, 5.1, etc).
	/// 
	/// Anton Thorsell
	/// </summary>

	private static SoundControl Instance = null;

	private static Dictionary<string, float> s_Volume = new Dictionary<string, float>();

	public AudioSpeakerMode m_CurrentSpeakerMode;

	//we make the constructor private so only Volumecontrol can create a Volumecontrol
	private SoundControl()
	{
		FMOD.Studio.System system = FMOD_StudioSystem.instance.System;
		
		FMOD.GUID guid1;
		FMOD.Studio.MixerStrip bus1;
		FMOD.RESULT res;
		
		system.lookupID("bus:/", out guid1);
		res = system.getMixerStrip (guid1, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus1);
		m_Volume.Add ("Master", bus1);
		
		FMOD.GUID guid2;
		FMOD.Studio.MixerStrip bus2;
		
		system.lookupID ("vca:/VCA_SFX", out guid2);
		res = system.getMixerStrip (guid2, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus2);
		m_Volume.Add ("SFX", bus2);
		
		FMOD.GUID guid3;
		FMOD.Studio.MixerStrip bus3;
		system.lookupID ("vca:/VCA_VO", out guid3);
		res = system.getMixerStrip (guid3, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus3);
		m_Volume.Add ("Voice", bus3);
		
		FMOD.GUID guid4;
		FMOD.Studio.MixerStrip bus4;
		
		system.lookupID ("vca:/VCA_Music", out guid4);
		res = system.getMixerStrip (guid4, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus4);
		m_Volume.Add ("Music", bus4);

		if(!s_Volume.ContainsKey("Master")){
			s_Volume.Add("Master", 1.0f);
		}
		if(!s_Volume.ContainsKey("SFX")){
			s_Volume.Add("SFX", 1.0f);
		}
		if(!s_Volume.ContainsKey("Voice")){
			s_Volume.Add("Voice", 1.0f);
		}
		if(!s_Volume.ContainsKey("Music")){
			s_Volume.Add("Music", 1.0f);
		}
		
		LoadVolume();
	}

	public static SoundControl getInstance()
	{
		if (!Instance)
		{
			GameObject container;
			container = new GameObject();
			container.name = "SoundControl";
			Instance = container.AddComponent(typeof(SoundControl)) as SoundControl;
		}
		return Instance;
	}


	// this variable will contain the volume controllers for the various groups of soundobjects
	Dictionary<string, FMOD.Studio.MixerStrip> m_Volume = new Dictionary<string, FMOD.Studio.MixerStrip>();


	//this is where we change the volume for a specified group.
	//there is no need to find every soundobject and change it's volume
	//since fmod already knows about every sound that exists in the scene
	public void ChangeVolume(float newVolume, string tagToBeChanged)
	{
		FMOD.Studio.MixerStrip mixerStrip = null;
		if(m_Volume.TryGetValue(tagToBeChanged, out mixerStrip)){
			if(mixerStrip != null) {
				Debug.Log("newVolume on: " + tagToBeChanged + ", volume: " + newVolume);
				s_Volume[tagToBeChanged] = newVolume;
				mixerStrip.setFaderLevel(newVolume);
			}
		}
		SaveVolume ();
	}

	public float GetVolume (string tag)
	{
		float volume = 1f;
		if (m_Volume.ContainsKey (tag)) {
			volume = s_Volume[tag];
			//m_Volume [tag].getFaderLevel (out volume);
			Debug.Log("Volume: " + volume);
		}
		else{
			Debug.LogWarning("GetVolume: Tag missing");
		}
		return volume;
	}


	private void LoadVolume(){
		ChangeVolume(s_Volume["Master"], "Master");
		ChangeVolume(s_Volume["SFX"], "SFX");
		ChangeVolume(s_Volume["Voice"], "Voice");
		ChangeVolume(s_Volume["Music"], "Music");
		/*
		ChangeVolume (PlayerPrefs.GetFloat ("Master", m_MasterVolume), "Master");
		ChangeVolume (PlayerPrefs.GetFloat ("SFX", m_SFXVolume), "SFX");
		ChangeVolume (PlayerPrefs.GetFloat ("Voice", m_VoiceVolume), "Voice");
		ChangeVolume (PlayerPrefs.GetFloat ("Music", m_MusicVolume), "Music");
		*/
	}


	private void SaveVolume(){
		/*
		float setThis = 0f;
		m_Volume ["Master"].getFaderLevel(out setThis);
		PlayerPrefs.SetFloat ("Master", setThis);

		float setThis1 = 0f;
		m_Volume ["SFX"].getFaderLevel(out setThis1);
		PlayerPrefs.SetFloat ("SFX", setThis1);

		float setThis2 = 0f;
		m_Volume ["Voice"].getFaderLevel(out setThis2);
		PlayerPrefs.SetFloat ("Voice", setThis2);

		float setThis3 = 0f;
		m_Volume ["Music"].getFaderLevel(out setThis3);
		PlayerPrefs.SetFloat ("Music", setThis3);
		*/
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
}
