using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Example on serialization taken from: http://www.codeproject.com/Articles/1789/Object-Serialization-using-C
/// <summary>
/// Class to save to file and load from file using serialization
/// By: Aleksi Lindeman
/// </summary>

[System.Serializable]
public class SerializablePair<T, U> {
	public SerializablePair() {}
	
	public SerializablePair(T first, U second) {
		this.first = first;
		this.second = second;
	}
	
	public T first { get; set; }
	public U second { get; set; }
};

[System.Serializable()]
public class GameData : ISerializable{
	public string currentCheckpointID = "";
	public List<string> usedCheckpoints = new List<string>();
	public List<SerializablePair<int, string>> interactableStates = new List<SerializablePair<int, string>>();
	public List<string> inventoryItems = new List<string>();
	
	public GameData(){
		
	}
	
	// Deserialization constructor
	public GameData(SerializationInfo info, StreamingContext context){
		currentCheckpointID = (string)info.GetValue("currentCheckpointID", typeof(string));
		usedCheckpoints = (List<string>)info.GetValue("usedCheckpoints", typeof(List<string>));
		interactableStates = (List<SerializablePair<int, string>>)info.GetValue("interactableStates", typeof(List<SerializablePair<int, string>>));
		inventoryItems = (List<string>)info.GetValue("inventoryItems", typeof(List<string>));
	}
	
	// Serialization function
	public void GetObjectData(SerializationInfo info, StreamingContext context){
		info.AddValue("currentCheckpointID", currentCheckpointID);
		info.AddValue("usedCheckpoints", usedCheckpoints as object, typeof(List<string>));
		info.AddValue("interactableStates", interactableStates as object, typeof(List<SerializablePair<int, string>>));
		info.AddValue("inventoryItems", inventoryItems as object, typeof(List<string>));
	}
	
	public void save(string path){
		Stream stream = File.Open(path, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
		bformatter.Serialize(stream, this);
		stream.Close();
	}
	
	public static GameData load(string path){
		if(File.Exists(path)){
			//Debug.Log("LOAD: File exists: "+path);
			Stream stream = File.Open(path, FileMode.Open);
			BinaryFormatter bformatter = new BinaryFormatter();
			GameData gameData = (GameData)bformatter.Deserialize(stream);
			stream.Close();
			
			return gameData;
		}
		else{
			//GameData gameData = new GameData();
			//gameData.save(path);
			
			//return gameData;
			//Debug.Log("LOAD: File does not exist: "+path);
			return null;
		}
	}
	
	public bool hasCheckpointBeenUsed(Checkpoint checkpoint){
		return usedCheckpoints.Contains(checkpoint.getUniqueID());
	}
	
	public Interactable getInteractableFromID(int id){
		int idx = 0;
		foreach(Interactable inter in GameObject.FindObjectsOfType<Interactable>()){
			if(idx == id){
				return inter;
			}
			++idx;
		}
		return null;
	}
}
