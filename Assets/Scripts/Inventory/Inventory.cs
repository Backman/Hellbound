using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to handle Inventory items (add, remove, etc...)
/// TODO: Remove old unused variables
/// </summary>
public class Inventory {
	private static Inventory m_Instance = null;

	private List<InventoryItem> r_InvItems = new List<InventoryItem>();
	private List<InventoryItem> r_CombineItems = new List<InventoryItem>();
	private static InventoryItem m_CurrentSelectedInventoryItem = null;

	private GameObject r_InventoryMenu = null;
	private int m_InventoryIndex;
	private Dictionary<InventoryItem.Type, int> m_InventoryItemIndex = new Dictionary<InventoryItem.Type, int>();

	private List<InventoryItem> m_Items = new List<InventoryItem>();

	private int m_RealInventoryItems = 0;
	private UIGrid r_Grid = null;
	
	private Inventory(){
		Messenger.AddListener("reset pause window", resetInventory);
		r_InventoryMenu = GameObject.FindGameObjectWithTag("ExamineWindow");
		//r_InventoryMenu.SetActive(false);
		r_Grid = GUIManager.Instance.m_PauseWindow.r_InventoryWindow.GetComponentInChildren<UIGrid>();

		// Same types of objects should be placed on same space in inventory
		m_InventoryItemIndex.Add(InventoryItem.Type.Key, 0);
		m_InventoryItemIndex.Add(InventoryItem.Type.Oil, 1);
	}

	public void resetInventory() {
		hideInventoryMenu();
		m_CurrentSelectedInventoryItem = null;
		r_CombineItems.Clear ();
	}

	public static Inventory getInstance(){
		if(m_Instance == null)
			m_Instance = new Inventory();
		return m_Instance;
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
		TweenAlpha tween = (TweenAlpha)r_InventoryMenu.GetComponent(typeof(TweenAlpha));
		tween.PlayForward();
	}

	public void hideInventoryMenu(){
		TweenAlpha tween = (TweenAlpha)r_InventoryMenu.GetComponent(typeof(TweenAlpha));
		tween.PlayReverse();
	}
	
	public void addCombineItem(InventoryItem invItem, bool combine = true){
		if(!combine){
			if(r_CombineItems.Count != 0){
				r_CombineItems.Add(invItem);
			}
		}
		else{
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
	public void addInteractable(InventoryItem item, Interactable interactable){
		InventoryGridController.reposition ();
		InventoryItem obj = GameObject.Instantiate(item, r_Grid.transform.position, r_Grid.transform.rotation) as InventoryItem;
		m_Items.Add (obj);
		obj.transform.parent = r_Grid.transform;
		obj.transform.localScale = Vector3.one;
		obj.GetComponent<InventoryItem>().InteractableObject = interactable;
		InventoryGridController.reposition ();
	}

	public void removeItem(InventoryItem item){
		InventoryGridController.reposition ();
		InventoryItem obj = m_Items.Find(x => x.getType() == item.getType());
		m_Items.Remove(obj);
		GameObject.Destroy(obj.gameObject);
		InventoryGridController.reposition ();
	}
}
