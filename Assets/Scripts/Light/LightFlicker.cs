using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

	private Light r_Light;

	private float pi = 0f;

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

		r_Light.intensity = (Mathf.Sin(pi)/2f) + 1f;
	}

	private void UpdatePIValues(){

		pi += Random.Range (0.01f, 0.1f);

		if(pi > Mathf.PI*2f)
		{
			pi = 0f;
		}
	}
}