using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//peter
//lägg denna komponent på karaktären och sen lägg vilka punkter den ska gå till.
//lägg punkten på marken
[RequireComponent(typeof(ThirdPersonCharacter))]
public class PointToMove : MonoBehaviour {
	public List<GameObject> mTargets;
	private ThirdPersonCharacter r_Character; 
	// Use this for initialization
	void Start () {
		r_Character = GetComponent<ThirdPersonCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
		if (mTargets.Count != 0) {
			if(mTargets[0] != null){
				Vector3 move = mTargets [0].transform.position - transform.position;
				r_Character.move (move, false, move);
				if (collider.bounds.Contains (mTargets [0].transform.position)) {
					Debug.Log ("point reached");
					mTargets.Remove (mTargets [0]);
				}
			}
			else{
				mTargets.Remove (mTargets [0]);
			}
		} 
		else {
			r_Character.move (Vector3.zero, false, transform.position + Camera.main.transform.forward * 100.0f);
		}
	}
}
