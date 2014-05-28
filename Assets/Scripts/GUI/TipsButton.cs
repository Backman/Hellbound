using UnityEngine;
using System.Collections;
//anton
public class TipsButton : MonoBehaviour {

	/// <summary>
	/// TipsButton was written by Anton Thorsell
	/// 
	/// All this script does is show the mainlabel by default
	/// and show m_Tips when OnClick is called (when the button is pressed)
	/// 
	/// And whenever the gameobject is disabled (when it or a parent is disabled)
	/// it will revert the label back to the default value
	/// </summary>

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
