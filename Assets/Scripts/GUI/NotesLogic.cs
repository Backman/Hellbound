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
/// Created by Simon
/// </summary>
public class NotesLogic : MonoBehaviour
{
	UILabel m_TextLabel;
	UIPlayTween m_Tweener;

	bool    m_Tweening = false;
	bool    m_Showing  = false;

	void Awake(){
		m_TextLabel = GetComponentInChildren<UILabel>();
		if( m_TextLabel == null ){
			Debug.LogError("Error. No NoteLabel found!");
			Debug.Break();
		}

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
		m_Tweener.Play( true );
		while(m_Tweening){	yield return null;	}

		yield return StartCoroutine( awaitInput( "Use" ) );

		m_Tweening = true;
		m_Tweener.Play(true);
		while(m_Tweening){	yield return null;	}
		Messenger.Broadcast("lock player input", false );
		m_TextLabel.text = "";
		m_Showing = false;
	}

	IEnumerator awaitInput( string button){
		while( true ){
			if( Input.GetButtonDown(button) ){
				break;
			}
			yield return null;
		}
	}
}

