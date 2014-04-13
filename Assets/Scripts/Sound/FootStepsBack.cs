using UnityEngine;
using System.Collections;

public class FootStepsBack : MonoBehaviour {

	public bool b_IsHitting = false;


	void OnTriggerEnter(Collider other)
	{
		b_IsHitting = true;
	}

	void OnTriggerExit(Collider other)
	{
		b_IsHitting = false;
	}
}
