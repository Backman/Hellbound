using UnityEngine;
using System.Collections;

/// <summary>
/// This scirpt handles all logic associated with loading levels and displaying loading screens.
/// 
/// Created by Simon
/// </summary>
public class LoadingLogic : Singleton<LoadingLogic> {

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
		}

		r_LoadingMessage = r_LoadingScreen.GetComponentInChildren( typeof (UILabel) ) as UILabel;
		if( r_LoadingMessage == null ){
			Debug.LogError("Unable to find LoadingMessage label");
		} else {
			r_LoadingMessageTweener = r_LoadingMessage.GetComponent( typeof( UITweener ) ) as UITweener;
		}
	}

	public void loadLevel( string levelName, string loadMessage ){
	
		object[] args = new object[2];
		args[0] = levelName;
		args[1] = loadMessage;
		StartCoroutine( "loadLevel_CR", args );

	}

	IEnumerator loadLevel_CR( object[] args ){	
		Debug.Log("Call");
		loadingMessage = (string) args[1];

		r_LoadingScreenTweener.PlayForward();
		r_LoadingMessageTweener.PlayForward();

		yield return new WaitForSeconds( 2.0f );

		Application.LoadLevel( (string) args[0] );

		r_LoadingScreenTweener.PlayReverse();
		r_LoadingMessageTweener.PlayReverse();

		loadingMessage = "";
	}
}
