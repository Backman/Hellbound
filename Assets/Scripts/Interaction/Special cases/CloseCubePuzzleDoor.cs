using UnityEngine;
using System.Collections;

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
	private UIPlayTween r_Tweener;

	public void enableThis(){
		m_Enabled = true;
	}

	void OnTriggerEnter(Collider col){
		if( col.tag == "Player" && !m_Used){
			r_Tweener.playDirection = AnimationOrTween.Direction.Reverse;
			r_Tweener.Play(true);
			m_Used = true;
		}
	}
}
