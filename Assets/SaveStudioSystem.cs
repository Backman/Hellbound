using UnityEngine;
using System.Collections;

public class SaveStudioSystem : MonoBehaviour {
	
	void Start(){
		StartCoroutine ("waitforStudioSystems");
	}
	
	private IEnumerator waitforStudioSystems(){
		
		GameObject studiosystem = null;
		
		while (studiosystem == null) {
			studiosystem = GameObject.Find("FMOD_StudioSystem");
			yield return new WaitForSeconds(0.01f);
		}
		DontDestroyOnLoad (studiosystem);
		yield return 0;
	}
}
