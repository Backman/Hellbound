using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
	public static Inventory m_Instance = null;

	private List<InventoryItem> m_InvItems;
	private int m_GridWidth, m_GridHeight;
	private static InventoryItem m_CurrentSelectedInventoryItem = null;

	private UISprite m_InventoryMenu = null;
	private int m_InventoryIndex;
	private Dictionary<InventoryItem.Type, int> m_InventoryItemIndex = new Dictionary<InventoryItem.Type, int>();
	
	private Inventory() : base(){
		m_InvItems = new List<InventoryItem>();
		m_GridWidth = 16;
		m_GridHeight = 6;

		m_InventoryMenu = GameObject.FindGameObjectWithTag ("ExamineWindow").GetComponent<UISprite>();
		m_InventoryMenu.alpha = 0.0f;
		Debug.Log("Initialize Inventory");
		
		// Same types of objects should be placed on same space in inventory
		m_InventoryItemIndex.Add(InventoryItem.Type.KEY, 0);
		m_InventoryItemIndex.Add(InventoryItem.Type.OIL, 1);
	}

	public static Inventory getInstance(){
		if(m_Instance == null)
			m_Instance = new Inventory();
		return m_Instance;
	}

	public void add(InventoryItem invItem){
		m_InvItems.Add(invItem);
		invItem.setInventoryPosition(m_InventoryItemIndex[invItem.getType()]);
	}

	public void remove(InventoryItem invItem){
		m_InvItems.Remove(invItem);
	}

	public InventoryItem getSelectedItem(){
		return m_CurrentSelectedInventoryItem;
	}

	public void setSelectedItem(InventoryItem invItem){
		m_CurrentSelectedInventoryItem = invItem;
	}

	public void showInventoryMenu(){
		m_InventoryMenu.alpha = 1.0f;
	}

	public void hideInventoryMenu(){
		m_InventoryMenu.alpha = 0.0f;
	}
}
