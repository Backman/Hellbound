using UnityEngine;
using System.Collections;

public class InventoryInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Debug.Log("Initialized!");
		GameObject key = GameObject.FindGameObjectWithTag("InventoryKey");
		GameObject keyCopy = Instantiate(key, key.transform.position, key.transform.rotation) as GameObject;
		keyCopy.transform.parent = key.transform.parent;
		keyCopy.transform.localScale = Vector3.one;
		//Debug.Log ("Parent: "+key.transform.parent.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
