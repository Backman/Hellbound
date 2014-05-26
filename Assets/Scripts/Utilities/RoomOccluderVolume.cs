using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomOccluderVolume : MonoBehaviour
{
	private int m_FrameChunk = 100;
	public List<GameObject> m_ParentsToLookThrough = new List<GameObject> ();

	private List<GameObject> m_Objects = new List<GameObject>();

	void Start (){
		Collider col = GetComponent<Collider> ();
		if( col != null ){
			foreach (GameObject obj in m_ParentsToLookThrough) {
				foreach( Transform t in obj.GetComponentsInChildren<Transform>() ){
					if( t.GetComponentInChildren<Transform>().childCount == 0 && col.bounds.Contains( t.position ) ){
						m_Objects.Add( t.gameObject );
					}
				}
			}
		}

		m_FrameChunk = Mathf.Max(m_Objects.Count / 30, 20);
	}	

	public void show(){
		StopCoroutine ("hide_CR");

		StartCoroutine("show_CR");
	}

	public void hide(){
		StopCoroutine ("show_CR");

		StartCoroutine ("hide_CR");
	}

	IEnumerator show_CR(){
		int i = 0;
		foreach (GameObject obj in m_Objects) {
			obj.SetActive(true);
			i++;
			if ( i == m_FrameChunk ){
				i = 0;
				yield return null;
			}
		}
	}
	IEnumerator hide_CR(){
		int i = 0;
		foreach (GameObject obj in m_Objects) {
			obj.SetActive(false);
			i++;
			if ( i == m_FrameChunk ){
				i = 0;
				yield return null;
			}
		}
	}

	
}

