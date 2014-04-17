using UnityEngine;
using System.Collections;

/// <summary>
/// Class used for changing visibility of oil model. Should be attached to the model which is to be shown/hidden
/// </summary>
public class PreviewOil : PreviewItem {
	protected override void Awake(){
		PreviewItems.getInstance().add(InventoryItem.Type.Oil, this);
		gameObject.SetActive(false);
	}
	
	public override void show(){
		gameObject.SetActive(true);
	}
	
	public override void hide(){
		gameObject.SetActive(false);
	}
}
