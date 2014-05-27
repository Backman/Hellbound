using UnityEngine;
using System.Collections;

/// <summary>
/// Outline drawer.
/// 
/// Editor utility for displaying mesh colliders when an object is not selected
/// 
/// Created by Simon Jonasson
/// </summary>
public class OutlineDrawer : MonoBehaviour {
	public bool m_IsDrawn = true;
	public Color m_Color = Color.white;

	void OnDrawGizmos(){
		if( !m_IsDrawn )
			return;


		MeshCollider col = gameObject.GetComponent<MeshCollider> ();
		if( col != null ){
			Vector3[] verts = col.sharedMesh.vertices;
			if( verts.Length < 2 )
				return;

			for( int i = 1; i < verts.Length; ++i ){
				Gizmos.color = m_Color;
				Gizmos.DrawLine( verts[i-1], verts[i] );
			}
			Gizmos.DrawLine( verts[ verts.Length-1 ], verts[0] );

		}
	}
}
