using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HintsWindow : MonoBehaviour {
	private int m_LabelCount;

	public UITable m_Table;
	public HintObject r_Hint;

	void Awake() {
		r_Hint = m_Table.GetComponentInChildren<HintObject>();
	}

	void Start() {
		Messenger.Broadcast<HintsWindow>("add hints", this);
		
		r_Hint.gameObject.SetActive(false);

	}

	public void addHint(HintsText hintsText) {

		GameObject hintClone = (GameObject)Instantiate(r_Hint.gameObject);
		hintClone.name = r_Hint.name + m_LabelCount;
		hintClone.transform.parent = m_Table.transform;
		hintClone.transform.localScale = Vector3.one;
		
		m_Table.Reposition();

		HintObject hintObject = hintClone.GetComponent<HintObject>();
		hintObject.m_Title.text = "[66FA33]["+m_LabelCount+"][-]"+hintsText.m_Title;
		hintObject.m_Description.text = hintsText.m_Description;

		m_LabelCount++;
		m_Table.Reposition();
	}
}
