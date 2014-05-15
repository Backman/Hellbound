using UnityEngine;
using System.Collections;

public class GameObjectInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Messenger.Broadcast<GameObject>("onGameObjectInitialized", gameObject);
	}
}
