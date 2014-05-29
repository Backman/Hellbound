using UnityEngine;
using System.Collections;
//peter
[RequireComponent(typeof(UIScrollBar))]
public class VolumeBar : MonoBehaviour {

	public string VolumeName = "";

	// Use this for initialization
	void Start () {
		GetComponent<UIScrollBar>().value = SoundControl.getInstance().GetVolume (VolumeName);
		if(VolumeName == "Master")AudioListener.volume = GetComponent<UIScrollBar> ().value;
	}

	// Update is called once per frame
	void Update () {
		SoundControl.getInstance().ChangeVolume (GetComponent<UIScrollBar>().value, VolumeName);
		if(VolumeName == "Master")AudioListener.volume = GetComponent<UIScrollBar> ().value;
	}
}