using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class GUIManager : Singleton<GUIManager> {
	private bool m_Busy = false;
	private bool m_QuickSkip = false;
	private UILabel r_CurrentLabel;

	[SerializeField]
	private UISprite r_DescriptionWindow;
	[SerializeField]
	private UILabel[] r_DescriptionLabels;

	[SerializeField] [Range (0, 1)]
	private float m_WindowFadeTime = 0.0f;
	[SerializeField] [Range (0, 0.1f)]
	private float m_TextSpeed = 0.0f;
	[SerializeField] [Range (0, 1)]
	private float m_NewLineWait = 0.0f;
	[SerializeField]
	private bool m_doLinePadding = false;

	public void Awake(){
		if( r_DescriptionWindow == null ){
			Debug.LogError("Error! No description window present!");
		}
		foreach ( UILabel label in r_DescriptionLabels ){
			if( label == null ){
				Debug.LogError("Error! A label reference is invalid!");
			} else {
				label.text = "";
			}
		}
		r_DescriptionWindow.alpha = 0.0f;
		r_DescriptionWindow.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
	}

	public void simpleShowText(string text){
		if( !m_Busy ){
			m_Busy = true;
			StartCoroutine("simpleShowText_CR", text);
		} else {
			Debug.Log("GUI manager is busy at the moment");
		}
	}
	#region Coroutines
	IEnumerator simpleShowText_CR(string text){
		//TODO: Lock controllers
		yield return StartCoroutine( "showWindow" );

		StartCoroutine ("listenForQuickSkip");

		yield return StartCoroutine( "feedText", text );
			
		yield return StartCoroutine( "hideWindow" );
		StopCoroutine( "feedText" );
		StopCoroutine( "feedLine" );
		StopCoroutine( "listenForQuickSkip" );

		foreach( UILabel label in r_DescriptionLabels ){
			label.text = "";
		}

		m_Busy = false;
		//TODO: Unlock controllers
	}

	IEnumerator showWindow(){
		Color col = new Color();
		Vector3 scale = new Vector3();
		while( r_DescriptionWindow.color.a < 0.99f ){
			col = r_DescriptionWindow.color;
			col.a +=  Time.deltaTime / m_WindowFadeTime;
			r_DescriptionWindow.color = col; 
				
			scale = r_DescriptionWindow.transform.localScale;
			scale.y += Time.deltaTime / m_WindowFadeTime;
			r_DescriptionWindow.transform.localScale = scale;

			yield return null;
		}
		col.a = 1.0f;
		scale = Vector3.one;
		r_DescriptionWindow.color = col;		
		r_DescriptionWindow.transform.localScale = scale;
	}

	IEnumerator feedText( string text ){
		//Place all words in a stack
		string[] w = Regex.Split( text, @"(\s)" ); 
		Stack<string> words = new Stack<string>();
		foreach( string word in w.Reverse<string>()){
			words.Push(word);
		}

		m_QuickSkip = false;
		foreach( UILabel label in r_DescriptionLabels ){
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

		yield return StartCoroutine("awaitInput", "Fire2");
		m_QuickSkip = false;

		while( words.Count > 0 ){
			foreach( UILabel l in r_DescriptionLabels ){
				if( words.Count == 0 ){
					break;
				}
				for( int i = 0; i < r_DescriptionLabels.Count() - 1; ++i ){
					r_DescriptionLabels[i].text = r_DescriptionLabels[i+1].text;
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
			yield return StartCoroutine("awaitInput", "Fire2" );
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

	IEnumerator hideWindow(){
		Color col = new Color();
		Vector3 scale = new Vector3();
		while( r_DescriptionWindow.color.a > 0.01f ){
			col = r_DescriptionWindow.color;
			col.a -=  Time.deltaTime / m_WindowFadeTime;
			r_DescriptionWindow.color = col; 

			scale = r_DescriptionWindow.transform.localScale;
			scale.y -= Time.deltaTime / m_WindowFadeTime;
			r_DescriptionWindow.transform.localScale = scale;

			yield return null;
		}
		col.a = 0.0f;
		scale.y = 0.0f;

		r_DescriptionWindow.color = col;
		r_DescriptionWindow.transform.localScale = scale;
	}

	IEnumerator listenForQuickSkip(){
		while( true ){
			if( Input.GetButtonDown("Fire2") ){
				m_QuickSkip = true;
			}
			yield return null;
		}
	}

	IEnumerator awaitInput(string button){
		yield return null;
		while( true ){
			if( Input.GetButtonDown(button) ){
				break;
			}
			yield return null;
		}
		yield return null;
	}

	#endregion
}
