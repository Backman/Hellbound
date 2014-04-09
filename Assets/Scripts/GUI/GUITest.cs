using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {
	[Multiline]
	public string m_MyString = "Enter text here";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetButtonDown("Fire2") ){
			GUIManager.Instance.simpleShowText(m_MyString);
		}
	}
}
