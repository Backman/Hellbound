using UnityEngine;
using System.Collections;

public class RopeSound : MonoBehaviour {

	private FMOD.Studio.EventInstance m_event;
	private FMOD.Studio.ParameterInstance m_parameter;

	private Rigidbody m_body;
	private Vector3 acceleration = new Vector3();
	private Vector3 lastVelocity = new Vector3();

	// i am in need of that goddamn rope sound
	void Start (){
		m_body = gameObject.GetComponent<Rigidbody> ();
		m_event = FMOD_StudioSystem.instance.GetEvent ("event:/SFX/Static_Emitters/Hanged_Man");
		m_event.start ();
		m_event.getParameter ("Speed", out m_parameter);
	}
	
	// Update is called once per frame
	void Update (){
		acceleration = m_body.rigidbody.velocity;

		//acceleration = (m_body.rigidbody.velocity - lastVelocity) / Time.fixedDeltaTime;
		//lastVelocity = rigidbody.rigidbody.velocity;

		Debug.Log (getLargestAcceleration(acceleration));
		m_parameter.setValue(getLargestAcceleration(acceleration));
	}


	private float getLargestAcceleration(Vector3 vec3){
		float ret = 0f;
		float x = System.Math.Abs (vec3.x);
		float y = System.Math.Abs (vec3.y);

		if(x >= y){
			ret = x;
		}
		else{
			ret = y;
		}

		//400f beacuse i expect the number will be between 0 and 40 and i
		//also want the result to be somewhere between 0 and 10

		ret = (ret * 100f) / 10f;

		return ret;
	}

	void OnDisable()
	{
		m_event.stop ();
		m_event.release ();
	}
}
