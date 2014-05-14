using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//peter
//skapar en kopia av en object som ska följa original objektet och ha outlines only shadern på sig.
//skapar en kopia så att objektet kan ha kvar sin original shader på sig samtidigt ha outlines.

public class OutlinesToTarget : MonoBehaviour {
	public List<GameObject> m_Target;
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

	private void searchchild(List<GameObject> allchilds, Transform trans){
		foreach (Transform each in trans) {
			allchilds.Add (each.gameObject);
			searchchild(allchilds, each);
		}
	}

	public void onFocus(GameObject obj) {
		Debug.Log ("outlines ON");
		List<GameObject> all = new List<GameObject>();
		all.Add (obj.gameObject);
		searchchild (all, obj.transform);
		foreach (GameObject each in all) {
			if (each.renderer) {
				if (each.renderer.enabled == true && each.GetComponent<MeshFilter> () != null) {
					m_Target.Add (new GameObject ("outline_copy"));
					GameObject lTarget = m_Target [m_Target.Count - 1];
					lTarget.transform.position = each.transform.position;
					lTarget.transform.rotation = each.transform.rotation;
					lTarget.transform.localScale = each.transform.localScale;
					lTarget.transform.parent = each.transform;
					//l for local
					Mesh m = each.GetComponent<MeshFilter> ().mesh;
					MeshFilter lMeshFilter = lTarget.AddComponent<MeshFilter> ();
					lMeshFilter.mesh = m;

					MeshRenderer lMeshRenderer = lTarget.AddComponent<MeshRenderer> ();

					lMeshRenderer.sharedMaterial = m_Material;
				}
			}
		}

		//old code
		/*
		Mesh mesh = obj.GetComponent<Mesh>();
		GameObject temp = new GameObject("Duplicant");
		MeshFilter meshFilter = temp.AddComponent<MeshFilter>();
		temp.AddComponent<MeshRenderer>();

		Mesh tempMesh = temp.GetComponent<Mesh>();
		tempMesh.vertices = mesh.vertices;
		tempMesh.colors = mesh.colors;
		tempMesh.triangles = mesh.triangles;a
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

		m_Target = temp;
		*/
	}

	public void leaveFocus(){
		Debug.Log ("outlines OFF");
		foreach(GameObject each in m_Target){
			Destroy(each);
		}
		m_Target.Clear ();
	}
}
