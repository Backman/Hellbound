using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceHandler : MonoBehaviour {

	private static VoiceHandler m_instance;

	private List<FMOD_StudioEventEmitter> r_EmitterList = new List<FMOD_StudioEventEmitter>();

	private VoiceHandler()
	{
		
	}

	public static VoiceHandler getInstance()
	{
		if (!m_instance) 
		{
			GameObject container;
			container = new GameObject();
			container.name = "VoiceHandler";
			m_instance = container.AddComponent(typeof(VoiceHandler)) as VoiceHandler;
			DontDestroyOnLoad(m_instance);
		}
		return m_instance;
	}


	//we will assume that "target" is valid (since we have checked this numerous
	//before this point)
	public void addVoice(string path, GameObject target)
	{
		Debug.Log (path);
		Debug.Log (target);
		if(target.GetComponent<FMOD_StudioEventEmitter>() != null)
		{
			FMOD_StudioEventEmitter SEE = target.GetComponent<FMOD_StudioEventEmitter>();

			SEE.Stop();

			SEE.path = path;

			SEE.Play();
			r_EmitterList.Add(SEE);

		}
		else
		{
			FMOD_StudioEventEmitter SEE = target.AddComponent<FMOD_StudioEventEmitter>();

			SEE.path = path; //here
			
			SEE.Play();
			r_EmitterList.Add(SEE);
		}

		if (r_EmitterList.Count <= 1) {
			StartCoroutine ("CheckIfDone");
		}
	}

	private IEnumerator CheckIfDone()
	{
		List<FMOD_StudioEventEmitter> destroyThese = new List<FMOD_StudioEventEmitter>();

		while(r_EmitterList.Count != 0)
		{
			foreach(FMOD_StudioEventEmitter see in r_EmitterList)
			{

				if(see.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.SUSTAINING)//here
				{
					see.Stop();
					destroyThese.Add(see);
				}
			}
			while(destroyThese.Count != 0)
			{
				r_EmitterList.Remove(destroyThese[0]);
				Destroy(destroyThese[0]);
			}
			yield return null;
		}
		yield return 0;
	}

}
