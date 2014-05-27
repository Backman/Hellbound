using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Room occluder trigger.
/// 
/// A zone which regulates which RoomOccluderVolumes that should-, and should not, be visiable
/// from whitin that zone
/// 
/// Created by Simon Jonasson and Alexi Lindeman
/// </summary>
public class RoomOccluderTrigger : MonoBehaviour{

	public enum TriggerType { Show, Hide };

	[TooltipAttribute("Show: The listed rooms will be shown, all other rooms will be hidden.\n" +
					  "Hide: The listed rooms will be hidden, all other rooms will be shown.")]
	public TriggerType m_TriggerType = TriggerType.Show;

	public List<RoomOccluderVolume> m_Rooms = new List<RoomOccluderVolume>();
	private List<RoomOccluderVolume> m_OtherRooms = new List<RoomOccluderVolume>();

	

	void Start(){
		//Fill m_OtherRooms with all room that are not listen in m_Rooms
		RoomOccluderVolume[] rooms = GameObject.FindObjectsOfType<RoomOccluderVolume> ();
		foreach (RoomOccluderVolume room in rooms ){
			if( !m_Rooms.Contains( room ) ){
				m_OtherRooms.Add(room);
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if( col.tag == "Player" ){
			Debug.Log("Player entered "+gameObject.name);

			doAction();
		}
	}

	void doAction(){
		if (m_TriggerType == TriggerType.Hide) {
			hide (m_Rooms);
			show (m_OtherRooms);
		} else if (m_TriggerType == TriggerType.Show) {
			hide (m_OtherRooms);
			show (m_Rooms);
		}
	}

	void hide(List<RoomOccluderVolume> rooms){
		foreach( RoomOccluderVolume room in rooms ){
			if( room != null)
				room.hide();
		}
	}

	void show(List<RoomOccluderVolume> rooms){
		foreach( RoomOccluderVolume room in rooms ){
			if( room != null)
				room.show();
		}
	}
}

