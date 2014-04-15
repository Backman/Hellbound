using UnityEngine;
using System.Collections;

public class Platform : PuzzleLogic {

	// Use this for initialization
	void Start () {
		base.Start();
		Messenger.Broadcast("testPls", gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void onEvent(GameObject obj){
		Debug.Log("Event triggered!");
	}
}
