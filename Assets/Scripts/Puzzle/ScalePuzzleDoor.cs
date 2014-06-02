using UnityEngine;
using System.Collections;

public class ScalePuzzleDoor : MonoBehaviour {
	public FMODAsset m_DoorSound = null;
	private UIPlayTween m_Tweener;

	void Awake() {
		m_Tweener = GetComponent<UIPlayTween> ();
	}
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onScalePuzzleCleared", scalePuzzleCleared);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void scalePuzzleCleared(GameObject obj, bool tr){
		if (m_Tweener != null) {
			if(m_DoorSound != null) {
				FMOD_StudioSystem.instance.PlayOneShot(m_DoorSound, transform.position);
			}
			m_Tweener.Play(true);
		}
	}
}
