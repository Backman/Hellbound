using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to handle Inventory items (add, remove, etc...)
/// TODO: Remove old unused variables
/// </summary>
public class Inventory {
	private static Inventory m_Instance = null;

	private List<Interactable> m_InvItems = new List<Interactable>();

	private List<UISprite> m_Items = new List<UISprite>();

	private int m_RealInventoryItems = 0;
	private UITable r_Table = null;
	
	private Inventory(){
		r_Table = GUIManager.Instance.m_PauseWindow.r_InventoryWindow.GetComponentInChildren<UITable>();
	}

	public static Inventory getInstance(){
		if(m_Instance == null)
			m_Instance = new Inventory();
		return m_Instance;
	}

	public bool containsItem(Interactable item) {
		return m_InvItems.Contains(item);
	}

	public List<Interactable> Items {
		get { return m_InvItems; }
	}

	/// <summary>
	/// Create inventory item from InventoryType; Called by sub classes of Interactable.
	/// </summary>
	public void addInteractable(Interactable item){
		if(item.m_Thumbnail == null) {
			return;
		}
		InventoryTableController.reposition ();
		UISprite sprite = GameObject.Instantiate(item.m_Thumbnail, r_Table.transform.position, r_Table.transform.rotation) as UISprite;
		m_Items.Add (sprite);
		m_InvItems.Add (item);
		sprite.gameObject.transform.parent = r_Table.transform;
		sprite.gameObject.transform.localScale = Vector3.one;
		InventoryTableController.reposition ();
	}

	public void removeItem(Interactable item){
		InventoryTableController.reposition ();
		UISprite sprite = m_Items.Find(x => x.spriteName == item.m_Thumbnail.spriteName);
		m_Items.Remove(sprite);
		m_InvItems.Remove (item);
		GameObject.Destroy(sprite);
		InventoryTableController.reposition ();
	}
}
