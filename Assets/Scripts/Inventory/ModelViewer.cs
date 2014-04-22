using UnityEngine;
using System.Collections;

public class ModelViewer : MonoBehaviour{
	public float m_RotationSpeed = 100.0f;

	public GameObject r_ItemToShow = null;

	private Transform r_Transform;
	private Bounds m_Bounds;
	void Start(){
		r_Transform = transform;
		Messenger.AddListener<InventoryItem>("show inventory model", previewItem);
	}

	void Update () {
		if(r_ItemToShow != null && Input.GetButton("Fire1")) {
			float moveX = Input.GetAxis("Mouse X");
			float moveY = Input.GetAxis("Mouse Y");
			Vector3 pivot = m_Bounds.center;

			r_Transform.RotateAround(pivot, Vector3.up, -moveX * m_RotationSpeed);
			r_Transform.RotateAround(pivot, Vector3.right, moveY * m_RotationSpeed);
		}
	}
	
	public void previewItem(InventoryItem item){
		if(item.m_ModelPreview == null) {
			return;
		}

		if(r_ItemToShow != null) {
			Destroy(r_ItemToShow);
		}
		
		r_ItemToShow = Instantiate(item.m_ModelPreview, transform.position, transform.rotation) as GameObject;
		r_ItemToShow.transform.parent = r_Transform;
		r_ItemToShow.layer = gameObject.layer;

		Collider col = r_ItemToShow.GetComponent<BoxCollider>();
		if(col == null) {
			col = r_ItemToShow.AddComponent<BoxCollider>();
		}

		m_Bounds = col.bounds;


	}
}

