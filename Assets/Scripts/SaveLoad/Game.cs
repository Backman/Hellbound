using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to save and load game using checkpoints
/// By: Aleksi Lindeman
/// </summary>
public class Game {
	private static string m_CurrentSavegame = "data.hbsg";
	private static GameData m_CurrentGameData = null;
	private static bool m_LoadingLevel = false;
	
	//public static void setCurrentSavegame(string savegame){
		//m_CurrentSavegame = savegame;
		//m_CurrentGameData = GameData.load(m_CurrentSavegame);
	//}
	
	/// <summary>
	/// Checks if savegame file exists, and that at least one checkpoint has been used in the savegame
	/// </summary>
	public static bool doesSavegameExist(){
		GameData gameData = GameData.load(m_CurrentSavegame);
		if(gameData != null && gameData.usedCheckpoints.Count > 0){
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Creates a new savegame file. Overrides if it already exists
	/// </summary>
	public static void createSavegame(){
		Debug.Log("Create new savegame!");
		GameData gameData = new GameData();
		gameData.save(m_CurrentSavegame);
		m_CurrentGameData = gameData;
	}
	
	public static GameData getGameData(){
		return m_CurrentGameData;
	}
	
	public static void setCurrentSavegameCheckpoint(string checkpointID){
		// Only attempt to save if savegame is used
		if(m_CurrentGameData != null){
			m_CurrentGameData.currentCheckpointID = checkpointID;
			// Set so we can no longer use this checkpoint
			m_CurrentGameData.usedCheckpoints.Add(checkpointID);
			//m_CurrentGameData.save(m_CurrentSavegame);
		}
	}
	
	public static bool hasCheckpointBeenUsed(Checkpoint checkpoint){
		if(m_CurrentGameData != null){
			return m_CurrentGameData.hasCheckpointBeenUsed(checkpoint);
		}
		// Return true if no game data is loaded to prevent saving because no save destination has been set
		return true;
	}
	
	public static void loadUsingCheckpointData(){
		if(Application.CanStreamedLevelBeLoaded(m_CurrentGameData.levelToLoad)){

			m_LoadingLevel = true;
			Application.LoadLevel(m_CurrentGameData.levelToLoad);
		} 
		else{
			Debug.LogWarning ("Unable to load level: "+m_CurrentGameData.levelToLoad);
		}
	}
	
	// Load using checkpoint. Checkpoint is loaded using m_CurrentSavegame file
	public static void load(bool loadCheckpointData = true){
		m_CurrentGameData = GameData.load(m_CurrentSavegame);
		if(m_CurrentGameData != null){
			Debug.Log("Attempting to load from checkpoint: "+m_CurrentGameData.currentCheckpointID);
			if(loadCheckpointData){
				loadUsingCheckpointData();
			}
		}
		else{
			Debug.Log("Failed to load game data "+m_CurrentSavegame);
		}
	}
	
	public static void save(){
		if(m_CurrentGameData != null){
			m_CurrentGameData.save(m_CurrentSavegame);
		}
	}
	
	public static bool isLoadingLevel(){
		return m_LoadingLevel;
	}
	
	public static void setLoadingLevel(bool loading){
		m_LoadingLevel = false;
	}
}
