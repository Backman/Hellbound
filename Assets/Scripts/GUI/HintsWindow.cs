using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//arvid

public class HintsWindow : MonoBehaviour {
	private int m_LabelCount;
	private bool m_Init = false;
	private string m_CurrLevel = "";

	public UITable m_Table;
	public HintObject r_Hint;

	//void Awake() {
	//	r_Hint = m_Table.GetComponentInChildren<HintObject>();
	//}

	void OnEnable() {
		if( m_CurrLevel != Application.loadedLevelName ){
			m_Init = false;
			m_CurrLevel = Application.loadedLevelName;
		}
	}

	void LateUpdate(){
		if( !m_Init ){
			m_Init = true;
			clearHints();
			Messenger.Broadcast<HintsWindow>("add hints", this);
		}
	}

	private void clearHints(){
		var t = m_Table.children;
		foreach( Transform trans in t ){
			Destroy( trans.gameObject );
		}
		m_LabelCount = 0;
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
