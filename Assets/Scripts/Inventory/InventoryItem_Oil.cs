﻿using UnityEngine;
using System.Collections;

public class InventoryItem_Oil : InventoryItem{
	public override void examine(){
		Debug.Log("It's just oil goddamnit!");
	}
	
	public override void use(){
		
	}
	
	public override void combine(InventoryItem invItem){
		switch(invItem.getType()) {
			case InventoryItem.Type.KEY:
				Debug.Log("Unable to combine oil with a key!");
				break;
			case InventoryItem.Type.OIL:
				Debug.Log("Unable to combine oil with oil!");
				break;
		}
	}

	public override void drop(){
		
	}

	public override InventoryItem.Type getType(){
		return InventoryItem.Type.OIL;
	}
}
