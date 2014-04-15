using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryMouseEvent : MonoBehaviour {

	private static bool m_LeftMouseDown;
	private static GameObject m_MouseDownObject;
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
			m_MouseDownObject = UICamera.hoveredObject;
		}
	}

	void onMouseRelease(int button){
		if(m_MouseDownObject && button == 0) {
			InventoryItem invItem = m_MouseDownObject.GetComponent<InventoryItem>();
			if (UICamera.hoveredObject == m_MouseDownObject && invItem != null) {
				PreviewItems.getInstance().previewItem(invItem.getType());
				Debug.Log("Preview: "+invItem.name);
				Inventory.getInstance().setSelectedItem(invItem);
				Inventory.getInstance().showInventoryMenu();
				Inventory.getInstance().addCombineItem(invItem, false);
			}
			m_MouseDownObject = null;
		}
	}
}
