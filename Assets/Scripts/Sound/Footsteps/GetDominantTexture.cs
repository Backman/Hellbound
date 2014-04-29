using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetDominantTexture : MonoBehaviour {

	public float m_SurfaceType = 0;
	
	private Terrain r_Terrain;
	private TerrainData r_TerrainData;
	private Vector3 r_TerrainPos;

	private bool m_TerrainExist = true;

	void Start(){
		if(Terrain.activeTerrain != null){
			r_Terrain = Terrain.activeTerrain;
			r_TerrainData = r_Terrain.terrainData;
			r_TerrainPos = r_Terrain.transform.position;
		}
	}

	
	private void UpdateTerrain(Terrain newTerrain){
		r_Terrain = newTerrain;
		r_TerrainData = r_Terrain.terrainData;
		r_TerrainPos = r_Terrain.transform.position;
	}


	void OnTriggerEnter(Collider other){
		if (other.GetComponent<Terrain>() != null) {			
			UpdateTerrain(other.GetComponent<Terrain>());			
			m_SurfaceType = GetMostDominantTexture(gameObject.transform.position);				
		}
	}

	
	public int GetMostDominantTexture(Vector3 worldPos) {

		int x = (int)(((worldPos.x - r_TerrainPos.x) / r_TerrainData.size.x) * r_TerrainData.alphamapWidth);
		int z = (int)(((worldPos.z - r_TerrainPos.z) / r_TerrainData.size.z) * r_TerrainData.alphamapHeight);
		
		float[,,] alphamap = r_TerrainData.GetAlphamaps(x,z,1,1);
		
		float[] textureMix = new float[alphamap.GetUpperBound(2)+1];

		for (int n = 0; n < textureMix.Length; n++){
			textureMix[n] = alphamap[0,0,n];
		}

		float mostDominant = 0;
		int dominantSurface = 0;

		for (int n = 0 ; n < textureMix.Length; n++){
			if (textureMix[n] > mostDominant){
				dominantSurface = n;
				mostDominant = textureMix[n];
			}
		}

		return dominantSurface;		
	}	
}