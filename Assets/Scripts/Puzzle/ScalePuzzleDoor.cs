using UnityEngine;
using System.Collections;

public class ScalePuzzleDoor : MonoBehaviour {
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
			m_Tweener.Play(true);
		}
	}
}
