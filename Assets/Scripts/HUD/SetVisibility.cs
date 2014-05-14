using UnityEngine;
using System.Collections;
//peter
//used to set visibility after initiate
//can be used if one wants to see the panel in editor but not in runtime
public class SetVisibility : MonoBehaviour {
	public bool Visible = false;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<UIPanel> ().alpha = Visible? 1:0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
