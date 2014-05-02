using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// Examine behaviour.
/// All logic for the Examine functions
/// 
/// Created by Simon
/// </summary>
public class ExamineLogic : MonoBehaviour {

	private UILabel[] r_ExamineLabels;
	private UISprite  r_NextSprite;
	private float 	  m_TextSpeed;
	private float 	  m_NewLineWait;
	private bool  	  m_DoLinePadding;

	private bool 	  m_QuickSkip = false;
	private UILabel   r_CurrentLabel;

	public void initialize( UILabel[] examineLabels, UISprite nextSprite, float textSpeed, 
	                  float newLineWait, bool doLinePadding ){
		if( examineLabels.Length == 0 ){
			Debug.LogError("No Examine labels passed in ExamineBehaviours init");
		}
		if( nextSprite == null ){
			Debug.LogError("No Next sprite passed in ExamineBehaviours init");
		}
		r_ExamineLabels = examineLabels;
		r_NextSprite = nextSprite;
		m_TextSpeed = textSpeed;
		m_NewLineWait = newLineWait;
		m_DoLinePadding = doLinePadding;
	}


	public IEnumerator showText(object[] args){

		m_QuickSkip = false;
		
		StartCoroutine ("listenForQuickSkip", args[3]);
		
		yield return StartCoroutine( "feedText", args );
		
		StopCoroutine( "feedText" );
		StopCoroutine( "feedLine" );
		StopCoroutine( "listenForQuickSkip" );
	}

	public IEnumerator clearLables(){
		foreach( UILabel label in r_ExamineLabels ){
			label.text = "";
		}
		yield return null;
	}

	#region Coroutines
	/// <summary>
	/// This function handles the logic about which line of text
	/// should be displayed to the player
	/// 
	/// Args[0] = (string) the text
	/// Args[1] = (bool)  allow quick skip
	/// Args[2] = (string) scroll method
	/// Args[3] = (obj) scroll method argument
	/// </summary>
	IEnumerator feedText( object[] args ){
		//Place all words in a stack
		string[] w = Regex.Split( (string)args[0] , @"(\s)" ); 
		Stack<string> words = new Stack<string>();
		foreach( string word in w.Reverse<string>()){
			words.Push(word);
		}
		foreach( UILabel label in r_ExamineLabels ){
			if( words.Count == 0 ){
				break;
			}
			r_CurrentLabel = label;
			string line = MyGUI.Tools.getLine( words, r_CurrentLabel, (bool) args[4] );
			
			if( m_QuickSkip ){
				label.text = line;
			} else {
				yield return StartCoroutine( "feedLine", line);
			}
		}
		
		yield return StartCoroutine( (string) args[2], args[3]);
		m_QuickSkip = false;
		
		while( words.Count > 0 ){
			foreach( UILabel l in r_ExamineLabels ){
				if( words.Count == 0 ){
					break;
				}
				for( int i = 0; i < r_ExamineLabels.Count() - 1; ++i ){
					r_ExamineLabels[i].text = r_ExamineLabels[i+1].text;
				}	
				r_CurrentLabel.text = "";
				string line = MyGUI.Tools.getLine( words, r_CurrentLabel, (bool) args[4] );
				if( m_QuickSkip ){
					r_CurrentLabel.text = line;
				} else {
					yield return StartCoroutine( "feedLine", line);
					yield return m_QuickSkip ? null : new WaitForSeconds( m_NewLineWait );
				}
			}
			yield return StartCoroutine( (string) args[2], args[3]);
			m_QuickSkip = false;
		}

	}

	/// <summary>
	/// This function detects if the player wishes to speed up
	/// the text display. IF the player clicks "Fire2" as the text
	/// is being printed, all text of that "frame" is displayed
	/// instantaniously
	/// </summary>
	/// <returns>The for quick skip.</returns>
	IEnumerator listenForQuickSkip(string button){
		while( true ){
			if( Input.GetButtonDown( button ) ){
				m_QuickSkip = true;
			}
			yield return null;
		}
	}
	
	/// <summary>
	/// This function locks further progress until the player pushes
	/// the button (that is passed as parameter)
	/// </summary>
	IEnumerator awaitInput( string button){
		yield return null;
		r_NextSprite.alpha = 1.0f;
		while( true ){
			if( Input.GetButtonDown(button) ){
				break;
			}
			yield return null;
		}
		yield return null;
		r_NextSprite.alpha = 0.0f;
	}

	/// <summary>
	/// Displays the actual line of text to the player, character by character
	/// </summary>
	IEnumerator feedLine( string line ){
		for( int i = 0; i < line.Length; ++i){
			if( m_QuickSkip ){
				r_CurrentLabel.text = line;
			} else {
				r_CurrentLabel.text += line[i];
				yield return new WaitForSeconds(m_TextSpeed);
			}
		}
	}
	#endregion

}
