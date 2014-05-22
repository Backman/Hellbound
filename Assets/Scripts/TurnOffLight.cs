using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOffLight : MonoBehaviour {
	public List<Light> m_Lights = new List<Light>();
	public string m_ColliderTag = "Player";
	public float m_TurnOffSpeed = 0.3f;
	public float m_Delay = 0.0f;
	private bool m_Triggered = false;
	
	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag){
			if(m_Lights.Count > 0 && !m_Triggered){
				StartCoroutine("turnOffLights");
				m_Triggered = true;
			}
		}
	}

    IEnumerator turnOffLights() {
		yield return new WaitForSeconds(m_Delay);
		float t = m_TurnOffSpeed;
		while(t >= 0.0f){
			foreach(Light light in m_Lights){
				if( light != null ){
					light.intensity *= (t / m_TurnOffSpeed);
				}
			}
			t -= Time.deltaTime;
			yield return null;
		}
		foreach(Light light in m_Lights){
			if( light != null ){
				light.intensity = 0.0f;
			}
		}
	}
}
