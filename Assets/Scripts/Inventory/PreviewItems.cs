using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreviewItems {
	private static PreviewItems m_Instance = null;
	private Dictionary<InventoryItem.Type, PreviewItem> m_PreviewItems = new Dictionary<InventoryItem.Type, PreviewItem>();
	
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
}
