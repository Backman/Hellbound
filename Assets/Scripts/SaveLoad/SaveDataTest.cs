using UnityEngine;
using System.Collections;

public class SaveDataTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SaveData saveData = new SaveData();
		Game.save(saveData, "save.hbsg");
		
		Game.load("save.hbsg");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
