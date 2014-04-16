using UnityEngine;
using System.Collections;

public class ModelViewer : MonoBehaviour{
	public float m_RotationSpeed = 100.0f;

	public GameObject r_ItemToShow = null;
	void Start(){
		Messenger.AddListener<InventoryItem>("show inventory model", previewItem);
	}

	void Update () {
		if(r_ItemToShow != null){
			r_ItemToShow.transform.Rotate(new Vector3(0.0f, m_RotationSpeed * Time.deltaTime, 0.0f));
		}
	}
	
	public void previewItem(InventoryItem item){
		if(r_ItemToShow != null && !r_ItemToShow.Equals (item.m_ModelPreview)){
			Debug.Log ("Destroying");
			Destroy (r_ItemToShow);
			r_ItemToShow = (GameObject)Instantiate (item.m_ModelPreview, transform.position, transform.rotation);
			r_ItemToShow.transform.parent = transform;
			r_ItemToShow.layer = gameObject.layer;
		} else if (r_ItemToShow == null){
			r_ItemToShow = (GameObject)Instantiate (item.m_ModelPreview, transform.position, transform.rotation);
			r_ItemToShow.transform.parent = transform;
			r_ItemToShow.layer = gameObject.layer;
		}
	}
}

