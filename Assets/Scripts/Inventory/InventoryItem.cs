using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class for Inventory items
/// </summary>
public abstract class InventoryItem : MonoBehaviour{
	public enum Type{
		KEY,
		OIL
	};

	public abstract void examine();
	public abstract void use();
	public abstract void combine(InventoryItem invItem);
	public abstract void drop();
	public abstract InventoryItem.Type getType();

	protected virtual void Awake(){
		Inventory.getInstance().add(this, gameObject);
	}
	
	public UIAtlas getAtlas(){
		return gameObject.GetComponent<UISprite>().atlas;
	}
	/*
	public static InventoryItem createFromInteractable(Interactable obj){
		GameObject item = GameObject.FindGameObjectWithTag(obj.getInventoryName());
		if(item != null){
			GameObject itemCopy = Instantiate(item) as GameObject;
			if(itemCopy != null){
				return itemCopy.GetComponent<InventoryItem>();
			}
		}
		return null;
	}*/
}
