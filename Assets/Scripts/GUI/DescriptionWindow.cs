using UnityEngine;
using System.Collections;

public class DescriptionWindow : MonoBehaviour {

	public UILabel r_DescriptionLabel;

	public string Text {
		get { return r_DescriptionLabel.text; }
		set { r_DescriptionLabel.text = value; }
	}

	// Use this for initialization
	void Start () {
		Messenger.AddListener("reset pause window", reset);
		Messenger.AddListener<string>("set description text", setDescription);
	}

	public void reset() {
		Text = "";
	}

	public void setDescription(string text) {
		Text = text;
	}
}
