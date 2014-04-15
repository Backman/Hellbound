using UnityEngine;
using System.Collections;

public class PuzzleFaller : Puzzle {
	//<summary>
	//Everytime a PuzzleFallerPlatform is used, it calls this function
	//to see if the winning condition for this puzzle has been fulfilled.
	//Winning condition for this puzzle is to make all platforms fall
	//(making them no longer usable)
	//</summary>
	protected virtual void Start(){}
	public bool checkConditions(){
		foreach(PuzzleFallerPlatform puzzle in m_ChildPuzzles){
			if(!puzzle.usable()){
				return false;
			}
		}
		Messenger.Broadcast("onPuzzleFallerConditionSet", gameObject);
		return true;
	}
}
