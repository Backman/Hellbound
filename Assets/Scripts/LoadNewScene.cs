using UnityEngine;
using System.Collections;

public class LoadNewScene : MonoBehaviour {

	/// <summary>
	/// MessengerList will contain all calls that have been made and
	/// send them out in an orderly fasion (to avoid spikes)
	/// </summary>
	public string m_NewScene = "";


	void OnTriggerEnter(Collider other){
		Application.LoadLevel(m_NewScene);
	}
}
