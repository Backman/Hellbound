using UnityEngine;
using System.Collections;

public class InventoryInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Debug.Log("Initialized!");
		GameObject key = GameObject.FindGameObjectWithTag("InventoryKey");
		GameObject keyCopy = Instantiate(key) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
