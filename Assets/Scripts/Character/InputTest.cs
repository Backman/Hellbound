using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Axis value: "+Input.GetAxis("RunTrigger"));
	}
}
