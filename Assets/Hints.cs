using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HintsText {
	public string m_Title = "";
	public string m_Description = "";
}

public class Hints : MonoBehaviour {
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
