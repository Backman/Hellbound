using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Hellbound/HUD/Text field")]
public class Conversation : MonoBehaviour {

	private UISprite m_Sprite;
	private UILabel m_Title, m_Text;
	
	// Use this for initialization
	void Start () {
		m_Sprite = gameObject.GetComponent<UISprite>();
		m_Sprite.alpha = 1.0f;
		
		m_Title = GameObject.FindGameObjectWithTag("ConversationTitle").GetComponent<UILabel>();
		m_Title.text = "Priest";
		m_Title.alignment = NGUIText.Alignment.Left;
		
		m_Text = GameObject.FindGameObjectWithTag("ConversationText").GetComponent<UILabel>();
		m_Text.alpha = 1.0f;
		//m_Text.text = "This is some random textadsadsadsadsadsa";
		m_Text.alignment = NGUIText.Alignment.Left;
		
		//ProgressText prog = GameObject.FindGameObjectWithTag("ConversationText").GetComponent<ProgressText>();
		//prog.setText(m_Text.text);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void setTitle(string title){
		if(title == ""){
			m_Title.alpha = 0.0f;
		}
		else{
			m_Title.alpha = 1.0f;
		}
		m_Title.text = title;
	}
	
	public void setText(string text){
	/*
		ProgressText prog = GameObject.FindGameObjectWithTag("ConversationText").GetComponent<ProgressText>();
		int offset = 0;
		for(int i = 0; i < text.Length; ++i){
			if(offset >= 23){
				text = text.Substring(0, i) + "\n" + text.Substring(i, text.Length - i);
				offset = 0;
			}
			else{
				++offset;
			}
		}
		m_Text.text = text;
		prog.setText(text);
		*/
	}
	
	public void setVisible(bool visible){
		if(visible){
			m_Sprite.alpha = 1.0f;
		}
		else{
			m_Sprite.alpha = 0.0f;
		}
	}
}
