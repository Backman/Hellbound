using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {
	
	Transform r_PlayerTransform;

	void Start(){

		Initialize ();

		DontDestroyOnLoad (gameObject);

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

	void Update(){
		transform.position = r_PlayerTransform.position;
		transform.rotation = r_PlayerTransform.rotation;
	}

	void OnLevelWasLoaded(int level){
		Initialize ();
	}

	private void Initialize(){
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("PlayerSpawn");
		
		foreach (GameObject g in spawns) {
			if(gameObject != g){
				Destroy(g);
			}
		}
		
		r_PlayerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		
		r_PlayerTransform.position = gameObject.transform.position;
		r_PlayerTransform.rotation = gameObject.transform.rotation;
	}
}
