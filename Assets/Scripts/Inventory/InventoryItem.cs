using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class InventoryItem : UIDragDropItem{
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
	private bool m_FixedToGrid = true;

	void Start()
	{
		base.Start();
		Inventory.getInstance().add(this);
		//Debug.Log ("Created an inventory item: " + m_InventoryPos);
	}
	
	protected override void OnDragDropStart(){
		m_FixedToGrid = false;
		Inventory.getInstance().setSelectedItem(this);
		base.OnDragDropStart();
	}

	protected override void OnDragDropRelease(GameObject surface){
		if(surface != null){
			InventoryItem invItem = surface.GetComponent<InventoryItem>();
			if(invItem != null) {
				combine(invItem);
			}
			//Debug.Log("Dropped on: "+surface.name);
			//GameObject respawns = GameObject.FindGameObjectsWithTag("Respawn");
		}
		m_FixedToGrid = true;
		base.OnDragDropRelease(null);
	}

	void Update(){
		if(m_FixedToGrid){
			gameObject.transform.localPosition = new Vector3(m_InventoryPos*100.0f, 0, 0.0f);
			//gameObject.transform.position = new Vector3(-1.9f + m_InventoryPos*0.3f, 0.65f, 0.0f);
		}
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
