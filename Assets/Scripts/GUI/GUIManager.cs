using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

[System.Serializable]
public class PauseWindow {
	public GameObject r_MainWindow;
	public GameObject r_InventoryWindow;
	public GameObject r_HintWindow;
	public GameObject r_SaveLoadWindow;
	public GameObject r_SettingsWindow;
}

/// <summary>
/// This class handles the GUI window that displays NGUI 
/// widgets to the player.
/// 
/// This class mostly acts as a relay which will
/// call functions further down in the hierarcy.
///
/// Created by Simon Jonasson
/// 
/// Modified by Peter, Arvid Backman
/// 
///  _minor_ editing by Anton Thorsell
/// (basically anything that have to do with sounds)
/// 
/// </summary>

public class GUIManager : Singleton<GUIManager> {
	public PauseWindow m_PauseWindow;
	[SerializeField]
	private GameObject r_PauseWindow;
	[SerializeField]
	private UISprite r_ExamineWindow;
	[SerializeField]
	private UISprite r_SubtitlesWindow;
	[SerializeField]
	private InteractText r_InteractText;

	private LoadingLogic r_LoadingLogic;
	private NotesLogic   r_NotesLogic;

	private Queue m_MonologeQueue = new Queue ();
	private bool WritingDialouge = false;

	////////////////////////////////////////////////
	[SerializeField] [Range (0, 1)]	//
	public float m_WindowFadeTime = 0.0f;	//
	[SerializeField] [Range (0, 0.1f)]	//
	public float m_ExamineTextSpeed = 0.0f;	//
	[SerializeField] [Range (0, 1)]	//
	public float m_ExamineNewLineWait = 0.0f;	//
	[SerializeField]	//
	public bool m_ExamineDoLinePadding = false;	//
	////////////////////////////////////////////////
	
	private bool m_GamePaused = false;
	public bool GamePaused {
		get { return m_GamePaused; }
	}
	private bool m_Examining = false;
	private bool m_SubtitlesDisplayed = false;
	private bool m_InventoryIsUp = false;
	
	private ExamineLogic m_Examine;
	private SubtitlesLogic m_Subtitles;
	
	private UILabel[] r_ExamineLabels;
	private UILabel[] r_SubtitlesLables;

	/// <summary>
	/// Controlls wether the inventoryWindow is currently tweening or not
	/// </summary>
	private bool m_InventoryTweening = false;
	private Camera r_MainCamera = null;

	public void Awake(){
		DontDestroyOnLoad( gameObject );
		
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

		r_LoadingLogic = GetComponentInChildren<LoadingLogic>();
		if( r_LoadingLogic == null ){
			Debug.LogError("Error. No loading logic found");
		}

		r_NotesLogic = GetComponentInChildren<NotesLogic>();
		if( r_NotesLogic == null ){
			Debug.LogError("Error! No notes logic found!");
		}
		r_MainCamera = Camera.main;
	}
	
	void Update() {
		if (Input.GetButtonDown("Pause")) {
			togglePause();
		}
		if (Input.GetButtonDown("Inventory") && !m_GamePaused && !m_InventoryTweening) {
			m_InventoryTweening = true;
			m_InventoryIsUp = !m_InventoryIsUp;
			inventory();
		}
	}

	public void togglePause(){
		m_GamePaused = !m_GamePaused;
		pauseGame(m_GamePaused);
		if (!m_GamePaused) {
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = null;
		} 
	}

	public void pauseGame(bool pause) {
		if (pause) {
			Screen.lockCursor = false;
			Screen.showCursor = true;
			Time.timeScale = 0.0000001f;
			fadePauseWindow(true);

		} else {
			Time.timeScale = 1.0f;
			Screen.lockCursor = true;
			Screen.showCursor = false;
			fadePauseWindow(false);

			Messenger.Broadcast("reset pause window");
		}

	}

	// Adds/Removes blur on the game view and shows/hides the PauseMenu UI widgets
	private void fadePauseWindow(bool show){
		if (show) m_PauseWindow.r_MainWindow.GetComponent<Menu>().show ();
		m_PauseWindow.r_MainWindow.GetComponent<UIPlayTween>().Play(show);
		if(!show) m_PauseWindow.r_MainWindow.GetComponent<Menu>().dontShow();

		if(r_MainCamera == null) {
			r_MainCamera = Camera.main;
		}
		PauseGameEffect pge = r_MainCamera.GetComponent(typeof( PauseGameEffect ) ) as PauseGameEffect;
		if( pge != null ){
			pge.StopCoroutine("pauseGame");
			pge.StartCoroutine("pauseGame", show);
			PauseMenu.getInstance().hideAll();	//If this row is removed, we will display hints when we open the pause menu the first time
		} else {
			Debug.LogError("Error! No PauseGameEffect present. Are you using the correct PlayerController?\nIf you are, the PlayerController prefab is blue while you are in edit mode, otherwize it is red");
		}
	}

	public void pauseExit(){
		togglePause ();
		loadLevel (0, "");
	}

	public void DestroyThis(){
		Destroy (gameObject);
	}
	
	public void doneTweening(){
		m_InventoryTweening = false;
	}
	
	public void inventory(){
		
		m_PauseWindow.r_InventoryWindow.GetComponent<UIPlayTween>().Play (true);
		if(m_InventoryIsUp) {
			m_PauseWindow.r_InventoryWindow.GetComponent<UIPlayTween>().tweenGroup = 1;
		}
		else {
			m_PauseWindow.r_InventoryWindow.GetComponent<UIPlayTween>().tweenGroup = 0;
		}
		
	}

	public void loadLevel( string levelName, string loadMessage ){
		r_LoadingLogic.loadLevel(levelName, loadMessage);
	}

	public void loadLevel( int sceneNumber, string loadMessage ){
		r_LoadingLogic.loadLevel(sceneNumber, loadMessage);
	}

	public void loadLastCheckPoint(string loadMessage) {
		r_LoadingLogic.loadLastCheckpoint(loadMessage);
	}

	/// <summary>
	/// Shows a simple textbox with the supplied text.
	/// The button-string dictates which button that closes the window. It is optional, defaults to Examine
	/// The lockMovement bool dictates if the players movement is locked while the text is visible, defaults to true
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="button">Button.</param>
	/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
	public bool simpleShowText(string text, string button = "Examine", bool lockMovement = true){
		if( !m_Examining ){
			m_Examining = true;
			object[] args = new object[5];
			args[0] = text;	
			args[1] = lockMovement;
			args[2] = "awaitInput";	//Method for making text advance
			args[3] = button;
			args[4] = false;
			
			StartCoroutine("examine", args);
			return true;
		}
		else {
			return false;
		}
	}

	public bool IsShowingText{ get { return m_Examining; } }
	
	/// <summary>
	/// Shows the subtitles.
	/// </summary>
	public void showSubtitles( MyGUI.SubtitlesSettings[] subtitles ){
		
		foreach (MyGUI.SubtitlesSettings sts in subtitles) {
			m_MonologeQueue.Enqueue(sts);
		}
		
		if (!WritingDialouge) {
			WritingDialouge = true;
			StartCoroutine("subtitles", subtitles);
		}
		
	}
	
	public void setupInteractionTexts( string examineText, string useText ){
		r_InteractText.setupInteractionTexts( examineText, useText );
	}
	
	public void interactTextActive( bool status ){
		r_InteractText.active( status );
	}

	public void showNote( MyGUI.NoteSettings noteSettings ){
		r_NotesLogic.showNote( noteSettings );
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
		
		m_Examine = gameObject.AddComponent<ExamineLogic>();
		m_Examine.initialize( r_ExamineLabels, sprite, m_ExamineTextSpeed,
		                     m_ExamineNewLineWait, m_ExamineDoLinePadding );
	}
	
	private void initSubtitleWindow(){
		Transform t = r_SubtitlesWindow.transform;
		
		//Fetch the "ExamineTextLabels" among the ExamineWindows children, then
		//sort them
		r_SubtitlesLables = t.GetComponentsInChildren<UILabel>();
		r_SubtitlesLables = r_SubtitlesLables.OrderBy( x => x.name ).ToArray();
		
		m_Subtitles = gameObject.AddComponent<SubtitlesLogic>();
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
		
		object[] args = new object[8];
		StartCoroutine( m_Subtitles.clearLables() );
		
		yield return StartCoroutine( "showWindow", r_SubtitlesWindow );	
		
		while(m_MonologeQueue.Count != 0){
			
			MyGUI.SubtitlesSettings useThis = (MyGUI.SubtitlesSettings)(m_MonologeQueue.Dequeue());
			
			args[0] = useThis.Text;	
			args[1] = useThis.DisplayTime;
			args[2] = "awaitTime";	//Method for making text advance
			args[3] = useThis.TextSpeed;
			args[4] = false;
			
			if(useThis.SoundPath != "" && useThis.SoundPath != "event:/"){
				args[5] = true;
				args[6] = useThis.SoundPath;
				if(useThis.SoundPosition != null){
					args[7] = useThis.SoundPosition;
				}
			}
			else{ 
				args[5] = false;
			} 
			
			yield return StartCoroutine( m_Subtitles.showSubtitles( args ) );
			StartCoroutine( m_Subtitles.clearLables() );
			
			
		}
		WritingDialouge = false;
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
			col.a += Time.deltaTime / m_WindowFadeTime;
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
			col.a -= Time.deltaTime / m_WindowFadeTime;
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