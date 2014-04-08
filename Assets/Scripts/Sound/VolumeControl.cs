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

	private List<KeyValuePair<string, FMOD.Studio.ParameterInstance>> m_SoundObjects = new List<KeyValuePair<string, FMOD.Studio.ParameterInstance>>();

	private Dictionary<string, float> m_Volume = new Dictionary<string, float>();

	// Use this for initialization
	void Start () 
	{


		m_Volume.Add ("Master", 100f);
		m_Volume.Add ("SFX", 100f);
		m_Volume.Add ("Voice", 100f);
		m_Volume.Add ("Music", 100f);


		GameObject[] allSounds = GameObject.FindGameObjectsWithTag ("Sound");
		foreach(GameObject g in allSounds)
		{
			SoundObject testThis = g.GetComponent<SoundObject>();

			FMOD_StudioEventEmitter Emitter = testThis.FMOD_Emitter;

			if(testThis.s_Tag == "Master")
			{
				KeyValuePair<string, FMOD.Studio.ParameterInstance> saveThis = 
					new KeyValuePair<string, FMOD.Studio.ParameterInstance>("Master", Emitter.getParameter("Master"));
				m_SoundObjects.Add (saveThis);
			}

			if(testThis.s_Tag == "SFX")
			{
				KeyValuePair<string, FMOD.Studio.ParameterInstance> saveThis = 
					new KeyValuePair<string, FMOD.Studio.ParameterInstance>("SFX", Emitter.getParameter("SFX"));
				m_SoundObjects.Add (saveThis);			
			}

			if(testThis.s_Tag == "Voice")
			{
				KeyValuePair<string, FMOD.Studio.ParameterInstance> saveThis = 
					new KeyValuePair<string, FMOD.Studio.ParameterInstance>("Voice", Emitter.getParameter("Voice"));
				m_SoundObjects.Add (saveThis);				
			}
			
			if(testThis.s_Tag == "Music")
			{
				KeyValuePair<string, FMOD.Studio.ParameterInstance> saveThis = 
					new KeyValuePair<string, FMOD.Studio.ParameterInstance>("Music", Emitter.getParameter("Music"));
				m_SoundObjects.Add (saveThis);
			}
		}
	}

	void ChangeVolume(float newVolume, string tagToBeChanged)
	{

		m_Volume [tagToBeChanged] = newVolume;

		foreach(KeyValuePair<string, FMOD.Studio.ParameterInstance> g in m_SoundObjects)
		{
			if(g.Key == tagToBeChanged)
			{
				g.Value.setValue(newVolume);
			}
		}
	}
}
