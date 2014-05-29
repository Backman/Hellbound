using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(UIPopupList))]
public class ShadowOption : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void stuff(){
		switch(GetComponent<UIPopupList>().value){
			case "On":
			//QualitySettings.sha
			break;
			case "Off":

			break;
		}
	}
}
