using UnityEngine;
using System.Collections;

public class FpsLimiter : MonoBehaviour {
	public int m_TargetFrame = 60;
	// Use this for initialization
	void Start () {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = m_TargetFrame;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_TargetFrame != Application.targetFrameRate) {
			Application.targetFrameRate = m_TargetFrame;
		}
	}
}
