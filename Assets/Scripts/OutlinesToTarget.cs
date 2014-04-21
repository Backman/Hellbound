using UnityEngine;
using System.Collections;

public class OutlinesToTarget : MonoBehaviour {
	public GameObject target;
	public Shader shader;
	// Use this for initialization
	void Start () {
		GameObject temp = Instantiate(target) as GameObject;
		temp.renderer.material.shader = shader;


		target = temp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
