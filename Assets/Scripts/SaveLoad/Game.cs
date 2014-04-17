using UnityEngine;

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Game {
	public static void save(SaveData saveData, string path){
		Stream stream = File.Open(path, FileMode.Create);
		BinaryFormatter bformatter = new BinaryFormatter();
		Debug.Log("Saving game to "+path);
		
		bformatter.Serialize(stream, saveData);
		stream.Close();
	}
	
	public static SaveData load(string path){
		Stream stream = File.Open(path, FileMode.Open);
		BinaryFormatter bformatter = new BinaryFormatter();
		Debug.Log("Loading game from "+path);
		
		SaveData saveData = new SaveData();
		saveData = (SaveData)bformatter.Deserialize(stream);
		stream.Close();
		Debug.Log("foundGem: "+saveData.foundGem);
		Debug.Log("checkpoint: "+saveData.checkpoint);
		return saveData;
	}
}
