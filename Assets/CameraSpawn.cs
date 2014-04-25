using UnityEngine;
using System.Collections;

public class CameraSpawn : MonoBehaviour {

	Transform r_CameraRig;
	
	void Start(){
		Initialize ();
		DontDestroyOnLoad (gameObject);
	}
	
	void OnLevelWasLoaded(){
		Initialize ();
	}

	void Update(){
		transform.position = r_CameraRig.position;
		transform.rotation = r_CameraRig.rotation;
	}

	private void Initialize(){

		r_CameraRig = GameObject.Find("Third Person Camera Rig").transform;

		r_CameraRig.position = transform.position;
		r_CameraRig.rotation = transform.rotation;
	}
}
