using UnityEngine;
using System.Collections;

public class _InventoryItem : MonoBehaviour {

	void Update(){
		if(Input.GetKeyDown(KeyCode.Mouse0)){
			GameObject obj = UICamera.hoveredObject;
			Debug.Log("Hit: "+obj.name);
		}
	}
}
