using UnityEngine;
using System.Collections;

public class InventoryItem_Key : InventoryItem{

	public override void combine(InventoryItem invItem){
		switch(invItem.getType()) {
		case InventoryItem.Type.Key:
			Debug.Log("Unable to combine a key with a key!");
			break;
		default:
			Debug.Log("Unable to combine a key with oil!");
			break;
		}
	}

	public override void drop(){

	}
}
