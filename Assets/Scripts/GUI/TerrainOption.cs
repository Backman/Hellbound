using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIPopupList))]
public class TerrainOption : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void stuff(){
		switch(GetComponent<UIPopupList>().value){
		case "High":
			QualitySettings.lodBias = 3;
			break;
		case "Medium":
			QualitySettings.lodBias = 2;
			break;
		case "Low":
			QualitySettings.lodBias = 1;
			break;
		}
	}
}
