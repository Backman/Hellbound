
using UnityEngine;
using System.Collections;

public class TipsButton : MonoBehaviour {
	
	private UILabel r_Label;
	
	[HideInInspector]
	public string m_MainLabel;
	
	[HideInInspector]
	public string m_Tips;
	
	// Use this for initialization
	void Start () {
		r_Label = GetComponent<UILabel> ();
		r_Label.text = m_MainLabel;
	}
	
	public void OnClick(){
		r_Label.text = m_Tips;
	}
	
	public void OnDisable(){
		r_Label.text = m_MainLabel;
	}
}
