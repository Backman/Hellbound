using UnityEngine;
using System.Collections;

/// <summary>
/// This scirpt handles all logic associated with loading levels and displaying loading screens.
/// 
/// Created by Simon Jonasson
/// </summary>
public class LoadingLogic : MonoBehaviour {

	UISprite  r_LoadingScreen;
	UITweener r_LoadingScreenTweener;

	UILabel   r_LoadingMessage;
	UITweener r_LoadingMessageTweener;
	private string loadingMessage { 
		set { r_LoadingMessage.text = value; }
	}

	// Use this for initialization
	void Start () {
		r_LoadingScreen = gameObject.GetComponent( typeof( UISprite ) ) as UISprite;
		if( r_LoadingScreen == null ){
			Debug.LogError("Unable to find LoadingScreen");
		} else {
			r_LoadingScreenTweener = r_LoadingScreen.GetComponent( typeof( UITweener) ) as UITweener;
			r_LoadingScreen.alpha = 1.0f;
			StartCoroutine("fadeIn");
		}

		r_LoadingMessage = r_LoadingScreen.GetComponentInChildren( typeof (UILabel) ) as UILabel;
		if( r_LoadingMessage == null ){
			Debug.LogError("Unable to find LoadingMessage label");
		} else {
			r_LoadingMessageTweener = r_LoadingMessage.GetComponent( typeof( UITweener ) ) as UITweener;
		}
	}

	IEnumerator fadeIn() {
		yield return new WaitForSeconds(0.5f);
		r_LoadingScreenTweener.PlayReverse();
		r_LoadingScreenTweener.ResetToBeginning();
		r_LoadingScreenTweener.PlayReverse();
	}

	IEnumerator fadeOut() {
		yield return new WaitForSeconds(0.5f);
		r_LoadingScreenTweener.PlayForward();
		r_LoadingScreenTweener.ResetToBeginning();
		r_LoadingScreenTweener.PlayForward();
	}

	public void loadLevel( int sceneNumber, string loadMessage ){
	

		object[] args = new object[2];
		args[0] = sceneNumber;
		args[1] = loadMessage;
		StartCoroutine( "loadLevelWithNumber_CR", args );

		Messenger.Cleanup ();
	}

	IEnumerator loadLevelWithNumber_CR( object[] args ){
		loadingMessage = (string) args[1];
		
		r_LoadingScreenTweener.PlayForward();
		r_LoadingMessageTweener.PlayForward();

		yield return new WaitForSeconds( r_LoadingScreenTweener.duration );

		bool hasPro = UnityEditorInternal.InternalEditorUtility.HasPro();
		if(hasPro) {
			AsyncOperation ao = Application.LoadLevelAsync((int)args[0]);

			yield return ao;

		} else {
			Application.LoadLevel( (int) args[0] );
		}

		yield return new WaitForSeconds(1.0f);		
		
		r_LoadingScreenTweener.PlayReverse();
		r_LoadingMessageTweener.PlayReverse();

		loadingMessage = "";
	}

	public void loadLevel( string levelName, string loadMessage ){
		
		
		object[] args = new object[2];
		args[0] = levelName;
		args[1] = loadMessage;
		StartCoroutine( "loadLevel_CR", args );
		
		Messenger.Cleanup ();
	}

	IEnumerator loadLevel_CR( object[] args ){
		loadingMessage = (string) args[1];

		r_LoadingScreenTweener.PlayForward();
		r_LoadingMessageTweener.PlayForward();

		yield return new WaitForSeconds( r_LoadingScreenTweener.duration );

		bool hasPro = UnityEditorInternal.InternalEditorUtility.HasPro();
		if(hasPro) {
			AsyncOperation ao = Application.LoadLevelAsync((string)args[0]);

			yield return ao;
		} else {
			Application.LoadLevel( (string) args[0] );
		}
		
		yield return new WaitForSeconds(1.0f);

		r_LoadingScreenTweener.PlayReverse();
		r_LoadingMessageTweener.PlayReverse();

		loadingMessage = "";
	}

	public void loadLastCheckpoint(string loadMessage){
		StartCoroutine( "loadLastCheckpoint_CR", loadMessage );
	}

	IEnumerator loadLastCheckpoint_CR(string loadMessage){
		loadingMessage = loadMessage;
		
		r_LoadingScreenTweener.PlayForward();
		r_LoadingMessageTweener.PlayForward();
		Messenger.Broadcast<bool>("lock player input", true);
		
		yield return new WaitForSeconds( r_LoadingScreenTweener.duration );

		Game.load();

		yield return new WaitForSeconds(1.0f);

		r_LoadingScreenTweener.PlayReverse();
		r_LoadingMessageTweener.PlayReverse();
		Messenger.Broadcast<bool>("lock player input", false);
		loadingMessage = "";
		Messenger.Cleanup ();
	}
}
