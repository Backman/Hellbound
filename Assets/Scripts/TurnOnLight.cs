using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOnLight : MonoBehaviour {
	public List<Light> m_Lights = new List<Light>();
	private float[] m_OrignalIntensity;
	public string m_ColliderTag = "Player";
	public float m_TurnOnSpeed = 0.3f;
	public float m_Delay = 0.0f;
	private bool m_Triggered = false;

	public void Start(){
		m_OrignalIntensity = new float[m_Lights.Count];
		for(int i = 0; i < m_Lights.Count; ++i){
			m_OrignalIntensity[i] = m_Lights[i].intensity;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag){
			if(m_Lights.Count > 0 && !m_Triggered){
				StartCoroutine("turnOnLights");
				m_Triggered = true;
			}
		}
	}
	
	IEnumerator turnOnLights(){
		yield return new WaitForSeconds(m_Delay);
		float t = 0.0f;
		while(t <= m_TurnOnSpeed){
			for(int i = 0; i < m_Lights.Count; ++i){
				m_Lights[i].intensity = (t / m_TurnOnSpeed) * m_OrignalIntensity[i];
			}
			t += Time.deltaTime;
			yield return null;
		}

		for(int i = 0; i < m_Lights.Count; ++i){
			m_Lights[i].intensity = m_OrignalIntensity[i];
		}
	}
}
