using UnityEngine;
using System.Collections;

//peter

public class Shutdown : MonoBehaviour {

	public void shutdown(){
		Application.Quit();

		//change debug into something useful for webplayer
		#if UNITY_WEBPLAYER
		Debug.LogWarning("Btw Shutdown wont work in webplayer.");
		#endif

		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
