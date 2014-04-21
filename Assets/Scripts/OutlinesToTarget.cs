using UnityEngine;
using System.Collections;

public class OutlinesToTarget : MonoBehaviour {
	public GameObject m_Target;
	public Shader m_Shader;

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject> ("onFocus", onFocus);
		Messenger.AddListener ("leaveFocus", leaveFocus);
	}

	public void onFocus(GameObject obj) {
		GameObject temp = Instantiate(obj) as GameObject;
		var components = (Interactable)temp.GetComponent(typeof(Interactable));
		components.enabled = false;
		components.collider.enabled = false;
		/*foreach (var component in components) {
			Debug.Log ("Disabling: " + component.ToString ());
			component = false;
		}*/
		//GetComponent<Renderer> ().enabled = true;

		temp.renderer.material.shader = m_Shader;
				
		m_Target = temp;
	}

	public void leaveFocus(){
		Destroy (m_Target);
	}
}
