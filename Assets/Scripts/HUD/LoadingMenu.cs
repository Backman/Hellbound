using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingMenu : MonoBehaviour {
	public List<Checkpoint> m_CheckpointsAvailable = new List<Checkpoint>();
	void Start(){
		foreach(Checkpoint checkpoint in m_CheckpointsAvailable){
			checkpoint.start();
		}
	}
	
	public void load(){
		// This check should also be used in StartMenu initialization to change UI 
		// depending on if a valid savegame exists
		if(Game.doesSavegameExist()){
			Game.load();
		}
	}
	
	void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	void OnLevelWasLoaded(int level){
		Messenger.Broadcast<int>("OnLevelWasLoaded", level);
	}
}
