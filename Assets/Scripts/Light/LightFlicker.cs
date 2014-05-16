using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	private Light r_Light;
	private float[] m_Pi = new float[5];
	public float movementIntensity = 1f;

	void Start () {
		r_Light = GetComponent<Light> ();


		m_Pi [0] = 0f;
		m_Pi [1] = 0f;
		m_Pi [2] = 0f;
		m_Pi [3] = 0f;
		m_Pi [4] = 0f;

	}


	void Update () {

		float amount = (Mathf.Sin(m_Pi[0]) / 4f) + (Mathf.Sin (m_Pi[1]) / 4f) + 1f;

		float z = Mathf.Cos(m_Pi[3]);
		float x = Mathf.Sin(m_Pi[2]);
		Vector3 newVec = new Vector3(x/amount, (amount*movementIntensity)+1f, z/amount);
		transform.localPosition = newVec;

		r_Light.intensity = amount;

		UpdatePIValues();
	}

	private void UpdatePIValues()
	{
		for(int i = 0; i < m_Pi.Length; i++) {
			float rand = Random.Range(0.01f, 0.1f);
			m_Pi[i] += rand;

			if(m_Pi[i] >= Mathf.PI*2f)
			{
				m_Pi[i] = 0f;
			}
		}
	}
}