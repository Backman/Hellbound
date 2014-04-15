using UnityEngine;
using System.Collections;

public class PreviewKey : PreviewItem {

	// Use this for initialization
	protected override void Awake(){
		PreviewItems.getInstance().add("Key", this);
		gameObject.SetActive(false);
	}
	
	public override void show(){
		gameObject.SetActive(true);
	}
	
	public override void hide(){
		gameObject.SetActive(false);
	}
}
