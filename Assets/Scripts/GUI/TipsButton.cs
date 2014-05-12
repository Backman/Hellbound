using UnityEngine;
using System.Collections;

public class TipsButton : MonoBehaviour {

	private UILabel r_Label;

	// Use this for initialization
	void Start () {
		r_Label = GetComponent<UILabel> ();
		r_Label.enabled = false;
	}

	public void OnClick(){
		r_Label.enabled = true;
	}

	public void OnDisable(){
		r_Label.enabled = false;
	}
}
