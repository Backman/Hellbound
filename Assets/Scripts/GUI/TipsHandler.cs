﻿using UnityEngine;
using System.Collections;

public class TipsHandler : MonoBehaviour {
	
	[System.Serializable]
	public class Tips{
		[SerializeField][Multiline]
		public string Text;
	}
	[HideInInspector]
	public GameObject r_Prefab;
	public Tips[] tips;
	
	void Awake(){
		
		int Finished_Buttons = 0;
		foreach (Tips t in tips) {
			
			GameObject newTips = Instantiate(r_Prefab) as GameObject;
			newTips.transform.parent = gameObject.transform;
			
			newTips.name = "TipsButton";
			newTips.GetComponentInChildren<TipsButton>().m_Tips = t.Text;
			
			
			newTips.GetComponentInChildren<TipsButton>().m_MainLabel = "Tips #" + Finished_Buttons.ToString();
			Vector3 scaleValue = new Vector3(1f,1f,1f);
			newTips.transform.localScale = scaleValue;
			
			//--------------
			Vector3 newPosition = new Vector3(0f,-((float)(Finished_Buttons)*525),0f);
			
			newTips.transform.localPosition = newPosition;
			
			//------------
			
			Finished_Buttons++;
		}
	}
}