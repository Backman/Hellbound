using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class GUIManager : Singleton<GUIManager> {
	private bool m_Busy = false;

	[SerializeField]
	private UISprite r_DescriptionWindow;
	[SerializeField]
	private UILabel[] r_DescriptionLabels;

	[SerializeField] [Range (0, 1)]
	private float m_WindowFadeTime = 0.0f;
	[SerializeField]
	private float m_TextSpeed = 0.0f;
	private UILabel r_CurrentDescriptionLabel;
	public void Awake(){
		if( r_DescriptionWindow == null ){
			Debug.LogError("Error! No description window present!");
		}
		if( r_DescriptionLabels[0] == null ){
			Debug.LogError("Error! No description label present!");
		}
		r_DescriptionWindow.alpha = 0.0f;
		r_DescriptionWindow.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
		r_CurrentDescriptionLabel = r_DescriptionLabels[0];

		foreach( UILabel label in r_DescriptionLabels ){
			label.text = "";
		}
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
		StartCoroutine( "feedText", text );
		//TODO: Unlock controllers
	
		yield return StartCoroutine ("awaitInput", "Fire1");
	
		//TODO: Lock controllers			
		yield return StartCoroutine( "hideWindow" );
		StopCoroutine( "feedText" );
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

	IEnumerator awaitInput(string button){
		while( true ){
			if( Input.GetButtonDown(button) )
				break;
			yield return null;
		}
	}

	IEnumerator feedText( string text ){
		//Place all words in a stack
		string[] w = Regex.Split( text, @"(\s)" ); 
		Stack<string> words = new Stack<string>();
		foreach( string word in w.Reverse<string>()){
			words.Push(word);
		}

		int currLabel = 0;
		bool modified;

		//TODO: Write a getNextLine function!
		while( words.Count > 0 ){
			modified = false;
			string currWord = words.Pop();
			string nextWord = words.Peek();
			yield return StartCoroutine("feedWord", currWord);

			//These are used in calculating wheter the next word fits
			//in the current row or not
			r_DescriptionLabels[currLabel].UpdateNGUIText();
			Vector2 size = NGUIText.CalculatePrintedSize(nextWord);
			r_CurrentDescriptionLabel = r_DescriptionLabels[currLabel];

			if( currWord.Contains("\n") ){
				currLabel++;
				modified = true;
			}
			else if(r_CurrentDescriptionLabel.width <= size.x + r_CurrentDescriptionLabel.printedSize.x) {
				currLabel++;
				modified = true;

				if( string.IsNullOrEmpty( nextWord ) ){
					words.Pop();
					Debug.Log("Ate2: " + nextWord);
				}
			}

			//If the currLabel variable's been modified this word
			//we must handle it by either incrementing label nr or clearing old
			if( modified ){
				if( currLabel >= r_DescriptionLabels.Length ){
					yield return StartCoroutine("awaitInput", "Fire2");
					foreach( UILabel label in r_DescriptionLabels){
						label.text = "";
					}
					currLabel = 0;
				}

				r_CurrentDescriptionLabel = r_DescriptionLabels[currLabel];
			}
		}
	}

	IEnumerator feedWord( string text ){
		Debug.Log(text);
		for( int i = 0; i < text.Length; ++i){
			r_CurrentDescriptionLabel.text += text[i];
			yield return new WaitForSeconds(m_TextSpeed);
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
		foreach( UILabel label in r_DescriptionLabels ){
			label.text = "";
		}
		r_DescriptionWindow.color = col;
		r_DescriptionWindow.transform.localScale = scale;
		r_CurrentDescriptionLabel = r_DescriptionLabels[0];
	}
	#endregion
}
