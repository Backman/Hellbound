using UnityEngine;
using System.Collections;

public class HangedMan : MonoBehaviour {
	/// <summary>
	/// Anton
	/// </summary>

	private FMOD.Studio.ParameterInstance r_parameter;

	private float m_pi = 0f;
	public float m_ChangeValue = 1f;

	public bool m_Once = false;

	void Start()
	{
		r_parameter = GetComponent<FMOD_StudioEventEmitter> ().getParameter("Speed");
	}

	void Update()
	{
		m_pi += 0.0045f;
		if (m_pi > Mathf.PI * 2f) {
			m_pi = 0f;
		}

		if (m_pi >= Mathf.PI-0.005f && m_pi <= Mathf.PI+0.005f && !m_Once) {
			m_Once = true;
			r_parameter.setValue(0.7f);
			Debug.Log("PLAY YOU MOFO");
		}
		if (m_pi >= (Mathf.PI*0.8f)-0.005f && m_pi <= (Mathf.PI*0.8f)+0.005f && m_Once) {
			m_Once = false;
			r_parameter.setValue(0.0f);
		}

		if (m_pi >= (Mathf.PI*2f)-0.005f && m_pi <= (Mathf.PI*2f)+0.005f && !m_Once) {
			m_Once = true;
			r_parameter.setValue(0.7f);
			Debug.Log("PLAY YOU MOFO");
		}
		if (m_pi >= (Mathf.PI*1.8f)-0.005f && m_pi <= (Mathf.PI*1.8f)+0.005f && m_Once) {
			m_Once = false;
			r_parameter.setValue(0.0f);
		}

		Quaternion newValues = gameObject.transform.localRotation;
		newValues.x = Mathf.Cos(m_pi)*(0.01f);
		gameObject.transform.localRotation = newValues;
	}
}
