using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to handle Inventory items (add, remove, etc...)
/// TODO: Remove old unused variables
/// </summary>
public class Inventory {
	public static Inventory m_Instance = null;

	private List<InventoryItem> r_InvItems = new List<InventoryItem>();
	private List<InventoryItem> r_CombineItems = new List<InventoryItem>();
	private static InventoryItem m_CurrentSelectedInventoryItem = null;

	private GameObject r_InventoryMenu = null;
	private int m_InventoryIndex;
	private Dictionary<InventoryItem.Type, int> m_InventoryItemIndex = new Dictionary<InventoryItem.Type, int>();
	private Dictionary<InventoryItem.Type, GameObject> r_InventoryItemIdentifier = new Dictionary<InventoryItem.Type, GameObject>();

	private int m_RealInventoryItems = 0;
	
	private Inventory(){
		r_InventoryMenu = GameObject.FindGameObjectWithTag("ExamineWindow");
		r_InventoryMenu.SetActive(false);
		
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
		r_InvItems.Add(invItem);
		if(!r_InventoryItemIdentifier.ContainsKey(invItem.getType())){
			r_InventoryItemIdentifier.Add(invItem.getType(), obj);
		}
		else{
			++m_RealInventoryItems;
		}
	}

	public void remove(InventoryItem invItem){
		r_InvItems.Remove(invItem);
	}

	public InventoryItem getSelectedItem(){
		return m_CurrentSelectedInventoryItem;
	}

	public void setSelectedItem(InventoryItem invItem){
		m_CurrentSelectedInventoryItem = invItem;
	}

	public void showInventoryMenu(){
		r_InventoryMenu.SetActive(true);
	}

	public void hideInventoryMenu(){
		r_InventoryMenu.SetActive(false);
	}
	
	public void addCombineItem(InventoryItem invItem, bool combine = true){
		if(!combine){
			if(r_CombineItems.Count != 0){
				r_CombineItems.Add(invItem);
			}
		}
		else{
			Debug.Log("Show combine stuff");
			r_CombineItems.Add(invItem);
			// Combine if two inventory items has been selected to be combined
			if(r_CombineItems.Count == 1){
				Combine.show();
			}
			else if(combine){
				combineItems();
			}
		}
	}
	
	public void combineItems(){
		Debug.Log ("Combine selected items!");
		r_CombineItems.Clear();
		Combine.hide();
	/*
		foreach(InventoryItem combineItem in m_CombineItems){
			combineItem.combine(
		}
		*/
	}

	/// <summary>
	/// Create inventory item from InventoryType; Called by sub classes of Interactable.
	/// </summary>
	public void addInteractable(InventoryItem.Type inventoryType){
		GameObject obj = r_InventoryItemIdentifier[inventoryType];
		GameObject copy = GameObject.Instantiate(obj, obj.transform.position, obj.transform.rotation) as GameObject;
		copy.transform.parent = obj.transform.parent;
		copy.transform.localScale = Vector3.one;
		float x = m_RealInventoryItems * 150;
		copy.transform.localPosition = new Vector3(x, 0.0f, 0.0f);
		copy.SetActive(true);
		InventoryGridController.reposition ();
	}
}
