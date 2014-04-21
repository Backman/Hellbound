using UnityEngine;
using System.Collections;

public class TurnOffLight : MonoBehaviour {
	public Light[] m_Lights;
	public string m_ColliderTag = "Player";
	public float m_TurnOffSpeed = 0.3f;
	public float m_Delay = 0.0f;
	private bool m_Triggered = false;
	
	void OnTriggerEnter(Collider other){
		if(other.tag == m_ColliderTag){
			if(m_Lights.Length > 0 && !m_Triggered){
				StartCoroutine("sturnOffLights");
				m_Triggered = true;
			}
		}
	}

	IEnumerator sturnOffLights(){
		yield return new WaitForSeconds(m_Delay);
		float t = m_TurnOffSpeed;
		while(t >= 0.0f){
			foreach(Light light in m_Lights){
				light.intensity *= (t / m_TurnOffSpeed);
			}
			t -= Time.deltaTime;
			yield return null;
		}
		foreach(Light light in m_Lights){
			light.intensity = 0.0f;
		}
	}
}
