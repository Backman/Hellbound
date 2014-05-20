using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Duplicate window.
/// 
/// This script searches for scripts which has naming conflicts.
/// 
/// Created by Simon Jonasson, 
/// Inspiered by Kristian Langelund Helle Jespersen, http://zaxisgames.blogspot.se/2012/02/automatically-locate-all-unused-assets.html
/// </summary>
[ExecuteInEditMode]
public class DuplicateWindow : EditorWindow {
	List<string> m_Strings = new List<string> ();
	static bool m_Init = false;
	static bool m_Found = false;
	
	// Add menu named "CleanUpWindow" to the Window menu 
	[MenuItem("Window/DupesWindow")] 
	static void Init() 
	{ 
		// Get existing open window or if none, make a new one: 
		DuplicateWindow window = (DuplicateWindow)EditorWindow.GetWindow(typeof(DuplicateWindow));
		window.minSize = new Vector2 (200, 300);
		window.Show(); 
		m_Init = false;
		m_Found = false;
	} 

	//This ain't pretty, but it works. I am by no means a GUI designer :-P
	void OnGUI() 
	{ 
			
		GUIStyle style = new GUIStyle ();
		style.wordWrap = true;
		style.normal.textColor = Color.white;

		GUILayout.BeginVertical ();
		if (GUILayout.Button("Log dupes")) 
		{ 
			compareAssetList(UsedAssets.GetAllAssets()); 
			m_Init = true;

		} 
		if (m_Init && !m_Found) {
			style.normal.textColor = Color.green;
			GUILayout.Label ("\nNo duplicates found :-D\n\n", style);
			style.normal.textColor = Color.white;
		} else if ( m_Init && m_Found){
			style.normal.textColor = Color.red;
			string s = "";
			foreach( string t in m_Strings ){
				s = s + t + "\n";
			}
			GUILayout.Label ("\nDuplicates found!!!\n\n" + s, style);
			Debug.Log("Found duplicates:\n"+s);
			style.normal.textColor = Color.white;
		} else {
			GUILayout.Label ("\nPress the button above. The results will be printed in this window and in the console.\n\nThe seach might take a few minues. During that time, Unity will freeze\n\nIn the case there is a huge amount of conflicts, you might need to expand the window and press the button again\n\nKnown issues: This window will sometimes not register clicks and sometimes revert to this 'Init' stage after a seach has been done. In that case, just do the search again.", style);
		}
		GUILayout.EndVertical ();	
	} 



	private void compareAssetList(string[] assetList) 
	{ 

		m_Strings.Clear ();
		List<string> filenames = new List<string> ();
		for (int i = 0; i < assetList.Length; i++) 
		{  
			string s =  assetList[i].Substring( assetList[i].LastIndexOf("/") +1 ).ToLower();	//Get the filename without path
			int idx = s.IndexOf("."); //find index of the file-type specifier

			if( idx > 0 ){
				string type = s.Substring(idx);

				if( idx > 0 && (type == ".cs" || type == ".boo" || type == ".js")){
					s = s.Substring(0, idx);	//get filename without file type
					filenames.Add(s);
				}
			}
		} 

		filenames.Sort ();
		for( int i = 1; i < filenames.Count; i++ ){
			if( filenames[i] == filenames[i-1] ){	//Since the strings are sorted, if string n and n-1 is identical, two script files has the same name
				m_Found = true;
				if( !m_Strings.Contains(filenames[i]) ){ 
					m_Strings.Add(filenames[i]); 
				}				   
			}
		}
	} 
}




















