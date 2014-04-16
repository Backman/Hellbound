﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// This class handles the GUI window that displays text messages
/// to the player as the player for example examines items.
/// 
/// Created by Simon
/// </summary>
public class GUIManager : Singleton<GUIManager> {
    [SerializeField]
    private GameObject r_PauseWindow;
	[SerializeField]
	private UISprite r_ExamineWindow;
	[SerializeField]
	private UISprite r_SubtitlesWindow;

	////////////////////////////////////////////////
	[SerializeField] [Range (0, 1)]				  //	
	public float m_WindowFadeTime = 0.0f;		  //
	[SerializeField] [Range (0, 0.1f)]			  //
	public float m_ExamineTextSpeed = 0.0f;		  //
	[SerializeField] [Range (0, 1)]				  //
	public float m_ExamineNewLineWait = 0.0f;	  //
	[SerializeField]							  //
	public bool m_ExamineDoLinePadding = false;	  //
	////////////////////////////////////////////////

    private bool m_GamePaused = false;
	private bool m_Examining = false;
	private bool m_SubtitlesDisplayed = false;

	private ExamineBehaviour   m_Examine;
	private SubtitlesBehaviour m_Subtitles;

	private UILabel[] r_ExamineLabels;
	private UILabel[] r_SubtitlesLables;

	public void Start(){
		if( r_ExamineWindow == null ){
			Debug.LogError("Error! No description window present!");
		} 
		else {
			r_ExamineWindow.alpha = 0.0f;
			r_ExamineWindow.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
			initExamineWindow();
		}

		if( r_SubtitlesWindow == null ){
			Debug.LogError("Error! No subtitles window present!");
		} 
		else {
			r_SubtitlesWindow.alpha = 0.0f;
			r_SubtitlesWindow.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
			initSubtitleWindow();
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

	/// <summary>
	/// Will display a window with the supplied text.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
	public void simpleShowText(string text, bool lockMovement){
		if( !m_Examining ){
			m_Examining = true;
			object[] args = new object[5];
			args[0] = text;	
			args[1] = lockMovement;
			args[2] = "awaitInput";		//Method for making text advance
			args[3] = "Fire2";
			args[4] = false;

			StartCoroutine("examine", args);
		}
		else {
			Debug.Log("Bussy examining");
		}
	}

	/// <summary>
	/// Shows the subtitles.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="displayTime">Display time.</param>
	/// <param name="textSpeed">Text speed.</param>
	/// <param name="doLinePadding">If set to <c>true</c> do line padding.</param>
	public void showSubtitles( MyGUI.SubtitlesSettings[] subtitles ){

			StartCoroutine("subtitles", subtitles);

	}
	#region private functions
	private void initExamineWindow(){
		//Fetch the "NextSprite" among the ExaminWindows children
		Transform t = r_ExamineWindow.transform;
		UISprite sprite = t.FindChild("NextSprite").GetComponent<UISprite>();
		sprite.alpha = 0.0f;
		
		//Fetch the "ExamineTextLabels" among the ExamineWindows children, then
		//sort them
		r_ExamineLabels = t.GetComponentsInChildren<UILabel>();
		r_ExamineLabels = r_ExamineLabels.OrderBy( x => x.name ).ToArray();
		
		m_Examine = gameObject.GetComponent<ExamineBehaviour>();
		m_Examine.initialize( r_ExamineLabels, sprite, m_ExamineTextSpeed,
		                     m_ExamineNewLineWait, m_ExamineDoLinePadding );
	}

	private void initSubtitleWindow(){
		Transform t = r_SubtitlesWindow.transform;
		
		//Fetch the "ExamineTextLabels" among the ExamineWindows children, then
		//sort them
		r_SubtitlesLables = t.GetComponentsInChildren<UILabel>();
		r_SubtitlesLables = r_SubtitlesLables.OrderBy( x => x.name ).ToArray();
		
		m_Subtitles = gameObject.GetComponent<SubtitlesBehaviour>();
		m_Subtitles.initialize( r_SubtitlesLables );
	}
	#endregion


	IEnumerator examine(object[] args){

		if( (bool) args[1] ){
			Messenger.Broadcast("lock player input", true );
		}

		r_SubtitlesWindow.GetComponent<TweenPosition>().PlayForward();

		StartCoroutine( m_Examine.clearLables() );
		yield return StartCoroutine( "showWindow", r_ExamineWindow );
		yield return StartCoroutine( m_Examine.showText( args ) );	
		yield return StartCoroutine( "hideWindow", r_ExamineWindow );
		
		Messenger.Broadcast("lock player input", false );

		m_Examining = false;
		r_SubtitlesWindow.GetComponent<TweenPosition>().PlayReverse();
	}

	IEnumerator subtitles(MyGUI.SubtitlesSettings[] subtitles){

		object[] args = new object[5];
		StartCoroutine( m_Subtitles.clearLables() );

		yield return StartCoroutine( "showWindow", r_SubtitlesWindow );	
		foreach( MyGUI.SubtitlesSettings subtitle in subtitles ){
			args[0] = subtitle.Text;	
			args[1] = subtitle.DisplayTime;
			args[2] = "awaitTime";		//Method for making text advance
			args[3] = subtitle.TextSpeed;
			args[4] = false;


			yield return StartCoroutine( m_Subtitles.showSubtitles( args ) );
			StartCoroutine( m_Subtitles.clearLables() );

		}
		yield return StartCoroutine( "hideWindow", r_SubtitlesWindow );

	}

	#region General Coroutines
	/// <summary>
	/// Causes the text box to appera smoothly.
	/// </summary>
	IEnumerator showWindow(UISprite window){
		Color col = new Color();
		Vector3 scale = new Vector3();
		while( window.color.a < 0.99f ){
			col = window.color;
			col.a +=  Time.deltaTime / m_WindowFadeTime;
			window.color = col; 
			
			scale = window.transform.localScale;
			scale.y += Time.deltaTime / m_WindowFadeTime;
			window.transform.localScale = scale;
			
			yield return null;
		}
		col.a = 1.0f;
		scale = Vector3.one;
		window.color = col;		
		window.transform.localScale = scale;
	}
	
	/// <summary>
	/// Hides the window in a smooth way
	/// </summary>
	IEnumerator hideWindow( UISprite window){
		Color col = new Color();
		Vector3 scale = new Vector3();
		while( window.color.a > 0.01f ){
			col = window.color;
			col.a -=  Time.deltaTime / m_WindowFadeTime;
			window.color = col; 
			
			scale = window.transform.localScale;
			scale.y -= Time.deltaTime / m_WindowFadeTime;
			window.transform.localScale = scale;
			
			yield return null;
		}
		col.a = 0.0f;
		scale.y = 0.0f;
		
		window.color = col;
		window.transform.localScale = scale;
	}
	#endregion
}
