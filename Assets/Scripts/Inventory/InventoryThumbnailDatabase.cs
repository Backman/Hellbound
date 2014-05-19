using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InventoryThumbnailData {
	[SerializeField] string m_Name;
	[SerializeField] UISprite m_Sprite;
	
	public InventoryThumbnailData(){
		m_Name = "";
		m_Sprite = null;
	}
	
	public string getName(){
		return m_Name;
	}
	
	public UISprite getSprite(){
		return m_Sprite;
	}
	
	public void setName(string name){
		m_Name = name;
	}
	
	public void setSprite(UISprite sprite){
		m_Sprite = sprite;
	}
};

public class InventoryThumbnailDatabase : MonoBehaviour {
	[SerializeField] List<InventoryThumbnailData> m_ThumbnailData = new List<InventoryThumbnailData>();
	private static InventoryThumbnailDatabase Instance = null;
	
	void Awake(){
		Instance = this;
	}
	
	public List<InventoryThumbnailData> getThumbnailData(){
		return m_ThumbnailData;
	}
	
	public static UISprite getThumbnail(string name){
		if(Instance != null) {
			foreach(InventoryThumbnailData inventoryThumbnailData in Instance.m_ThumbnailData){
				if(inventoryThumbnailData.getName() == name){
					return inventoryThumbnailData.getSprite();
				}
			}
		}
		return null;
	}
}
