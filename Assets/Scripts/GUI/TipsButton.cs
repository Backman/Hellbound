using UnityEngine;
using System.Collections;

public class TipsButton : MonoBehaviour {



	// Use this for initialization
	void Start () {
		transform.GetChild (0).gameObject.SetActive(false);
	}


	public void OnClick(){
		transform.GetChild (0).gameObject.SetActive(true);
	}

	public void OnDisable(){
		transform.GetChild (0).gameObject.SetActive(false);
	}
}
