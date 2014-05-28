using UnityEngine;
using System.Collections;

public class StartSoundControl : MonoBehaviour {

	[Range(0f,1f)]
	public float Master;

	// Use this for initialization
	void Start () {
		SoundControl.GetInstance().checkIfCorrect(true);
	}
	
	// Update is called once per frame
	void Update () {
		SoundControl.GetInstance().ChangeVolume(Master, "Master");
	}
}
