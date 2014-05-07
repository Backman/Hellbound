using UnityEngine;
using System.Collections;

public class SetupUILogic : MonoBehaviour {
	public GameObject m_UiToInstantiate;

	void Start(){
		if( FindObjectOfType( typeof(UIRoot) ) == null ){
			Instantiate( m_UiToInstantiate, Vector3.zero + Vector3.up*-100.0f, Quaternion.identity);
		}
	}
}
