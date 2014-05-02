using UnityEngine;
using System.Collections;

public class Roof : MonoBehaviour {

	public float m_LowerAmount = 1.0f;

	// Use this for initialization
	void Start () {
		Messenger.AddListener ("lower roof", lowerRoof );
	}

	public void lowerRoof(){
		gameObject.transform.Translate (new Vector3 (0.0f, -m_LowerAmount, 0.0f));
	}
}
