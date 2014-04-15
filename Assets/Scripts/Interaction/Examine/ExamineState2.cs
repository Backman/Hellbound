using UnityEngine;
using System.Collections;

[System.Serializable]
public class ExamineState2 : State<ExamineTest> {
	public override void pickUp (ExamineTest entity) {
		Debug.Log("Pickup examineState2");
		Inventory.getInstance().addInteractable(InventoryItem.Type.KEY);
	}
}
