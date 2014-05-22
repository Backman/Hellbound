using UnityEngine;
using System.Collections;

public class CleanUpOldUI : MonoBehaviour {

	void Awake(){
		GameObject obj = GameObject.FindGameObjectWithTag ("UIRoot");
		if (obj != null) {
			Debug.Log("Old UIRoot found. Queueing for destruction");
			GameObject.Destroy(obj);
		}
	}
}
