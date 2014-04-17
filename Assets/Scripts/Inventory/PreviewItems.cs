using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for keeping a list of inventory preview objects, and switching which object to preview
/// </summary>
public class PreviewItems {
	public float m_RotationSpeed = 100.0f;

	private static PreviewItems m_Instance = null;
	private Dictionary<InventoryItem.Type, PreviewItem> m_PreviewItems = new Dictionary<InventoryItem.Type, PreviewItem>();
	private GameObject m_ItemToShow = null;
	public static PreviewItems getInstance(){
		if(m_Instance == null){
			m_Instance = new PreviewItems();
		}
		return m_Instance;
	}
		
	public void add(InventoryItem.Type type, PreviewItem item){
		m_PreviewItems.Add(type, item);
	}
	
	public void previewItem(InventoryItem.Type type){
		foreach(KeyValuePair<InventoryItem.Type, PreviewItem> item in m_PreviewItems){
			if(item.Key != type){
				item.Value.hide();
			}
			else{
				item.Value.show();	
			}
		}
	}

	void Update () {
		if(m_ItemToShow != null){
			m_ItemToShow.transform.Rotate(new Vector3(0.0f, m_RotationSpeed * Time.deltaTime, 0.0f));
		}
	}

	public void previewItem(InventoryItem item){
		m_ItemToShow = item.m_ModelPreview;
	}
}
