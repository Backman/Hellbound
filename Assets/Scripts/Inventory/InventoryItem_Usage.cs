using UnityEngine;
using System.Collections;

public class InventoryItem_Usage : UISprite {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void use(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		invItem.use();
		Inventory.getInstance().hideInventoryMenu();
	}

	public void examine(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		invItem.examine();
		Inventory.getInstance().hideInventoryMenu();
	}

	public void drop(){
		InventoryItem invItem = Inventory.getInstance().getSelectedItem();
		invItem.drop();
		Inventory.getInstance().hideInventoryMenu();
	}
}
