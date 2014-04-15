using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Puzzle : MonoBehaviour{
	// String instead of enum to have dynamic state
	protected string m_State;
	protected List<Puzzle> m_ChildPuzzles = new List<Puzzle>();
	
	public void addChild(Puzzle puzzle){
		m_ChildPuzzles.Add(puzzle);
	}
	
	public void removeChild(Puzzle puzzle){
		m_ChildPuzzles.Remove(puzzle);
	}
	
	public void setState(string state){
		m_State = state;
	}
}
