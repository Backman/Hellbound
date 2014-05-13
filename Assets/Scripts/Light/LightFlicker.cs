using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	private Light r_Light;
	private float m_MPi;
	private float m_SPi;

	public float mainFloat = 0.08f;
	public float secondaryFloat = 0.08f;


	void Start () {
		r_Light = GetComponent<Light> ();
	}


	void Update () {


		float x = getMainPI ();
		float y = getSecondaryPI ();


		if(x >= y){
			r_Light.intensity = x;
		}
		else{
			r_Light.intensity = y;
		}
	}

	private float getMainPI()
	{
		float ret = 0f;
		
		if (m_MPi >= Mathf.PI) {
			m_MPi = 0f;
		}
		m_MPi += mainFloat;
		ret = Mathf.Sin(m_MPi);

		return ret;
	}

	private float getSecondaryPI()
	{
		float ret = 0f;
		
		if (m_SPi >= Mathf.PI) {
			m_SPi = 0f;
		}
		m_SPi += secondaryFloat;
		ret = Mathf.Sin(m_SPi) + 0.2f;
		
		return ret;
	}
}
