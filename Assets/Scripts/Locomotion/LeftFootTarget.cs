using UnityEngine;
using System.Collections;

public class LeftFootTarget : MonoBehaviour {
	private static Vector3 m_TargetPosition;
	
	void LateUpdate(){
		Vector3 origin = gameObject.transform.position;
		Vector3 direction = new Vector3(0.0f, -1.0f, 0.0f);
		RaycastHit hitInfo;
		if(Physics.Raycast(origin, direction, out hitInfo)){
			m_TargetPosition = hitInfo.point;
		}
	}
	
	public static Vector3 getPosition(){
		return m_TargetPosition;
	}
}
