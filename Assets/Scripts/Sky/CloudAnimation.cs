using UnityEngine;
using System.Collections;

public class CloudAnimation : MonoBehaviour {

	[SerializeField] float m_RotationSpeed = 100.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform camTransform = Camera.main.transform;
		gameObject.transform.localPosition = camTransform.localPosition;
		Vector3 newRot = new Vector3(0.0f, 0.0f, m_RotationSpeed * Time.deltaTime);
		gameObject.transform.Rotate(newRot);
	}
}
