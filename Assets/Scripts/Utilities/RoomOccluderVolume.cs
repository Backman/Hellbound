using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Room occluder volume.
/// 
/// The logic for a room volume which will help us occlude objects which aren't visable to the player.
/// If hide is called on a room volume, each mesh renderer within that room who are children of the m_ParentsToLookThrough list
/// will be deactivated and thus won't be rendered.
/// 
/// Created by Simon Jonasson and Alexi Lindeman
/// </summary>
public class RoomOccluderVolume : MonoBehaviour
{
	public List<GameObject> m_ParentsToLookThrough = new List<GameObject> ();

	private int m_FrameChunk = 100;	//How many object we handle per frame
	private List<MeshRenderer> m_Objects = new List<MeshRenderer>();
	private bool m_Shown = true;	//If the objects within the room are occluded or not


	void Start (){
			Collider col = GetComponent<Collider> ();

			//This function takes each object in the m_ParentsToLookThrough list and check if they are within the volume of the 
			//room by sending a ray downwards from the object.
			//Since rooms are represented by a mesh collider, If the ray hits the RoomOccluders collider, we know that the object is within the room. 
			if (col != null) {
				RaycastHit hit = new RaycastHit ();

				foreach (GameObject obj in m_ParentsToLookThrough) {
					foreach (Transform t in obj.GetComponentsInChildren<Transform>()) {
						Ray ray = new Ray (new Vector3 (t.position.x, t.position.y + 10, t.position.z), Vector3.down);
						if (t.GetComponentInChildren<Transform> ().childCount == 0 && col.Raycast (ray, out hit, 15)) {
							MeshRenderer r = t.GetComponent<MeshRenderer> ();
							if (r != null)
								m_Objects.Add (r);
						}
					}
				}
			}

			m_FrameChunk = Mathf.Max (m_Objects.Count / 30, 20);
	}

	public void show(){
		if( !m_Shown ){
			m_Shown = true;
			StopCoroutine ("hide_CR");
			StartCoroutine("show_CR");
		}
	}

	public void hide(){
		if( m_Shown ){
			m_Shown = false;
			StopCoroutine ("show_CR");
			StartCoroutine ("hide_CR");
		}
	}

	IEnumerator show_CR(){
		int i = 0;
		foreach (MeshRenderer obj in m_Objects) {
			obj.enabled = true;
			i++;
			if ( i == m_FrameChunk ){
				i = 0;
				yield return null;
			}
		}
	}
	IEnumerator hide_CR(){
		int i = 0;
		foreach (MeshRenderer obj in m_Objects) {
			obj.enabled = false;
			i++;
			if ( i == m_FrameChunk ){
				i = 0;
				yield return null;
			}
		}
	}
}

