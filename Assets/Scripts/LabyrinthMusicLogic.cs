using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LabyrinthMusicLogic : MonoBehaviour {

	public List<Interactable> m_NeededInteractables = new List<Interactable>();
	public string m_StateTheyNeed;
	public FMOD_StudioEventEmitter m_MusicEmitter;
	public string m_Parameter;
	public float m_ParameterValue = 0.0f;
	private FMOD.Studio.ParameterInstance m_ParameterInstance;

	void Start() {
		if(m_MusicEmitter != null) {
			m_ParameterInstance = m_MusicEmitter.getParameter(m_Parameter);
		}
	}

	void OnTriggerEnter(Collider other) {
		bool stopMusic = true;
		foreach(Interactable inter in m_NeededInteractables) {
			if(inter.getPuzzleState() != m_StateTheyNeed) {
				stopMusic = false;
				break;
			}
		}
		if(stopMusic) {
			m_ParameterInstance.setValue(m_ParameterValue);
		}
	}
}
