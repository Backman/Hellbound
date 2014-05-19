using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//anton
//peter
public class TipsCreator : MonoBehaviour {
	public GameObject r_Prefab;

	void Start(){
		HintsHandler TH = GameObject.Find ("TipsHandler").GetComponent<HintsHandler>();
		List<HintsText> hints = new List<HintsText>();
		foreach (HintsHandler eachHH in  GameObject.FindObjectsOfType<HintsHandler> ()) {
			foreach(HintsText hintText in eachHH.m_Hints){
				hints.Add(hintText);
			}
		}

		int Finished_Buttons = 0;

		foreach (HintsText t in hints) {
			
			GameObject newTips = Instantiate(r_Prefab) as GameObject;
			newTips.transform.parent = gameObject.transform;

			newTips.name = "HintsButton";
			newTips.GetComponentInChildren<TipsButton>().m_Tips = t.mText;
			newTips.GetComponentInChildren<TipsButton>().m_MainLabel = t.mTitle;

			Vector3 scaleValue = new Vector3(1f,1f,1f);
			newTips.transform.localScale = scaleValue;
			
			//--------------
			Vector3 newPosition = new Vector3(0f,-((float)(Finished_Buttons)*550),0f);
			
			newTips.transform.localPosition = newPosition;
			
			//------------
			
			Finished_Buttons++;
		}
	}
}