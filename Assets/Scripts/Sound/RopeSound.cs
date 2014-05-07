using UnityEngine;
using System.Collections;

public class RopeSound : MonoBehaviour {

	private FMOD.Studio.EventInstance m_event;
	private FMOD.Studio.ParameterInstance m_parameter;

	private Rigidbody m_body;
	private Vector3 acceleration = new Vector3();
	private Vector3 lastVelocity = new Vector3();

	// Use this for initialization
	void Start (){
		//m_event = FMOD_StudioSystem.instance.GetEvent ("");
		//m_event.start ();
		//m_event.getParameter ("", out m_parameter);
		m_body = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update (){
		acceleration = (m_body.rigidbody.velocity - lastVelocity) / Time.fixedDeltaTime;
		lastVelocity = rigidbody.rigidbody.velocity;

		Debug.Log (getLargestAcceleration(acceleration));
	}



	private float getLargestAcceleration(Vector3 vec3){
		float ret = 0f;
		if(vec3.x >= vec3.y){
			ret = vec3.x;
		}
		else{
			ret = vec3.y;
		}

		//400f beacuse i expect the number will be between 0 and 40 and i
		//also want the result to be somewhere between 0 and 10

		ret = (ret * 100f) / 400f;

		return ret;
	}
}
