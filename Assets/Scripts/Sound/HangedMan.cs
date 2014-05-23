
using UnityEngine;
using System.Collections;

public class HangedMan : MonoBehaviour {

	/// <summary>
	/// HangedMan was written by Anton Thorsell
	/// 
	/// This script was created to make a gameobject that resembles a body
	/// "swing" and start a sound that resembles a rope at the apex of the "swings"
	/// 
	/// This is a super simple script
	/// 
	/// </summary>

	private FMOD_StudioEventEmitter r_Emitter;

	private float m_pi = 0f;
	public float m_Speed = 1f;

	public float m_SwingAmount = 2;
	public float m_Speed = 1f;

	private bool m_LOnce = false;
	private bool m_ROnce = false;

	void Start()
	{
		r_Emitter = GetComponent<FMOD_StudioEventEmitter> ();
	}

	void Update()
	{
<<<<<<< HEAD
		m_pi += 0.0045f * m_Speed;
=======
		m_pi += 0.0045f*m_Speed;
>>>>>>> d01ffeb9e9e8f5dbed252e1541a3f0fdc9c52461

		if (m_pi >= Mathf.PI-0.005f && !m_ROnce) {
			m_ROnce = true;
			m_LOnce = false;
			r_Emitter.Play();
		}

		if (m_pi >= (Mathf.PI*2f)-0.005f && !m_LOnce) {
			m_LOnce = true;
			m_ROnce = false;
			r_Emitter.Play();
			m_pi = 0f;
		}

		Quaternion newValues = gameObject.transform.localRotation;
		newValues.x = (Mathf.Cos(m_pi)*(m_SwingAmount/100f));
		gameObject.transform.localRotation = newValues;
	}
}
