using UnityEngine;
using System.Collections;

public class InventoryInit : MonoBehaviour {
	private static bool initialized = false;
	private static GameObject m_Object;
	void Awake(){
		m_Object = gameObject;
		Debug.Log("Awake: "+gameObject.name);
		if(!initialized){
			initialized = true;
		}
	}
	// Use this for initialization
	void Start () {
		//Debug.Log("Initialized!");
        //GameObject key = GameObject.FindGameObjectWithTag("InventoryKey");
        //GameObject keyCopy = Instantiate(key, key.transform.position, key.transform.rotation) as GameObject;
        //keyCopy.transform.parent = key.transform.parent;
        //keyCopy.transform.localScale = Vector3.one;
		//Debug.Log ("Parent: "+key.transform.parent.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
