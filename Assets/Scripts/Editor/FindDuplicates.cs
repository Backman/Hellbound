using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

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
	
	void OnGUI() 
	{ 
		GUI.color = Color.white; 
		
		if (GUILayout.Button("Log dupes")) 
		{ 
			compareAssetList(UsedAssets.GetAllAssets()); 
		} 
		
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

		for( int i = 1; i < m_Strings.Count; i++ ){

			if( m_Strings[i] == m_Strings[i-1] )
				Debug.Log("Script name conflict: " + m_Strings[i] );
		}
	} 
}




















