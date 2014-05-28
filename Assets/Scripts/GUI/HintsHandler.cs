using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//peter, Anton Thorsell, arvid

/// <summary>
/// HintsHandler 
/// </summary>

[System.Serializable]
public class HintsText{
	[SerializeField][Multiline]
	public string mTitle;
	[SerializeField][Multiline]
	public string mText;
}
public class HintsHandler : MonoBehaviour {
	public List<HintsText> m_Hints = new List<HintsText>();

	void Awake() {
		Messenger.AddListener<HintsWindow>("add hints", addHints);
	}

	void addHints(HintsWindow window) {
		foreach(HintsText hintsText in m_Hints) {
			window.addHint(hintsText);
		}
		window.m_Table.Reposition();
		window.m_Table.repositionNow = true;
	}
}
