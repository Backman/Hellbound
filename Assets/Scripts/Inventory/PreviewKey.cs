using UnityEngine;
using System.Collections;

/// <summary>
/// Class used for changing visibility of key model. Should be attached to the model which is to be shown/hidden
/// </summary>
public class PreviewKey : PreviewItem {
	// Use this for initialization
	protected override void Awake(){
		PreviewItems.getInstance().add(InventoryItem.Type.KEY, this);
		gameObject.SetActive(false);
	}
	
	public override void show(){
		gameObject.SetActive(true);
	}
	
	public override void hide(){
		gameObject.SetActive(false);
	}
}
