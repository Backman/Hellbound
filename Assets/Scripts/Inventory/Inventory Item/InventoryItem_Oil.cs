using UnityEngine;
using System.Collections;

public class InventoryItem_Oil : InventoryItem{
	
	public override void combine(InventoryItem invItem){
		switch(invItem.getType()) {
		case InventoryItem.Type.Key:
			Debug.Log("Unable to combine oil with a key!");
			break;
		case InventoryItem.Type.Oil:
			Debug.Log("Unable to combine oil with oil!");
			break;
		}
	}

	public override void drop(){
		
	}
}
