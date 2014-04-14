using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ConditionDictionary {
	[SerializeField] private List<Condition> m_Conditions;
	[SerializeField] private List<bool> m_Values;
	
	public ConditionDictionary() {
		m_Conditions = new List<Condition>();
		m_Values = new List<bool>();
	}
	
	public Condition[] Keys
	{
		get { return m_Conditions.ToArray(); }
	}
	
	public bool[] Values
	{
		get { return m_Values.ToArray(); }
	}
}



[System.Serializable]
public class PrerequisiteState : State {
	public ConditionDictionary m_Prerequisites;
	// public Dictionary<Condition, bool> m_Prerequisites;
	
	public bool conditionsMet() {
		int i = 0;
		foreach(Condition c in m_Prerequisites.Keys) {
			if(c.isMet != m_Prerequisites.Values[i]){
				return false;
			}
			i++;
		}
		return true;
	}
}
