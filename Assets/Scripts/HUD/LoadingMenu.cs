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
		Game.load();
	}
	
	void Awake(){
		GameObject.DontDestroyOnLoad(gameObject);
	}
}
