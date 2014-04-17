﻿/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

[System.Serializable]
struct DialogSettings{
	[SerializeField] [Range (0, 1)]
	public float m_WindowFadeTime = 0.0f;
	[SerializeField] [Range (0, 0.1f)]
	public float m_TextSpeed = 0.0f;
	[SerializeField] [Range (0, 1)]
	public float m_NewLineWait = 0.0f;
	[SerializeField]
	public bool m_doLinePadding = false;
}

/// <summary>
/// This class handles the GUI window that displays text messages
/// to the player as the player for example examines items.
/// 
/// Created by Simon
/// </summary>
public class GUIManager : Singleton<GUIManager> {
	private bool m_Busy = false;
	private bool m_QuickSkip = false;


	private UILabel r_CurrentLabel;
	private UISprite r_NextSprite;

    [SerializeField]
    private GameObject r_PauseWindow;
	[SerializeField]
	private UISprite r_ExamineWindow;
	[SerializeField]
	private UILabel[] r_ExamineLabels; 

	[SerializeField] [Range (0, 1)]
	public float m_WindowFadeTime = 0.0f;
	[SerializeField] [Range (0, 0.1f)]
	public float m_TextSpeed = 0.0f;
	[SerializeField] [Range (0, 1)]
	public float m_NewLineWait = 0.0f;
	[SerializeField]
	public bool m_doLinePadding = false;

    private bool m_GamePaused = false;

	public void Start(){
		if( r_ExamineWindow == null ){
			Debug.LogError("Error! No description window present!");
		}
		Transform t = r_ExamineWindow.transform.Find("NextSprite");
		if( t == null ){
			Debug.LogError("Error! No sprite for the 'next' icon present!");
		} else {
			r_NextSprite = t.GetComponent<UISprite>();
		}
		foreach ( UILabel label in r_ExamineLabels ){
			if( label == null ){
				Debug.LogError("Error! A label reference is invalid!");
			} else {
				label.text = "";
			}
		}

		r_NextSprite.alpha = 0.0f;
		r_ExamineWindow.alpha = 0.0f;
		r_ExamineWindow.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
	
		
	}

	public void showSubtitles( MyGUI.SubtitleSettings subtitles ){

	}

	/// <summary>
	/// This function will display the passed text in a small box
	/// at the lower part of the screen. All formating of the text
	/// is handled internaly.
	/// 
	/// This will lock the players movement and ability to rotate the camera
	/// </summary>
	public void simpleShowTextLockMovement(string text){
		Messenger.Broadcast("lock player input", true );
		simpleShowText( text );
	}

	/// <summary>
	/// This function will display the passed text in a small box
	/// at the lower part of the screen. All formating of the text
	/// is handled internaly.
	/// 
	/// This will not lock the players movement
	/// </summary>
	public void simpleShowText(string text){
		if( !m_Busy ){
			m_Busy = true;	

			object[] args = new object[4];
			args[0] = text;
			args[1] = true;
			args[2] = "awaitInput";
			args[3] = "Fire2";
			StartCoroutine("simpleShowText_Manual", args);
		} 
	}

	/// <summary>
	/// This function will display the passed text in a small box
	/// at the lower part of the screen. All formating of the text
	/// is handled internaly.
	/// 
	/// The text will automatically scroll without the players
	/// involvement. The player can move while the text is displayed.
	/// </summary>
	public void simpleShowTextAutoScroll( string text, float scollSpeed){
		if( !m_Busy ){
			m_Busy = true;	
			
			object[] args = new object[4];
			args[0] = text;
			args[1] = false;
			args[2] = "awaitTime";
			args[3] = scollSpeed;
			StartCoroutine("simpleShowText_Manual", args);
		} 
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            m_GamePaused = !m_GamePaused;
            Messenger.Broadcast<bool>("lock player input", m_GamePaused);
            pauseGame(m_GamePaused);
        }
    }

    public void pauseGame(bool pause) {
		
        if (pause) {
            r_PauseWindow.SetActive(true);
            PauseMenu.getInstance().showInventory();
			r_PauseWindow.GetComponent<UIPlayTween>().Play(true);
		} else {
			r_PauseWindow.GetComponent<UIPlayTween>().Play(false);
        }
        
    }
	
	#region Examine Monologue Coroutines

	IEnumerator simpleShowText_Manual(object[] args){
		m_QuickSkip = false;

		yield return StartCoroutine( "showWindow" );

		if( (bool)args[1] == true ){
			StartCoroutine ("listenForQuickSkip", args[3]);
		}

		yield return StartCoroutine( "feedText", args );	
		yield return StartCoroutine( "hideWindow" );

		StopCoroutine( "feedText" );
		StopCoroutine( "feedLine" );
		StopCoroutine( "listenForQuickSkip" );
		
		foreach( UILabel label in r_ExamineLabels ){
			label.text = "";
		}

		Messenger.Broadcast("lock player input", false );
		m_Busy = false;

	}

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
		Debug.Log( args[0] );
		foreach( UILabel label in r_ExamineLabels ){
			if( words.Count == 0 ){
				break;
			}
			r_CurrentLabel = label;
			string line = getLine( words, r_CurrentLabel );
			
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
				string line = getLine( words, r_CurrentLabel );
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
		
		Debug.Log("Done printing text");
	}
	
	/// <summary>
	/// Returns a line composed of the words in the stack that would
	/// fit into the label.
	/// The line is returned when the next word wouldn't fit or the last poped
	/// word was a newline.
	/// 
	/// The stack is requiered to have all characters and all whitespaces in
	/// separate elements. The labels text must be blank.
	/// </summary>
	private string getLine( Stack<string> words, UILabel targetLabel ){
		string line = "";
		string currWord = "";
		Vector2 labelSize = new Vector2( targetLabel.width, targetLabel.height );
		Vector2 textSize  = new Vector2();
		targetLabel.UpdateNGUIText();
		
		//Add next word to the current line as long as the line would fit in the label
		//and not cause a newline.
		while( words.Count > 0 ){
			currWord = words.Peek();
			textSize = NGUIText.CalculatePrintedSize(line + currWord);
			
			if( textSize.y > labelSize.y ){	
				//Check if the current word is a whitespace. If it is, remove it
				if( currWord.Trim() == string.Empty ){
					words.Pop();
					line.Trim();
				}
				textSize = NGUIText.CalculatePrintedSize(line + " ");
				while( textSize.y < labelSize.y && m_doLinePadding ){
					line += " ";
					textSize = NGUIText.CalculatePrintedSize(line + " ");
				}
				return line;
			}
			line += words.Pop();
		}
		
		return line;
	}

	#endregion

	#region General Coroutines
	/// <summary>
	/// Causes the text box to appera smoothly.
	/// </summary>
	IEnumerator showWindow(){
		Color col = new Color();
		Vector3 scale = new Vector3();
		while( r_ExamineWindow.color.a < 0.99f ){
			col = r_ExamineWindow.color;
			col.a +=  Time.deltaTime / m_WindowFadeTime;
			r_ExamineWindow.color = col; 
			
			scale = r_ExamineWindow.transform.localScale;
			scale.y += Time.deltaTime / m_WindowFadeTime;
			r_ExamineWindow.transform.localScale = scale;
			
			yield return null;
		}
		col.a = 1.0f;
		scale = Vector3.one;
		r_ExamineWindow.color = col;		
		r_ExamineWindow.transform.localScale = scale;
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
	
	/// <summary>
	/// Hides the window in a smooth way
	/// </summary>
	IEnumerator hideWindow(){
		Color col = new Color();
		Vector3 scale = new Vector3();
		while( r_ExamineWindow.color.a > 0.01f ){
			col = r_ExamineWindow.color;
			col.a -=  Time.deltaTime / m_WindowFadeTime;
			r_ExamineWindow.color = col; 
			
			scale = r_ExamineWindow.transform.localScale;
			scale.y -= Time.deltaTime / m_WindowFadeTime;
			r_ExamineWindow.transform.localScale = scale;
			
			yield return null;
		}
		col.a = 0.0f;
		scale.y = 0.0f;
		
		r_ExamineWindow.color = col;
		r_ExamineWindow.transform.localScale = scale;
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
			if( Input.GetButtonDown(button) ){
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
	/// Will wait for waitTime seconds before returning
	/// </summary>
	IEnumerator awaitTime( float waitTime ){
		yield return new WaitForSeconds( waitTime );
	}	
	#endregion
}*/
