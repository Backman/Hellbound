using UnityEngine;

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class SaveData : ISerializable{
	public bool foundGem;
	public int checkpoint;
	public SaveData(){
		foundGem = true;
		checkpoint = 23;
	}
	
	/// <summary>
	/// Deserialization constructor.
	/// </summary>
	public SaveData(SerializationInfo info, StreamingContext context){
		foundGem = (bool)info.GetValue("foundGem", typeof(bool));
		checkpoint = (int)info.GetValue("checkpoint", typeof(int));
	}
	
	/// <summary>
	/// Serialization function.
	/// </summary>
	public void GetObjectData(SerializationInfo info, StreamingContext context){
		info.AddValue("foundGem", foundGem);
		info.AddValue("checkpoint", checkpoint);
	}
}
