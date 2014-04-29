using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EventData{
	[SerializeField] string m_Name;
	[SerializeField] string m_RequiredObjectState;
	public EventData(){
		m_Name = "";
		m_RequiredObjectState = "";
	}
	
	public string getName(){
		return m_Name;
	}
	
	public string getRequiredObjectState(){
		return m_RequiredObjectState;
	}
	
	public void setName(string name){
		m_Name = name;
	}
	
	public void setRequiredObjectState(string requiredState){
		m_RequiredObjectState = requiredState;
	}
}

[System.Serializable]
public class ObjectState {
	//[SerializeField] bool m_IsThisObject;
	[SerializeField] GameObject m_Object;
	[SerializeField] string m_State;
	
	public ObjectState(){
		m_Object = null;
		m_State = "";
	}
	/*
	public bool isObjectAsThis(){
		return m_IsThisObject;
	}
	*/
	public GameObject getObject(){
		return m_Object;
	}
	
	public string getState(){
		return m_State;
	}
	/*
	public void setObjectAsThis(){
		m_IsThisObject = true;
	}
	*/
	public void setObject(GameObject obj){
		m_Object = obj;
	}
	
	public void setState(string state){
		 m_State = state;
	}
}

/// <summary>
/// Unity is unable to serialize list of lists, so a class is needed to hold a list,
/// and then create a list of an instance of that class.
/// </summary>
[System.Serializable]
public class ObjectStates {
	[SerializeField] List<ObjectState> m_ObjectStates = new List<ObjectState>();
	public List<ObjectState> getObjects(){
		return m_ObjectStates;
	}
}

[System.Serializable]
public class TriggerData{
	[SerializeField] string m_EventName;
	[SerializeField] string m_NewObjectState;
	
	public TriggerData(){
		m_EventName = "";
		m_NewObjectState = "";
	}
	
	public string getName(){
		return m_EventName;
	}
	
	public string getNewObjectState(){
		return m_NewObjectState;
	}
	
	public void setName(string name){
		m_EventName = name;
	}
	
	public void setNewObjectState(string newObjectState){
		m_NewObjectState = newObjectState;
	}
}

[System.Serializable]
public class PuzzleLogicImp {
	[SerializeField] List<EventData> m_Events = new List<EventData>();
	[SerializeField] List<ObjectStates> m_ObjectStates = new List<ObjectStates>();
	[SerializeField] List<TriggerData> m_EventToTrigger = new List<TriggerData>();

	public EventData getEvent(int index){
        return m_Events[index];
    }
    
    public List<EventData> getEvents(){
    	return m_Events;
    }

	public TriggerData getTriggerData(int index){
        return m_EventToTrigger[index];
    }

    public void addEvent(){
        m_Events.Add(new EventData());
        m_ObjectStates.Add(new ObjectStates());
        m_EventToTrigger.Add(new TriggerData());
    }

    public void removeEvent(int index){
        m_Events.RemoveAt(index);
        m_ObjectStates.RemoveAt(index);
        m_EventToTrigger.RemoveAt(index);
    }

    public List<ObjectState> getObjectStates(int index){
		return m_ObjectStates[index].getObjects();
    }
}

public class PuzzleEvent{
	private static Dictionary<string, bool> m_EventCancelState = new Dictionary<string, bool>();
	public static void cancel(string eventName){
		if(m_EventCancelState.ContainsKey(eventName)){
			m_EventCancelState[eventName] = true;
		}
		else{
			m_EventCancelState.Add(eventName, true);
		}
	}
	
	public static bool isEventCancelled(string eventName){
		if(m_EventCancelState.ContainsKey(eventName)){
			return m_EventCancelState[eventName];
		}
		return false;
	}
	
	public static void trigger(string eventName, GameObject obj, bool triggerOnlyForMe){
		// Reset cancel state when triggering event again
		if(m_EventCancelState.ContainsKey(eventName)){
			m_EventCancelState[eventName] = false;
		}
		Messenger.Broadcast<GameObject, bool>(eventName, obj, triggerOnlyForMe);
	}
}

public class PuzzleLogic : MonoBehaviour{
    [SerializeField] PuzzleLogicImp m_Logic;
	public PuzzleLogicImp getLogic(){
		if (m_Logic == null){
			m_Logic = new PuzzleLogicImp();
        }
		return m_Logic;
    }
    
    void Start(){
    	int index = 0;
		foreach(EventData eventData in m_Logic.getEvents()){
			int idx = index;
			Messenger.AddListener(eventData.getName(), 
				delegate(GameObject obj, bool triggerOnlyForMe){
					//string requiredState = m_Logic.getEvent(idx).getRequiredObjectState();
					bool doCall = true;
					string eventName = m_Logic.getEvent(idx).getName();
					if(PuzzleEvent.isEventCancelled(eventName)){
						doCall = false;
					}
					
					if(triggerOnlyForMe && obj != gameObject){
						doCall = false;
					}
					/*
					Interactable interObj = obj.GetComponent<Interactable>();
					if(m_Logic.getEvent(idx).getName() == "onUseWith"){
						Debug.Log("State: "+interObj.getPuzzleState()+", expected: "+requiredState);
					}
					if(interObj != null && requiredState.Length > 0 && requiredState != interObj.getPuzzleState()){
						doCall = false;
					}
					*/
					if(doCall){
						List<ObjectState> objectStates = m_Logic.getObjectStates(idx);
						TriggerData triggerData = m_Logic.getTriggerData(idx);
						foreach(ObjectState objectState in objectStates){
							GameObject attachedObj = objectState.getObject();
							if(attachedObj == null){
								attachedObj = gameObject;
							}
							Interactable interSavedObj = attachedObj.GetComponent<Interactable>();
							if(interSavedObj != null){
								if(interSavedObj.getPuzzleState() != objectState.getState()){
									doCall = false;
									break;
								}
							}
							else{
								Debug.LogError("Condition objects need to have interactable script attached to it");
							}
						}
						if(doCall){
							/*
							Interactable thisInterObj = gameObject.GetComponent<Interactable>();
							if(thisInterObj != null && triggerData.getNewObjectState().Length > 0){
								thisInterObj.setPuzzleState(triggerData.getNewObjectState());
							}
							*/
							if(triggerData.getName().Length > 0 && !PuzzleEvent.isEventCancelled(triggerData.getName())){
								Messenger.Broadcast<GameObject, bool>(triggerData.getName(), gameObject, false);
							}
						}
					}
				}
			);
			++index;
		}
    }
}
