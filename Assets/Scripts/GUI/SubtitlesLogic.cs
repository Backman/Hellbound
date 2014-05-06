using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// Subtitle behaviour.
/// All the logic for the subtitles system
/// 
/// Created by Simon
/// </summary>
public class SubtitlesLogic: MonoBehaviour {
	
	private UILabel[] r_SubtitlesLabels;
	private UILabel   r_CurrentLabel;
	private float 	  m_Timer;
	private Object	  m_Lock = new Object();

	private float Timer { 
		get{ 
			float f;
			lock(m_Lock){ f = m_Timer;	}
			return f;
		}
		set{
			lock( m_Lock ) { m_Timer = value ; }
		}
	}

	
	public void initialize( UILabel[] subtitleseLabels ){
		if( subtitleseLabels.Length == 0 ){
			Debug.LogError("No subtitle labels passed in SubtitleBehaviours init");
		}
		r_SubtitlesLabels = subtitleseLabels;
	}
	
	
	public IEnumerator showSubtitles(object[] args){

		StartCoroutine( "feedText", args );
		yield return StartCoroutine( "awaitTime", (float) args[1] );
		
		StopCoroutine( "feedText" );
		StopCoroutine( "feedLine" );
	}

	public IEnumerator clearLables(){
		StopCoroutine( "feedText" );
		StopCoroutine( "feedLine" );
		foreach( UILabel label in r_SubtitlesLabels ){
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
	/// Args[4] = (bool) Line Padding
	/// Args[5] = (bool) Using sounds (true if acceptable path found)
	/// Args[6] = (string) SoundPath
	/// Args[7] = (vector3) SoundPosition
	/// </summary>
	IEnumerator feedText( object[] args ){
		//Place all words in a stack
		string[] w = Regex.Split( (string)args[0] , @"(\s)" ); 
		Stack<string> words = new Stack<string>();
		foreach( string word in w.Reverse<string>()){
			words.Push(word);
		}
		object[] feedLineArgs = new object[2];
		feedLineArgs[1] = args[3];

		if ((bool)args[5]) {
			FMOD_StudioSystem.instance.PlayOneShot((string)args[6], (Vector3)args[7]);
		}

		foreach( UILabel label in r_SubtitlesLabels ){
			if( words.Count == 0 ){
				break;
			}

			r_CurrentLabel = label;
			feedLineArgs[0] = MyGUI.Tools.getLine( words, r_CurrentLabel, (bool) args[4]);

			yield return StartCoroutine( "feedLine", feedLineArgs);

		}
		
		yield return StartCoroutine( (string) args[2], args[3]);
		#pragma warning disable CS0414
		while( words.Count > 0 ){
			foreach( UILabel l in r_SubtitlesLabels ){
				if( words.Count == 0 ){
					break;
				}
				for( int i = 0; i < r_SubtitlesLabels.Count() - 1; ++i ){
					r_SubtitlesLabels[i].text = r_SubtitlesLabels[i+1].text;
				}	
				r_CurrentLabel.text = "";
				feedLineArgs[0] = MyGUI.Tools.getLine( words, r_CurrentLabel, (bool) args[4]);
		
				yield return StartCoroutine( "feedLine", feedLineArgs);
			}
			yield return StartCoroutine( (string) args[2], args[3]);
		}
		#pragma warning restore CS0414

	}

	/// <summary>
	/// Will wait for waitTime seconds before returning
	/// </summary>
	IEnumerator awaitTime( float waitTime ){
		yield return new WaitForSeconds( waitTime );
	}	
	
	/// <summary>
	/// Displays the actual line of text to the player, character by character
	/// </summary>
	IEnumerator feedLine( object[] args ){
		string line = (string) args[0];
		float textSpeed = (float) args[1];
		for( int i = 0; i < line.Length; ++i){

			r_CurrentLabel.text += line[i];
			yield return new WaitForSeconds(textSpeed);

		}
	}
	#endregion
	
}
