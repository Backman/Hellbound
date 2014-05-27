using UnityEngine;
using System.Collections;

public class StartSoundControl : MonoBehaviour {

	[Range(0f,1f)]
	public float Master;

	// Use this for initialization
	void Start () {
		SoundControl.Instance.checkIfCorrect(true);
	}
	
	// Update is called once per frame
	void Update () {
		SoundControl.Instance.ChangeVolume(Master, "Master");
	}
}
