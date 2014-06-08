using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Locked interactable.
/// 
/// Legacy script, no longer in use.
/// 
/// Was previusly used to regulate objects which could be locked/unlocked.
/// Was obsolete by Alexis PuzzleLogic system
/// 
/// Created by Simon Jonasson
/// </summary>
public class LockedInteractable : Interactable {
	public string m_LockedText = "I can't open this";
	public LockedState m_LockedState;
	public OpenedState m_OpenedState;

	public List<Interactable> m_NeedsToBeOpen = new List<Interactable>();
	public List<string> m_NeedsItems = new List<string>();

	private Dictionary<Interactable, bool> m_InteractableKeyState = new Dictionary<Interactable, bool>();
	private Dictionary<string, bool> m_ItemKeyState = new Dictionary<string, bool>();
	
	public StateMachine<LockedInteractable> m_FSM;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		initializeKeyState();
		m_FSM = new StateMachine<LockedInteractable>(this, m_LockedState);
		m_FSM.addState(m_OpenedState);
	}

	public override void useWith (GameObject obj) {
		Interactable key = obj.GetComponent<Interactable>();

		if(m_NeedsToBeOpen.Contains (key)){
			m_InteractableKeyState[key] = true;
		}
		if( allKeys() ){
			m_FSM.changeState<OpenedState>();
		}
	}
	
	public override void activate ()
	{
		//base.activate ();
		bool all = true;

		//Check if all needed items is in inventory.
		foreach( string s in m_NeedsItems ){
			all = InventoryLogic.Instance.containsItem(s) & all;
		}

		//If all needed items were in inventory, remove them from inventory
		//and change state to open state
		if( all && allOpen() ){
			foreach( string s in m_NeedsItems ){
				m_ItemKeyState[s] = true;
				InventoryLogic.Instance.removeItem( s );
			}
			m_FSM.changeState<OpenedState>();
		} else {
			if(GUIManager.Instance.simpleShowText(m_LockedText, "Use")){
				base.activate ();
			}
		}

		PuzzleEvent.trigger("onOpenInteractable", gameObject, false);
		m_FSM.CurrentState.activate (this);

	}
	
	private void initializeKeyState(){
		foreach(Interactable obj in m_NeedsToBeOpen){
			m_InteractableKeyState.Add (obj, false);
		}
		foreach(string s in m_NeedsItems ){
			m_ItemKeyState.Add (s, false );
		}
	}
	
	public bool allItems() {
		bool ret = true;
		foreach( bool b in m_ItemKeyState.Values){
			ret = ret & b;
		}
		return ret;
	}

	public bool allOpen(){
		bool ret = true;
		foreach( bool b in m_InteractableKeyState.Values ){
			ret = ret & b;
		}
		return ret;
	}

	public bool allKeys(){
		return ( allItems() && allOpen() ) == true;
	}
	
	public override void examine () {
		m_FSM.CurrentState.examine (this);
	}
}