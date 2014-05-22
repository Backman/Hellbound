using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	private Light r_Light;
	private float m_pi = 0f;

	public float m_AverageLight = 1f;
	public float m_Speed = 1f;
	public float m_difference = 1f;


	void Start()
	{
		try{
			r_Light = GetComponent<Light>();
		}catch{
			Debug.LogWarning("No Light component attached to this object : " + gameObject);
		}
	}

	void Update () {
		UpdatePIValues ();

		r_Light.intensity = (Mathf.Sin(m_pi)*m_difference) + m_AverageLight;
	}

	private void UpdatePIValues(){

		m_pi += 0.01f*m_Speed;

		if(m_pi > Mathf.PI*2f)
		{
			m_pi = 0f;
		}
	}
}