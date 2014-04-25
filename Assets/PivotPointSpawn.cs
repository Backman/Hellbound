using UnityEngine;
using System.Collections;

public class PivotPointSpawn : MonoBehaviour {
	
	private Transform r_PivotPoint;
	private Transform r_virtualPosition;
	float Rx = 0f;
	
	void Start(){
		Initialize ();
		DontDestroyOnLoad (gameObject);
	}
	
	void OnLevelWasLoaded(){
		Initialize ();
	}
	
	void Update(){
		transform.rotation = r_PivotPoint.rotation;
		transform.position = r_PivotPoint.position;

		if (Input.GetKeyDown (KeyCode.K)) {
			r_PivotPoint.position = new Vector3 (0f, 1.72325f, 0f);
		}
	}
	
	private void Initialize(){
		Debug.Log ("Init");

		r_PivotPoint = GameObject.Find("Pivot Point").transform;

		r_PivotPoint.position = transform.position;
		r_PivotPoint.rotation = new Quaternion(transform.rotation.x,0f,0f,transform.rotation.z);

	}
}
