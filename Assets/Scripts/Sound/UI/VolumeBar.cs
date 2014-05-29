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
		/*
		GetComponent<UIScrollBar> ().value = 1f;
		//Debug.Log(SoundControl.getInstance().GetVolume (VolumeName));
		//GetComponent<UIScrollBar>().value = SoundControl.getInstance().GetVolume (VolumeName);
		number = GetComponent<UIScrollBar> ().value;
		if(VolumeName == "Master")AudioListener.volume = GetComponent<UIScrollBar> ().value;
		cond = true;
		*/
	}

	// Update is called once per frame
	void Update () {
	}

	public void onChange(){
		/*
		if (cond){
			Debug.Log("onChange!");
			SoundControl.getInstance().ChangeVolume (GetComponent<UIScrollBar>().value, VolumeName);
			number = GetComponent<UIScrollBar> ().value;
			if(VolumeName == "Master")AudioListener.volume = GetComponent<UIScrollBar> ().value;
		}
		*/
	}
}