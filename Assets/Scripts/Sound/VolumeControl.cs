using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VolumeControl : MonoBehaviour {

	private static VolumeControl m_instance;

	private VolumeControl()
	{

	}

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

	Dictionary<string, FMOD.Studio.MixerStrip> m_Volume = new Dictionary<string, FMOD.Studio.MixerStrip>();

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

	public void ChangeVolume(float newVolume, string tagToBeChanged)
	{
		m_Volume[tagToBeChanged].setFaderLevel (newVolume);
	}
}
