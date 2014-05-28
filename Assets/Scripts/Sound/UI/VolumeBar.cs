using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIScrollBar))]
public class VolumeBar : MonoBehaviour {

	public string VolumeName = "";

	// Use this for initialization
	void Start () {
		GetComponent<UIScrollBar>().value = SoundControl.GetInstance().GetVolume (VolumeName);
	}

	// Update is called once per frame
	void Update () {
		SoundControl.GetInstance().ChangeVolume (GetComponent<UIScrollBar>().value, VolumeName);
	}
}