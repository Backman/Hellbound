using UnityEngine;
using System.Collections;

public class getcubetexture : MonoBehaviour {


	void Start () {


		/*
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;

		Texture2D tex = renderer.material.mainTexture as Texture2D;

		tex.GetNativeTextureID ();

		Vector2 pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		*/
	}

	void Update () {
		
		RaycastHit hit;
		Ray theRay = new Ray (transform.position, new Vector3 (0f, -0.5f, 0f));

		if(Physics.Raycast(theRay, out hit)){
			if(hit.collider.GetComponent<FootstepSurface>() != null){

				Vector2 texCoord = hit.textureCoord;
				Texture textureHit = hit.transform.renderer.material.GetTexture("_MainTex");
				Debug.Log(textureHit.name + " :name of texture");
				Debug.Log (hit.transform.name + " :name of transform");

			}
			
		}
	}
}
