﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
	public static Inventory m_Instance = null;

	private List<InventoryItem> m_InvItems = new List<InventoryItem>();
	private List<InventoryItem> m_CombineItems = new List<InventoryItem>();
	private int m_GridWidth, m_GridHeight;
	private static InventoryItem m_CurrentSelectedInventoryItem = null;

	private GameObject m_InventoryMenu = null;
	private int m_InventoryIndex;
	private Dictionary<InventoryItem.Type, int> m_InventoryItemIndex = new Dictionary<InventoryItem.Type, int>();
	private Dictionary<InventoryItem.Type, GameObject> m_InventoryItemIdentifier = new Dictionary<InventoryItem.Type, GameObject>();

	private int m_RealInventoryItems = 0;
	
	private Inventory(){
		m_GridWidth = 16;
		m_GridHeight = 6;

		m_InventoryMenu = GameObject.FindGameObjectWithTag("ExamineWindow");
		m_InventoryMenu.SetActive(false);
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

	public void add(InventoryItem invItem, GameObject obj){
		m_InvItems.Add(invItem);
		invItem.setInventoryPosition(m_InventoryItemIndex[invItem.getType()]);
		if(!m_InventoryItemIdentifier.ContainsKey(invItem.getType())){
			m_InventoryItemIdentifier.Add(invItem.getType(), obj);
		}
		else{
			++m_RealInventoryItems;
		}
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
		m_InventoryMenu.SetActive(true);
	}

	public void hideInventoryMenu(){
		m_InventoryMenu.SetActive(false);
	}
	
	public void addCombineItem(InventoryItem invItem, bool combine = true){
		if(!combine){
			if(m_CombineItems.Count != 0){
				m_CombineItems.Add(invItem);
			}
		}
		else{
			m_CombineItems.Add(invItem);
			// Combine if two inventory items has been selected to be combined
			if(m_CombineItems.Count == 1){
				Combine.show();
			}
			else if(combine){
				combineItems();
			}
		}
	}
	
	public void combineItems(){
		Debug.Log ("Combine selected items!");
		m_CombineItems.Clear();
		Combine.hide();
	/*
		foreach(InventoryItem combineItem in m_CombineItems){
			combineItem.combine(
		}
		*/
	}

	public void addInteractable(InventoryItem.Type inventoryType){
		GameObject obj = m_InventoryItemIdentifier[inventoryType];
		GameObject copy = GameObject.Instantiate(obj, obj.transform.position, obj.transform.rotation) as GameObject;
		copy.transform.parent = obj.transform.parent;
		copy.transform.localScale = Vector3.one;
		float x = m_RealInventoryItems * 150;
		copy.transform.localPosition = new Vector3(x, 0.0f, 0.0f);
		copy.SetActive(true);
	}
}
