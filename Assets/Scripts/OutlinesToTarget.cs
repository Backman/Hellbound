using UnityEngine;
using System.Collections;

public class OutlinesToTarget : MonoBehaviour {
	public GameObject m_Target;
	public Shader m_Shader;
	public Color m_OutlineColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	[Range(0.0f, 0.03f)] public float m_OutlineWidth;
	private Material m_Material;
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject> ("onFocus", onFocus);
		Messenger.AddListener ("leaveFocus", leaveFocus);
		m_Material = new Material(m_Shader);
		m_Material.SetFloat ("_Outline", m_OutlineWidth);
		m_Material.SetColor("_OutlineColor", m_OutlineColor);
	}

	public void onFocus(GameObject obj) {
		GameObject temp = Instantiate(obj) as GameObject;
		var components = (Interactable)temp.GetComponent(typeof(Interactable));
		components.enabled = false;
		components.collider.enabled = false;

		temp.renderer.sharedMaterial = m_Material;
				
		m_Target = temp;
	}

	public void leaveFocus(){
		Destroy (m_Target);
	}
}
