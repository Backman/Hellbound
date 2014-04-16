﻿using UnityEngine;
using System.Collections;

public class InventoryItem_Key : InventoryItem{
	private static bool m_Initialized = false;
	protected override void Awake(){
		base.Awake();
		if(!m_Initialized){
			m_Initialized = true;
			gameObject.SetActive(false);
		}
	}

	public override void examine(){
		Debug.Log("It's just a key goddamnit!");
	}

	public override void use(){
		Debug.Log("Use key");
	}

	public override void combine(InventoryItem invItem){
		switch(invItem.getType()) {
			case InventoryItem.Type.KEY:
				Debug.Log("Unable to combine a key with a key!");
				break;
			case InventoryItem.Type.OIL:
				Debug.Log("Unable to combine a key with oil!");
				break;
		}
	}

	public override void drop(){

	}

	public override InventoryItem.Type getType(){
		return InventoryItem.Type.KEY;
	}
}
