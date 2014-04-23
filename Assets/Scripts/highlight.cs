using UnityEngine;
using System.Collections;

public class highlight : MonoBehaviour {
	
	private Shader saveThis;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.K)) {
			


			renderer.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");

		}

		if (Input.GetKeyUp(KeyCode.K)) {

			renderer.material.shader = Shader.Find("Diffuse");
		}
	}
}
