using UnityEngine;
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
	public GameObject tipsPanel;

	void Start(){

		GameObject tipsPanel = null;
		while (tipsPanel == null) {
			tipsPanel = GameObject.FindGameObjectWithTag("TipsPanel");
		}


		int Finished_Buttons = 0;

		foreach (Tips t in tips) {

			GameObject newTips = Instantiate(r_Prefab) as GameObject;

			tipsPanel.transform.parent = newTips.transform;

			newTips.name = "TipsButton";
			newTips.GetComponentInChildren<UILabel>().text = t.Text;
			Vector3 newValue = new Vector3(0f,(float)(Finished_Buttons * 525), 0);
			newTips.transform.position = newValue;

			Finished_Buttons++;
		}
	}
}
