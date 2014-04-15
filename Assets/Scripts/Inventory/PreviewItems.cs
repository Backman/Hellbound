using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreviewItems {
	private static PreviewItems m_Instance = null;
	private Dictionary<string, PreviewItem> m_PreviewItems = new Dictionary<string, PreviewItem>();
	
	public static PreviewItems getInstance(){
		if(m_Instance == null){
			m_Instance = new PreviewItems();
		}
		return m_Instance;
	}
	
	public void add(string id, PreviewItem item){
		m_PreviewItems.Add(id, item);
	}
	
	public void previewItem(string id){
		foreach(KeyValuePair<string, PreviewItem> item in m_PreviewItems){
			if(item.Key != id){
				item.Value.hide();
			}
			else{
				item.Value.show();	
			}
		}
	}
}
