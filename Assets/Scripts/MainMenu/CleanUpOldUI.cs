using UnityEngine;
using System.Collections;

public class CleanUpOldUI : MonoBehaviour {

	void Awake(){
		Screen.showCursor = true;
		Screen.lockCursor = false;
		GameObject obj = GameObject.FindGameObjectWithTag ("UIRoot");
		if (obj != null) {
			Debug.Log("Old UIRoot found. Queueing for destruction");
			GameObject.Destroy(obj);
		}
	}
}
