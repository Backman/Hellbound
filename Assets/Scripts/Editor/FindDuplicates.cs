﻿using UnityEngine;
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

	
	// Add menu named "CleanUpWindow" to the Window menu 
	[MenuItem("Window/DupesWindow")] 
	static void Init() 
	{ 
		// Get existing open window or if none, make a new one: 
		DuplicateWindow window = (DuplicateWindow)EditorWindow.GetWindow(typeof(DuplicateWindow));
		window.Show(); 

	} 

	//This ain't pretty, but it works. I am by no means a GUI designer :-P
	void OnGUI() 
	{ 
			
		GUIStyle style = new GUIStyle ();
		style.wordWrap = true;
		style.normal.textColor = Color.white;

		if (GUILayout.Button("Log dupes")) 
		{ 
			compareAssetList(UsedAssets.GetAllAssets()); 
		} 
		GUILayout.BeginVertical ();
		GUILayout.Label ("If there are duplicates, their names will be printed in the console.\n", style );
		GUILayout.Label ("After a name conflict's been detected, make sure you have Unity's 'Project' window open and then use the search field and type in the conflicting name.", style);
		GUILayout.EndVertical ();
	} 

	List<string> m_Strings = new List<string> ();
	private void compareAssetList(string[] assetList) 
	{ 
		m_Strings.Clear ();
		for (int i = 0; i < assetList.Length; i++) 
		{  
			string s =  assetList[i].Substring( assetList[i].LastIndexOf("/") +1 ).ToLower();	//Get the filename without path
			int idx = s.IndexOf("."); //find index of the file-type specifier

			if( idx > 0 ){
				string type = s.Substring(idx);

				if( idx > 0 && (type == ".cs" || type == ".boo" || type == ".js")){
					s = s.Substring(0, idx);	//get filename without file type
					m_Strings.Add(s);
				}
			}
		} 

		m_Strings.Sort ();
		for( int i = 1; i < m_Strings.Count; i++ ){
			if( m_Strings[i] == m_Strings[i-1] )	//Since the strings are sorted, if string n and n-1 is identical, two script files has the same name
				Debug.Log("Script name conflict: " + m_Strings[i] );
		}
	} 
}




















