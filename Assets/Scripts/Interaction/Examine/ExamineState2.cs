using UnityEngine;
using System.Collections;

[System.Serializable]
public class ExamineState2 : State<ExamineTest> {
	public override void pickUp (ExamineTest entity) {
		Debug.Log("Pickup examineState2");
		Inventory.getInstance().addInteractable(entity.m_InventoryItem, entity);
	}

	public override void activate (ExamineTest entity)
	{
		Debug.Log ("Using: " + entity);
	}
}
