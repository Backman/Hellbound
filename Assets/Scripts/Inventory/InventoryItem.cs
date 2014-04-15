﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private int m_InventoryPos = 0;

	protected virtual void Awake()
	{
		Inventory.getInstance().add(this, gameObject);
	}

	public void setInventoryPosition(int position){
		m_InventoryPos = position;
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
