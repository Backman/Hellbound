using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//arvid

public class HintsWindow : MonoBehaviour {
	private int m_LabelCount;

	public UITable m_Table;
	public HintObject r_Hint;

	void Start() {
		Messenger.Broadcast<HintsWindow>("add hints", this);

	}

	public void addHint(HintsText hintsText) {

		GameObject hintClone = (GameObject)Instantiate(r_Hint.gameObject);
		hintClone.name = r_Hint.name + m_LabelCount;
		hintClone.transform.parent = m_Table.transform;
		hintClone.transform.localScale = Vector3.one;
		
		m_Table.Reposition();

		HintObject hintObject = hintClone.GetComponent<HintObject>();
		hintObject.m_Title.text = hintsText.mTitle;
		hintObject.m_Description.text = hintsText.mText;

		m_LabelCount++;
		m_Table.Reposition();
	}
}
