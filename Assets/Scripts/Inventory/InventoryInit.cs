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
			copy();
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
	
	public static GameObject copy(){
		GameObject obj = Instantiate(m_Object, m_Object.transform.position, m_Object.transform.rotation) as GameObject;
		obj.transform.parent = m_Object.transform.parent;
		obj.transform.localScale = Vector3.one;
		return obj;
	}
}
