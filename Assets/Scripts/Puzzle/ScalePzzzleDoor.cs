using UnityEngine;
using System.Collections;

public class ScalePzzzleDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onScalePuzzleCleared", scalePuzzleCleared);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void scalePuzzleCleared(GameObject obj, bool tr){
		gameObject.SetActive(false);
	}
}
