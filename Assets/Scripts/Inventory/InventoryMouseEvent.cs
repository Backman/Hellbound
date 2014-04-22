using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to create mouse events for pressing on inventory items
/// </summary>
public class InventoryMouseEvent : MonoBehaviour {
	private static bool m_LeftMouseDown;
	private static GameObject r_MouseDownObject = null;
	// Use this for initialization
	void Start () {
		m_LeftMouseDown = Input.GetKeyDown(KeyCode.Mouse0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0) && !m_LeftMouseDown){
			m_LeftMouseDown = true;
			onMouseClick(0);
		}
		else if(Input.GetKeyUp(KeyCode.Mouse0) && m_LeftMouseDown){
			m_LeftMouseDown = false;
			onMouseRelease(0);
		}
	}

	// Button 0 is mouse0, (Left mouse button)
	// Button 1 is mouse1, (Right mouse button)
	void onMouseClick(int button){
		if(button == 0) {
			r_MouseDownObject = UICamera.hoveredObject;
		}
	}

	void onMouseRelease(int button){
		if(r_MouseDownObject && button == 0) {
			InventoryItem invItem = r_MouseDownObject.GetComponent<InventoryItem>();
			if (UICamera.hoveredObject == r_MouseDownObject && invItem != null) {
				Messenger.Broadcast<InventoryItem>("show inventory model", invItem);
				Inventory.getInstance().setSelectedItem(invItem);
				Inventory.getInstance().showInventoryMenu();
				Inventory.getInstance().addCombineItem(invItem, false);
			}
			r_MouseDownObject = null;
		}
	}
}
