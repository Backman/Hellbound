using UnityEngine;
using System.Collections;

/// <summary>
/// Class that is used as an interface to call functions inventory item handling functions when a button is pressed
/// </summary>
public class InventoryItem_Usage : MonoBehaviour {
	public void use(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		Inventory.getInstance().hideInventoryMenu();
		invItem.use();
		//Inventory.getInstance().hideInventoryMenu();
	}

	public void examine(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		invItem.examine();
		//Inventory.getInstance().hideInventoryMenu();
	}

	public void drop(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		invItem.drop();
		//Inventory.getInstance().hideInventoryMenu();
	}
	
	/// <summary>
	/// Let Inventory handle logic for combining
	/// </summary>
	public void combine(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		Inventory.getInstance().addCombineItem(invItem, true);
	}
}
