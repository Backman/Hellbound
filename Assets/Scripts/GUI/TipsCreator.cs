using UnityEngine;
using System.Collections;
//anton
public class TipsCreator : MonoBehaviour { 

	[HideInInspector]
	public GameObject r_Prefab;

	void Start(){
		TipsHandler TH = GameObject.Find ("TipsHandler").GetComponent<TipsHandler>();

		TipsHandler.Tips[] tips = TH.tips;

		int Finished_Buttons = 0;

		foreach (TipsHandler.Tips t in tips) {
			
			GameObject newTips = Instantiate(r_Prefab) as GameObject;
			newTips.transform.parent = gameObject.transform;
			
			newTips.name = "TipsButton";
			newTips.GetComponentInChildren<TipsButton>().m_Tips = t.Text;
			newTips.GetComponentInChildren<TipsButton>().m_MainLabel = t.Title;

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