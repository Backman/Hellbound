using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This file contains helper functions,
/// data container definitions
/// as well as namespace definitions
/// for GUI helpers for the project Hellbound.
/// 
/// Created by Simon Jonasson
/// 
/// _minor_ editing by Anton Thorsell
/// (basically anything that have to do with sounds)
/// </summary>
namespace MyGUI{

	[System.Serializable]
	public class SubtitlesSettings{
		[SerializeField][Range (0, 20)]
		public float DisplayTime;
		[SerializeField][Range (0, 1)]
		private float textSpeed;
		public  float TextSpeed { get{ return (float) (1.0f - textSpeed); } }

		public string SoundPath = "event:/";
		public GameObject SoundPosition;

		[SerializeField][Multiline]
		public string Text;
	}
	[System.Serializable]
	public class NoteSettings{
		[SerializeField][Multiline]
		public string text;
	}

	public class Tools{
		/// <summary>
		/// Returns a line composed of the words in the stack that would
		/// fit into the label.	
		/// The line is returned when the next word wouldn't fit or the last poped
		/// word was a newline.
		/// 
		/// The stack is requiered to have all characters and all whitespaces in
		/// separate elements. The labels text must be blank.
		/// </summary>
		public static string getLine( Stack<string> words, UILabel targetLabel, bool doLinePadding ){
			string line = "";
			string currWord = "";
			Vector2 labelSize = new Vector2( targetLabel.width, targetLabel.height );
			Vector2 textSize  = new Vector2();
			targetLabel.UpdateNGUIText();
			
			//Add next word to the current line as long as the line would fit in the label
			//and not cause a newline.
			while( words.Count > 0 ){
				currWord = words.Peek();
				textSize = NGUIText.CalculatePrintedSize(line + currWord);
				
				if( textSize.y > labelSize.y ){	
					//Check if the current word is a whitespace. If it is, remove it
					if( currWord.Trim() == string.Empty ){
						words.Pop();
						line.Trim();
					}
					textSize = NGUIText.CalculatePrintedSize(line + " ");
					while( textSize.y < labelSize.y && doLinePadding ){
						line += " ";
						textSize = NGUIText.CalculatePrintedSize(line + " ");
					}
					return line;
				}
				line += words.Pop();
			}
			
			return line;
		}			
	}
}

