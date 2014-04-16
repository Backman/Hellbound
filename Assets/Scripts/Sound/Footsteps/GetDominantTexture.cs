using UnityEngine;
using System.Collections;

public class GetDominantTexture : MonoBehaviour {

	public float m_SurfaceIndex = 0;

	private TerrainData m_TerrainData;
	private Vector3 m_TerrainPos;

	private bool m_SkipEveryThing = false;


	void Start () {
		if(Terrain.activeTerrain != null){
			Terrain terrain = Terrain.activeTerrain;
			m_TerrainData = terrain.terrainData;
			m_TerrainPos = terrain.transform.position;
		}
		else{
			m_SkipEveryThing = true;
		}
	}


	public float GetTextureAt(Vector3 WorldPos){
		if (!m_SkipEveryThing) {
			
			float terrainX = ((WorldPos.x - m_TerrainPos.x) / m_TerrainData.size.x) * m_TerrainData.alphamapWidth;
			float terrainZ = ((WorldPos.z - m_TerrainPos.z) / m_TerrainData.size.z) * m_TerrainData.alphamapHeight;
			
			float[,,] splatmapData = m_TerrainData.GetAlphamaps ((int)(terrainX), (int)(terrainZ), 1, 1);
			
			float[] cellMix = new float[(splatmapData.GetUpperBound(2)+1)];
			
			for (int n = 0; n < cellMix.Length; n++) {
				cellMix[n] = splatmapData[0,0,n];
			}
			
			float maxMix = 0;
			float maxIndex = 0;
			
			for(int n = 0; n < cellMix.Length; n++){
				if(cellMix[n] > maxMix){
					maxIndex = n;
					maxMix = cellMix[n];
				}
			}
			return maxIndex;
		}
		return -0.123f;
	}
}
