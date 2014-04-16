using UnityEngine;
using System.Collections;

public class InventoryItem_Key : InventoryItem{
	/// <summary>
	/// Ugly *temporarily* solution to make inventory items visible, but not the "dummy" that was used to create inventory items.
	/// m_Initialized needed because Awake doesn't get called right away when using SetActive(true) on the object.
	/// </summary>
	private static bool m_Initialized = false;
	protected override void Awake(){
		base.Awake();
		if(!m_Initialized){
			m_Initialized = true;
			gameObject.SetActive(false);
		}
	}

	public override void examine(){
		Debug.Log("It's just a key goddamnit!");
	}

	public override void use(){
		Debug.Log("Use key");
	}

	public override void combine(InventoryItem invItem){
		switch(invItem.getType()) {
			case InventoryItem.Type.KEY:
				Debug.Log("Unable to combine a key with a key!");
				break;
			case InventoryItem.Type.OIL:
				Debug.Log("Unable to combine a key with oil!");
				break;
		}
	}

	public override void drop(){

	}

	public override InventoryItem.Type getType(){
		return InventoryItem.Type.KEY;
	}
}
