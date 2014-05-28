using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIScrollBar))]
public class VolumeBar : MonoBehaviour {

	public string VolumeName = "";

	// Use this for initialization
	void Start () {
		GetComponent<UIScrollBar>().value = SoundControl.getInstance().GetVolume (VolumeName);
	}

	// Update is called once per frame
	void Update () {
		SoundControl.getInstance().ChangeVolume (GetComponent<UIScrollBar>().value, VolumeName);
	}
}