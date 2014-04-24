using UnityEngine;
using System.Collections;

public class CameraSpawn : MonoBehaviour {

	Transform r_CameraRig;
	
	void Start(){
		
		Initialize ();
		
		DontDestroyOnLoad (gameObject);
	}
	
	void Update(){
		transform.position = r_CameraRig.position;
		transform.rotation = r_CameraRig.rotation;
	}
	
	void OnLevelWasLoaded(int level){
		Initialize ();
	}

	private void Initialize(){
		r_CameraRig = GameObject.Find("Third Person Camera Rig").transform;
		
		r_CameraRig.position = gameObject.transform.position;
		r_CameraRig.rotation = gameObject.transform.rotation;
	}
}
