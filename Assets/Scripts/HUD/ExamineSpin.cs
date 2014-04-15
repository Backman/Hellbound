using UnityEngine;
using System.Collections;

public class ExamineSpin : MonoBehaviour {
	
	[SerializeField] float m_RotationSpeed = 100.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(new Vector3(0.0f, m_RotationSpeed * Time.deltaTime, 0.0f));
	}
}
