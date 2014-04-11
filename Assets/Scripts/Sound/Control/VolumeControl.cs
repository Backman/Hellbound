using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VolumeControl : MonoBehaviour {
	/// <summary>
	/// Volumecontrol handles the masterVolume and also the groupVolume (ie "Music", "Voice" and "SFX")
	/// Volumecontrol is a singleton, meaning that only one can exist in the scene at any one time
	/// 
	/// Anton Thorsell
	/// </summary>

	//The current instance is saved in the variable below
	private static VolumeControl m_instance;

	//we make the constructor private so only Volumecontrol can create a Volumecontrol
	private VolumeControl()
	{

	}

	//This is the only way to create volumecontrol outside the Volumecontrol script
	//if one have already been created we return that one.
	public static VolumeControl getInstance()
	{
		if (!m_instance) 
		{
			GameObject container;
			container = new GameObject();
			container.name = "VolumeControl";
			m_instance = container.AddComponent(typeof(VolumeControl)) as VolumeControl;
			DontDestroyOnLoad(m_instance);
		}
		return m_instance;
	}

	// this variable will contain the volume controllers for the various groups of soundobjects
	Dictionary<string, FMOD.Studio.MixerStrip> m_Volume = new Dictionary<string, FMOD.Studio.MixerStrip>();

	// Start will create volume controllers and connect it with its id or "key"
	void Start () 
	{
		FMOD.GUID guid;
		FMOD.Studio.System system = FMOD_StudioSystem.instance.System;

		FMOD.Studio.MixerStrip bus;

		system.lookupID("bus:/", out guid);
		system.getMixerStrip (guid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus);
		m_Volume.Add ("Master", bus);

		system.lookupID ("bus:/SFX", out guid);
		system.getMixerStrip (guid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus);
		m_Volume.Add ("SFX", bus);

		system.lookupID ("bus:/Voice", out guid);
		system.getMixerStrip (guid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus);
		m_Volume.Add ("Voice", bus);

		system.lookupID ("bus:/Music", out guid);
		system.getMixerStrip (guid, FMOD.Studio.LOADING_MODE.BEGIN_NOW, out bus);
		m_Volume.Add ("Music", bus);
	}


	//this is where we change the volume for a specified group.
	//there is no need to find every soundobject and change it's volume
	//since fmod already knows about every sound that exists in the scene
	public void ChangeVolume(float newVolume, string tagToBeChanged)
	{
		m_Volume[tagToBeChanged].setFaderLevel (newVolume);
	}
}
