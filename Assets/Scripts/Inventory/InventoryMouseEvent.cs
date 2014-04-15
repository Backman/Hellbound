using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryMouseEvent : MonoBehaviour {

	private static bool m_LeftMouseDown, m_RightMouseDown;
	private static GameObject m_MouseDownObject;
	// Use this for initialization
	void Start () {
		m_LeftMouseDown = Input.GetKeyDown(KeyCode.Mouse0);
		m_RightMouseDown = Input.GetKeyDown(KeyCode.Mouse1);
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

		if(Input.GetKeyDown(KeyCode.Mouse1) && !m_RightMouseDown){
			m_RightMouseDown = true;
			onMouseClick(1);
		}
		else if(Input.GetKeyUp(KeyCode.Mouse1) && m_RightMouseDown){
			m_RightMouseDown = false;
			onMouseRelease(1);
		}
	}

	// Button 0 is mouse0, (Left mouse button)
	// Button 1 is mouse1, (Right mouse button)
	void onMouseClick(int button){
		if(button == 1) {
			m_MouseDownObject = UICamera.hoveredObject;
		}
		
		bool hide = true;
		if(UICamera.hoveredObject != null){
			UISprite sprite = UICamera.hoveredObject.GetComponent<UISprite>();
			if(sprite != null){
				if(sprite.name == "ExamineWindow"){
					hide = false;
				}
				UIRect child = sprite.parent;
				// Check if the sprite or its parents name is ExamineWindow
				// and only hide examine window when pressing outside it
				if(hide){
					while(child != null){
						if(child.name == "ExamineWindow"){
							hide = false;
							break;
						}
						child = child.parent;
					}
				}
				if(hide){
					Inventory.getInstance().hideInventoryMenu();
				}
			}
			else{
				Inventory.getInstance().hideInventoryMenu();
			}
		}
		else{
			Inventory.getInstance().hideInventoryMenu();
		}
	}

	void onMouseRelease(int button){
		if(m_MouseDownObject && button == 1) {
			InventoryItem invItem = m_MouseDownObject.GetComponent<InventoryItem> ();
			if (invItem != null) {
				Inventory.getInstance().setSelectedItem(invItem);
				Inventory.getInstance().showInventoryMenu();
			}
			m_MouseDownObject = null;
		}
	}
}
