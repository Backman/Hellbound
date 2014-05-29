using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Distance culling.
/// 
/// This scirpt culls certain properties, depending on the scirpts settings, when the player is futher away than m_CullDistance.
/// Used to gain performance on the graveyard scene primarily.
/// 
/// !!!!!IMPORTANT!!!!!
/// This script can only be used on objects that do not change child hierarcy layout
/// 
/// 
/// Created by Simon Jonasson
/// </summary>
public class DistanceCulling : MonoBehaviour {
	public enum CullMode { Render, GameObj };
	public int m_CullDistance = 100;

	[TooltipAttribute("Decides what will be deactivated when the object is culled. Can NOT be changed after the game has been started.\n" +
					  "Render:  Disable parent plus all childrens renderers.\n" +
					  "GameObj: Disable all child objects, NOT the parent.")]
	public CullMode m_CullMode = CullMode.Render;
#if UNITY_EDITOR
	private int m_LastCullDistance;
#endif	
	private bool m_ChildRendererActive = true;
	private int m_SqrCullDistance;
	private Transform r_Player;
	private List<MeshRenderer> r_ChildRenderer = new List<MeshRenderer>();
	private List<GameObject> r_ChildObject = new List<GameObject>();
	private delegate void MyDelegate();
	MyDelegate activate, deactivate;


	void Start () {
		r_Player = GameObject.FindGameObjectWithTag ("Player").transform;	//Keep a reference to the player, it is slow to look it up all the time
		m_SqrCullDistance = m_CullDistance * m_CullDistance; 				//We use squared distances to avoid using sqauare roots.

		#if UNITY_EDITOR
		m_LastCullDistance = m_CullDistance;
		#endif

		//Depending on CullMode, we assign the delegates different behaviours
		switch( m_CullMode ){
		case CullMode.Render:
			activate += activateChildRenderers;
			deactivate += deactivateChildRenderers;
			break;
		case CullMode.GameObj: 
			activate += activateChildObjects;
			deactivate += deactivateChildObjects;
			break;
		}
		StartCoroutine ("checkDist");	//It is unnecessary to do distance culling every frame. 
										//Instead, this coroutine will perform the distance culling two secunds
	}

#if UNITY_EDITOR
	void Update(){
		if( m_LastCullDistance != m_CullDistance ){	//If someone changed CullDistance in the Editor. In release, the cull distance cannot be changed and thus, we don't need this update.
			m_SqrCullDistance = m_CullDistance * m_CullDistance;
			m_LastCullDistance = m_CullDistance;
		}
	}
#endif
	
	IEnumerator checkDist(){
		yield return new WaitForSeconds (Random.Range (0.1f, 1.5f));

		loadChildResources ();	

		//We use squared distances to avoid using square roots, since roots are slow
		while (true) {
			if( (r_Player.position - transform.position).sqrMagnitude > m_SqrCullDistance && m_ChildRendererActive ){	//If the player is closer than m_CullDist
				deactivate();
				m_ChildRendererActive = false;
			}
			else if( (r_Player.position - transform.position).sqrMagnitude < m_SqrCullDistance && !m_ChildRendererActive ){	//If the player is further away than m_CullDist
				activate();
				m_ChildRendererActive = true;
			}

			yield return new WaitForSeconds(2.0f);
		}
	}

	//We don't want to look up all entitys that we want to affect each time we do the culling.
	//Instead, we store references to all the entitys on startup, and use the references later
	void loadChildResources(){
		switch( m_CullMode ){

		case CullMode.Render:
			var meshes = GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer mesh in meshes) {
				r_ChildRenderer.Add(mesh);
			}
			break;
		
		case CullMode.GameObj:
			foreach( Transform t in transform ){
				r_ChildObject.Add( t.gameObject );
			}
			break;
		}
	}

	//Disables all renderers
	void deactivateChildRenderers(){
		foreach (MeshRenderer mesh in r_ChildRenderer) {
			mesh.enabled = false;
		}
	}
	//Enables all renderers
	void activateChildRenderers(){
		foreach (MeshRenderer mesh in r_ChildRenderer) {
			mesh.enabled = true;
		}
	}

	//Enable all objects
	void activateChildObjects(){ 
		foreach( GameObject obj in r_ChildObject ){
			obj.SetActive( true );
		}
	}
	//Disable all objects
	void deactivateChildObjects() { 
		foreach( GameObject obj in r_ChildObject ){
			obj.SetActive( false );
		}
	}
}
