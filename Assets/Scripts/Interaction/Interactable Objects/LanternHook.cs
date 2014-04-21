using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanternHook : Interactable {
	
	public LanternHookOpenState m_OpenedState = new LanternHookOpenState();
	public LanternHookCloseState m_LockedState = new LanternHookCloseState();

	public Interactable m_ObjectToOpen;

	public StateMachine<LanternHook> m_FSM;
	public bool m_IsOpen = false;
	// Use this for initialization
	protected override void Start () {
		base.Start();

		m_FSM = new StateMachine<LanternHook>(this, m_LockedState);
		m_FSM.addState(m_OpenedState);
	}
	
	public override void useWith (GameObject obj) {
		Interactable key = obj.GetComponent<Interactable>();
		if(key.m_InventoryItem.getType () == InventoryItem.Type.Lantern && !m_IsOpen){
			open();
			Inventory.getInstance().removeItem(key.m_InventoryItem);
		}
	}

	public void Update(){
		m_FSM.update (Time.deltaTime);
	}

	public void open(){
		m_IsOpen = true;
	}
	
	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}