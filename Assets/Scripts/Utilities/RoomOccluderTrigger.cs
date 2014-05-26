using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomOccluderTrigger : MonoBehaviour{
	public List<RoomOccluderVolume> m_RoomsToOcclude = new List<RoomOccluderVolume>();

	private static RoomOccluderTrigger r_PreviouslyTriggered = null;

	void OnTriggerEnter(Collider col){
		if( col.tag == "Player" ){
			Debug.Log("Player entered "+gameObject.name);

			if( r_PreviouslyTriggered != null ){
				r_PreviouslyTriggered.show();
			}
			r_PreviouslyTriggered = this;

			hide();
		}
	}

	void hide(){
		foreach( RoomOccluderVolume room in m_RoomsToOcclude ){
			room.hide();
		}
	}

	void show(){
		foreach( RoomOccluderVolume room in m_RoomsToOcclude ){
			room.show();
		}
	}

	//public OccludeType getOccludeType(){
		//return m_OccludeType;
	//}
}

