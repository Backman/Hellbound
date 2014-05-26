using UnityEngine;
using System.Collections;

/// <summary>
/// Notes logic.
/// This scirpt handles the printing of text to the small
/// paper notes that can be found throughout the game.
/// 
/// The notes are displayed as UISprites with a UILabel
/// ontop of them.
/// 
/// Created by Simon Jonasson
/// </summary>
public class NotesLogic : MonoBehaviour
{
	public UISprite m_ArrowUp;
	public UISprite m_ArrowDown;
	UILabel m_TextLabel;
	UIPlayTween m_Tweener;
	UIScrollView m_ScrollView;

	bool    m_Tweening = false;
	bool    m_Showing  = false;
	bool m_ButtonDown = false;

	bool m_TextToLarge = false;
	void Awake(){
		m_ScrollView = GetComponentInChildren<UIScrollView>();
		m_TextLabel = GetComponentInChildren<UILabel>();
		if( m_TextLabel == null ){
			Debug.LogError("Error. No NoteLabel found!");
			Debug.Break();
		}
		
		UIDragScrollView view;
		m_Tweener = gameObject.GetComponent<UIPlayTween>();		
	}

	public void showNote(MyGUI.NoteSettings noteSettings){
		if( !m_Showing ){
			m_Showing = true;
			StartCoroutine("showNote_CR", noteSettings);
		}
	}
	
	public void doneTweening(){ 
		m_Tweening = false; 
	}

	/// <summary>
	/// Tweens the note sprite upwards, displays the text and
	/// then waits for the user to press "Use" do hide the note
	/// </summary>
	IEnumerator showNote_CR(MyGUI.NoteSettings noteSettings){
		Messenger.Broadcast("lock player input", true );
		m_Tweening = true;
		m_TextLabel.text = noteSettings.text;
		if(m_TextLabel.height > (int)m_ScrollView.panel.height)
		{
			m_TextToLarge = true;
		}
		m_Tweener.Play( true );
		m_ScrollView.ResetPosition();
		m_ArrowDown.enabled = false;
		m_ArrowUp.enabled = false;
		while(m_Tweening){	yield return null;	}

		yield return StartCoroutine( awaitInput( "Use" ) );

		m_Tweening = true;
		m_Tweener.Play(true);
		while(m_Tweening){	yield return null;	}
		Messenger.Broadcast("lock player input", false );
		m_TextLabel.text = "";
		m_ScrollView.ResetPosition();
		m_Showing = false;
		m_TextToLarge = false;
	}

	IEnumerator awaitInput( string button){
		float value = 0.0f;
		while( true ){
			if(m_TextToLarge) {
				float scroll = Input.GetAxis("Mouse ScrollWheel");
				float vertical = Input.GetAxis ("Vertical");
				
				value += -scroll * m_ScrollView.scrollWheelFactor;
				value += -vertical * Time.deltaTime * 0.15f;
				if(value >= 1.0f) {
					m_ArrowDown.enabled = true;
					m_ArrowUp.enabled = false;
					value = 1.0f;
				} else if(value <= 0.0f) {
					m_ArrowUp.enabled = true;
					m_ArrowDown.enabled = false;
					value = 0.0f;
				} else {
					m_ArrowDown.enabled = true;
					m_ArrowUp.enabled = true;
				}
				m_ScrollView.SetDragAmount (0.0f, value, false);
			}

			if( Input.GetButtonDown(button) ){
				break;
			}
			yield return null;
		}
	}
}

