using UnityEngine;
using System.Collections;
//peter
[RequireComponent(typeof(UIScrollBar))]
public class VolumeBar : MonoBehaviour {

	public string VolumeName = "";
	public float number;
	bool cond = false;
	// Use this for initialization
	void Awake () {
		GetComponent<UIScrollBar>().value = SoundControl.getInstance().GetVolume (VolumeName);
		if(VolumeName == "Master")AudioListener.volume = GetComponent<UIScrollBar> ().value;
		cond = true;
	}

	// Update is called once per frame
	void Update () {
	}

	public void onChange(){
		if (cond){
			SoundControl.getInstance().ChangeVolume (GetComponent<UIScrollBar>().value, VolumeName);
			if(VolumeName == "Master")AudioListener.volume = GetComponent<UIScrollBar> ().value;

			number = GetComponent<UIScrollBar> ().value;
		}
	}
}