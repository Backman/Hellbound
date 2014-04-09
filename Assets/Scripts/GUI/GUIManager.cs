using UnityEngine;
using System.Collections;

public class GUIManager : Singleton<GUIManager> {
	private bool m_Busy = false;

	[SerializeField]
	private UISprite r_DescriptionWindow;
	[SerializeField]
	private UILabel r_DescriptionLabel;

	[SerializeField]
	[Range (0, 1)]
	private float m_WindowFadeSpeed = 1.0f;
	[SerializeField]
	[Range (0, 1)]
	private float m_TextSpeed = 1.0f;

	public void Awake(){
		if( r_DescriptionWindow == null ){
			Debug.LogError("Error! No description window present!");
		}
		if( r_DescriptionLabel == null ){
			Debug.LogError("Error! No description label present!");
		}
		m_TextSpeed = (1.0f - m_TextSpeed) * 0.1f;
		m_WindowFadeSpeed = 1.0f - m_WindowFadeSpeed;
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
	
		yield return StartCoroutine ("awaitInput");
	
		//TODO: Lock controllers			
		yield return StartCoroutine( "hideWindow" );
		StopCoroutine( "feedText" );
		m_Busy = false;
		//TODO: Unlock controllers

	}

	IEnumerator showWindow(){
		Color col = new Color();
		while( r_DescriptionWindow.color.a < 0.95f ){
			col = r_DescriptionWindow.color;
			col.a +=  Time.deltaTime / m_WindowFadeSpeed;
			r_DescriptionWindow.color = col; 
			yield return null;
		}
		col.a = 1.0f;
		r_DescriptionWindow.color = col;
	}

	IEnumerator awaitInput(){
		while( true ){
			if( Input.GetMouseButtonDown(0) )
				break;
			yield return null;
		}
	}

	IEnumerator feedText( string text ){
		string t = "";
		for( int i = 0; i < text.Length; ++i){
			t += text[i];
			r_DescriptionLabel.text = t;
			yield return new WaitForSeconds(m_TextSpeed);
		}
	}

	IEnumerator hideWindow(){
		Color col = new Color();
		while( r_DescriptionWindow.color.a > 0.05f ){
			col = r_DescriptionWindow.color;
			col.a -=  Time.deltaTime / m_WindowFadeSpeed;
			r_DescriptionWindow.color = col; 
			yield return null;
		}
		col.a = 0.0f;
		r_DescriptionLabel.text = "";
		r_DescriptionWindow.color = col;
	}
	#endregion
}
