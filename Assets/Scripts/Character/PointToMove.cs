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
	public bool Loop = false;
	private Transform r_Camera;	
	// Use this for initialization
	void Start () {
		r_Character = GetComponent<ThirdPersonCharacter>();
		if (mTargets.Count != 0) {
			if (gameObject.GetComponent<ThirdPersonController>()!=null) {
				gameObject.GetComponent<ThirdPersonController>().enabled = false;
			}
		}
		if (Camera.main) {
			r_Camera = Camera.main.transform;
		} else {
			Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}
	}

	// Update is called once per frame
	void Update () {
		if (mTargets.Count != 0) {
			if (mTargets [0] != null) {
				Vector3 head = transform.position + r_Camera.forward * 100.0f;
				Vector3 move = (mTargets [0].transform.position - transform.position);
				r_Character.move (move, false, head);

				if (collider.bounds.Contains (mTargets [0].transform.position)) {
					Debug.Log ("point reached");
					if (Loop && mTargets.Count != 1) {
							mTargets.Add (mTargets [0]);
					}
					mTargets.Remove (mTargets [0]);
				}

			} else {
				mTargets.Remove (mTargets [0]);
			}
			if (mTargets.Count == 0) {
				if (gameObject.GetComponent<ThirdPersonController> () != null) {
						gameObject.GetComponent<ThirdPersonController> ().enabled = true;
				}
			}
		} 
	}
}
