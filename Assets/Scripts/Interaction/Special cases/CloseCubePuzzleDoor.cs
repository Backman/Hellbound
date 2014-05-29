using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Close cube puzzle door.
/// 
/// A special case script for closing the door after the cube puzzle.
/// The standard door scripts cannot be used since this door is very 
/// different.
/// 
/// Created by Simon Jonasson
/// </summary>
public class CloseCubePuzzleDoor : MonoBehaviour {
	private bool m_Enabled = false;
	private bool m_Used    = false;
	[SerializeField][Tooltip("The tweener of the cube key door")]
	private List<UIPlayTween> r_Tweeners = new List<UIPlayTween>();
	public FMODAsset m_CloseDoorSound = null;
	public void enableThis(){
		m_Enabled = true;
	}

	void OnTriggerEnter(Collider col){
		if( col.tag == "Player" && !m_Used){
			foreach(UIPlayTween tweener in r_Tweeners) {
				tweener.playDirection = AnimationOrTween.Direction.Reverse;
				tweener.Play(true);
			}
			if(m_CloseDoorSound != null) {
				if(r_Tweeners.Count > 0) {
					FMOD_StudioSystem.instance.PlayOneShot(m_CloseDoorSound, r_Tweeners[0].transform.position);
				}
			}
			m_Used = true;
		}
	}
}
