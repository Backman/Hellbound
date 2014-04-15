using UnityEngine;
using System.Collections;

public class PreviewOil : PreviewItem {
	protected override void Awake(){
		PreviewItems.getInstance().add(InventoryItem.Type.OIL, this);
		gameObject.SetActive(false);
	}
	
	public override void show(){
		gameObject.SetActive(true);
	}
	
	public override void hide(){
		gameObject.SetActive(false);
	}
}
