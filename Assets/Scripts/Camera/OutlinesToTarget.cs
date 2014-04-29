using UnityEngine;
using System.Collections;

//peter
//skapar en kopia av en object som ska följa original objektet och ha outlines only shadern på sig.
//skapar en kopia så att objektet kan ha kvar sin original shader på sig samtidigt ha outlines.

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
	/*	Mesh mesh = obj.GetComponent<Mesh>();
		GameObject temp = new GameObject("Duplicant");
		MeshFilter meshFilter = temp.AddComponent<MeshFilter>();
		temp.AddComponent<MeshRenderer>();

		Mesh tempMesh = temp.GetComponent<Mesh>();
		tempMesh.vertices = mesh.vertices;
		tempMesh.colors = mesh.colors;
		tempMesh.triangles = mesh.triangles;
		tempMesh.normals = mesh.normals;

		//meshFilter.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
		temp.renderer.sharedMaterial = m_Material;
		temp.transform.position = obj.transform.position;
		temp.transform.rotation = obj.transform.rotation;
		temp.transform.localScale = obj.transform.localScale;


		var components = (Interactable)temp.GetComponent(typeof(Interactable));
		components.enabled = false;
		components.collider.enabled = false;

		temp.renderer.sharedMaterial = m_Material;

		m_Target = temp;	*/
	}

	public void leaveFocus(){
		Destroy (m_Target);
	}
}
