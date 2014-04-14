using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressText : MonoBehaviour {

	private string m_FullText;
	private UILabel m_Label;
	private float m_CurrentProgress = 0.0f;
	private float m_ProgressDuration;
	private float m_ProgressStartTime;
	
	[SerializeField] float m_TextAnimationSpeed = 25.0f;
	
	// Use this for initialization
	void Start () {
		m_Label = gameObject.GetComponent<UILabel>();
		m_FullText = m_Label.text;
		m_ProgressStartTime = Time.time;
		m_ProgressDuration = m_FullText.Length / m_TextAnimationSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		//m_Label.text
		float timePassed = Time.time - m_ProgressStartTime;
		float progress = timePassed / m_ProgressDuration;
		progress = Mathf.Min(1.0f, progress);
		//Debug.Log ("Progress: "+progress);
		m_Label.text = m_FullText.Substring(0, (int)(progress * m_FullText.Length));
	}
	
	// Progress it reset when text is changed
	public void setText(string text){
		m_ProgressStartTime = Time.time;
		m_FullText = text;
		m_ProgressDuration = m_FullText.Length / m_TextAnimationSpeed;
	}
}
