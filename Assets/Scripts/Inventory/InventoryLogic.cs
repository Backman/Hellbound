using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class InventoryLogic{
	private static InventoryLogic m_Instance;
	private Dictionary<string, List<GameObject>> m_Items = new Dictionary<string, List<GameObject>>();
	private UITable		   r_Table;

	private InventoryLogic(){
		r_Table = GUIManager.Instance.m_PauseWindow.r_InventoryWindow.GetComponentInChildren<UITable>();
	}

	#region Public
	public static InventoryLogic Instance{
		get{
			if( m_Instance == null ){
				m_Instance = new InventoryLogic();
			}
			return m_Instance;
		}
	}

	public bool addItem( string itemName, UISprite itemSprite ){
		if( itemName.Trim() == string.Empty || itemSprite == null ){
			Debug.LogError("Invalid item passed!");
			return false;
		}

		addItemToTable( itemName, itemSprite );
		//TODO: Check if part of combine item
		//TODO: Do logic if item is part of combie

		return true;
	}

	public void removeItem( string itemName ){
		if( m_Items.ContainsKey(itemName) ){
			GameObject lastObj = m_Items[itemName][m_Items[itemName].Count - 1];
			m_Items[itemName].Remove(lastObj);
			if(m_Items[itemName].Count == 0){
				m_Items.Remove(itemName);
			}
			removeItemFromTable(lastObj);
		} else {
			Debug.LogError("No item of that name ");
		} 
	}

	public bool containsItem( string itemName ){
		return m_Items.ContainsKey( itemName );
	}
	
	public Dictionary<string, List<GameObject>> getItems(){
		return m_Items;
	}

	#endregion

	#region Private
	private void addItemToTable( string itemName, UISprite itemSprite ){
		UISprite sprite = GameObject.Instantiate( itemSprite, r_Table.transform.position, r_Table.transform.rotation) as UISprite;
		sprite.name = itemName;

		sprite.gameObject.transform.parent = r_Table.transform;
		sprite.gameObject.transform.localScale = Vector3.one;

		if(!m_Items.ContainsKey(itemName)){
			m_Items.Add(itemName, new List<GameObject>());
		}
		m_Items[itemName].Add(sprite.gameObject);

		InventoryTableController.reposition ();
	}

	private void removeItemFromTable(GameObject item){
		bool itemRemoved = false;
		foreach( Transform child in r_Table.transform ){
			if (child.gameObject == item){
				GameObject.Destroy( child.gameObject );
				itemRemoved = true;
				break;
			}
		}

		if( !itemRemoved ){
			//Debug.LogError("No child: " + itemName + " found in itemTable" );
		}
		InventoryTableController.reposition();
	}
	#endregion
}

