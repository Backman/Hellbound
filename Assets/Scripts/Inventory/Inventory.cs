using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
	public static Inventory m_Instance = null;

	private List<InventoryItem> m_InvItems;
	private int m_GridWidth, m_GridHeight;

	public Inventory() : base(){
		m_InvItems = new List<InventoryItem>();
		m_GridWidth = 16;
		m_GridHeight = 6;
	}

	public static Inventory getInstance(){
		if(m_Instance == null)
			m_Instance = new Inventory();
		return m_Instance;
	}

	public void add(InventoryItem invItem){
		int index = m_InvItems.Count;
		m_InvItems.Add(invItem);
		invItem.setInventoryPosition(index);
	}

	public void remove(InventoryItem invItem){
		m_InvItems.Remove(invItem);
	}
}
