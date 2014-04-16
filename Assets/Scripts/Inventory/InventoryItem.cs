using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class for Inventory items
/// </summary>
public abstract class InventoryItem : MonoBehaviour{
	public enum Type{
		Key,
		Oil
	};

	public Type m_InventoryType;
	public GameObject m_ModelPreview;
	public Interactable InteractableObject {
		get; set;
	}

	public void examine() { InteractableObject.examine (); }
	public void use() { InteractableObject.activate (); }
	public abstract void combine(InventoryItem invItem);
	public abstract void drop();
	public InventoryItem.Type getType(){
		return m_InventoryType;
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
