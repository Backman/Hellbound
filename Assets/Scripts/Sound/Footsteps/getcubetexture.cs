using UnityEngine;
using System.Collections;

public class getcubetexture : MonoBehaviour {

	private Transform m_ObjectUnderThis;

	void Start () {

	}

	void Update () {

		RaycastHit hit;

		Vector3 directionray = new Vector3(0f,-0.5f,0f);

		Vector3 pos = new Vector3 (transform.position.x, transform.position.y + 0.2f,transform.position.z);

		Debug.DrawRay(pos, directionray);
		Ray theRay = new Ray (transform.position, directionray);

		if(Physics.Raycast(theRay, out hit)){
			if(hit.collider.GetComponent<FootstepSurface>() != null){
				m_ObjectUnderThis = hit.transform;
			}
		}
		getStrongestSurface();
	}

	public int getStrongestSurface(){

		int ret = 0;
		Texture2D maintex = m_ObjectUnderThis.renderer.material.mainTexture as Texture2D;
		Texture2D secondarytex = m_ObjectUnderThis.renderer.material.GetTexture ("_MainTex") as Texture2D;
		Debug.Log (maintex + " :prim");
		Debug.Log (secondarytex + " :sec");
		return ret;
	}
}