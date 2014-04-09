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
	public abstract InventoryItem.Type getType();

	private int m_InventoryPos = 0;
	private bool m_FixedToGrid = true;

	public InventoryItem() : base(){
		Inventory.getInstance().add(this);
	}

	protected override void OnDragDropStart(){
		m_FixedToGrid = false;
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
		base.OnDragDropRelease(surface);
	}

	void Update(){
		if(m_FixedToGrid){
			gameObject.transform.position = new Vector3(-2.5f + 0.2f * m_InventoryPos, 0.0f, 0.0f);
		}
	}

	public void setInventoryPosition(int position){
		m_InventoryPos = position;
	}
}
