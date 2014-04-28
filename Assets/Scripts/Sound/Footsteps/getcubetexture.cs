using UnityEngine;
using System.Collections;

public class getcubetexture : MonoBehaviour {



	private Transform m_ObjectUnderThis;

	void Start () {

	//TODO: test if making a list works well
	// or if i should make everything into numbers
	//(ask anton)
	}

	void Update () {
		
		RaycastHit hit;
		Ray theRay = new Ray (transform.position, new Vector3 (0f, -0.5f, 0f));

		if(Physics.Raycast(theRay, out hit)){
			if(hit.collider.GetComponent<FootstepSurface>() != null){
				m_ObjectUnderThis = hit.transform;
			}
			
		}
	}

	public int getStrongestSurface(){
		int ret = 0;

		Texture maintex = m_ObjectUnderThis.renderer.material.mainTexture as Texture;


		return ret;
	}
}
