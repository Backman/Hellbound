#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Class to draw inspector layout for PuzzleLogic
/// </summary>
[CustomEditor(typeof(PuzzleLogic))]
public class Puzzle : Editor {
	private int mIndex = 0;

	public void OnEnable(){
		hideFlags = HideFlags.HideAndDontSave;
	}
	
	public override void OnInspectorGUI(){
        PuzzleLogicImp logic = (target as PuzzleLogic).getLogic();
        //Debug.Log(logic.GetHashCode());
        if (logic != null){
            List<EventData> events = logic.getEvents();
            NGUIEditorTools.SetLabelWidth(80f);
            NGUIEditorTools.DrawSeparator();

            if(events.Count > 0 && mIndex < 1){
                mIndex = 1;
            }

            // New event button
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("New event"))
            {
                ++mIndex;
                logic.addEvent();
            }
            GUI.backgroundColor = Color.red;
            // Only allow deleting if there are events available to delete
            if (GUILayout.Button("Delete event") && events.Count > 0)
            {
                logic.removeEvent(mIndex - 1);
                --mIndex;
                if (mIndex < 1 && events.Count > 0)
                {
                    mIndex = 1;
                }
            }
            GUI.backgroundColor = Color.white;

            //Debug.Log(events.Count + " index: " + mIndex);

            if (events.Count > 0 && mIndex > 0)
            {
            	EventData eventData = logic.getEvent(mIndex - 1);
                List<ObjectState> objectStates = logic.getObjectStates(mIndex - 1);
				TriggerData triggerData = logic.getTriggerData(mIndex - 1);
                GUILayout.BeginHorizontal();
                {
					string eventName = EditorGUILayout.TextField("Event name", eventData.getName());
					eventData.setName(eventName);
                }
                GUILayout.EndHorizontal();
                /*
				GUILayout.BeginHorizontal();
				{
					string requiredState = EditorGUILayout.TextField("Object state", eventData.getRequiredObjectState());
					eventData.setRequiredObjectState(requiredState);
				}
				GUILayout.EndHorizontal();
				*/
                // Logic switcher
                GUILayout.BeginHorizontal();
                {
                    if (mIndex == 1) GUI.color = Color.grey;
                    if (GUILayout.Button("<<") && mIndex > 1) { --mIndex; }
                    GUI.color = Color.white;

                    // Draw textfield with current selected logic, 
                    // and get the value if it was changed manually by user
                    mIndex = EditorGUILayout.IntField(mIndex, GUILayout.Width(40f));
                    if (mIndex < 1) { mIndex = 1; }
                    if (mIndex >= events.Count) { mIndex = events.Count; }

                    GUILayout.Label("/ " + events.Count, GUILayout.Width(40f));
                    if (mIndex == events.Count) GUI.color = Color.grey;
                    if (GUILayout.Button(">>") && mIndex < events.Count) { ++mIndex; }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();

                NGUIEditorTools.DrawSeparator();

                // New state handler
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("New state handler")){
                    ObjectState objectState = new ObjectState();
                    objectStates.Add(objectState);
                }
                GUI.backgroundColor = Color.white;

				int index = 0;
                foreach(ObjectState objectState in objectStates){
                    GUILayout.BeginHorizontal();
                    {
                    	/*
						GUILayout.Label("This", GUILayout.Width(30f));
						bool isThis = true;
						EditorGUILayout.Toggle(isThis, GUILayout.Width(15f));
						GameObject obj = null;
						if(isThis){
							GUI.backgroundColor = Color.black;
							GUI.contentColor = Color.black;
							EditorGUILayout.ObjectField(objectState.getObject(), typeof(GameObject));
							GUI.backgroundColor = Color.white;
							GUILayout.Label("state equals", GUILayout.Width(85f));
						}
						else{
							obj = (GameObject)EditorGUILayout.ObjectField(objectState.getObject(), typeof(GameObject));
							GUILayout.Label("'s state equals", GUILayout.Width(85f));
						}
						*/
						Interactable interObj = null;
						if(objectState.getObject() != null){
							interObj = objectState.getObject().GetComponent<Interactable>();
						} 
						Interactable obj = (Interactable)EditorGUILayout.ObjectField(interObj, typeof(Interactable));
						GUILayout.Label("'s state equals", GUILayout.Width(85f));
                        string state = EditorGUILayout.TextField("", objectState.getState());
						GUI.backgroundColor = Color.red;
						if(GUILayout.Button("Delete"))
						{
							objectStates.RemoveAt(index);
							GUI.backgroundColor = Color.white;
							GUILayout.EndHorizontal();
							break;
						}
						GUI.backgroundColor = Color.white;

                        objectState.setObject(obj == null ? null : obj.gameObject);
                        objectState.setState(state);
					}
					GUILayout.EndHorizontal();
					++index;
                }

                NGUIEditorTools.DrawSeparator();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Event to trigger", GUILayout.Width(100f));
					string eventToTrigger = EditorGUILayout.TextField("", triggerData.getName());
					
					triggerData.setName(eventToTrigger);
                }
                GUILayout.EndHorizontal();
                /*
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("New object state", GUILayout.Width(100f));
					string newObjectState = EditorGUILayout.TextField("", triggerData.getNewObjectState());
					
					triggerData.setNewObjectState(newObjectState);
				}
				GUILayout.EndHorizontal();
				*/
            }
        }
        EditorUtility.SetDirty(target);
	}
}
#endif