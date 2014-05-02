using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScalePuzzle : MonoBehaviour {
	public List<GameObject> m_Cubes = new List<GameObject>();
	public List<GameObject> m_EvilScalePlacedCubes = new List<GameObject>();
	public List<GameObject> m_GoodScalePlacedCubes = new List<GameObject>();
	
	private List<Vector3> m_PlacedCubePositions = new List<Vector3>();
	private List<bool> m_CubePlaceUsed = new List<bool>();
	private GameObject r_ObjectInFocus;
	
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onRequestStartScalePuzzle", onRequestStartScalePuzzle);
		foreach(GameObject cube in m_Cubes){
			m_PlacedCubePositions.Add(cube.transform.localPosition);
			m_CubePlaceUsed.Add(true);
		}
	}
	
	// Update is called once per frame
	void Update(){
	
	}
	
	public void onRequestStartScalePuzzle(GameObject obj, bool tr){
		foreach(GameObject cube in m_Cubes){
			cube.SetActive(true);
		}
		StartCoroutine("inputLogic");
	}
	
	public GameObject getPreviousCube(ref int index){
		if(m_CubePlaceUsed.Count == 1){
			index = 0;
			return m_Cubes[0];
		}
		else if(m_CubePlaceUsed.Count == 0){
			return null;
		}
		
		while(true){
			if(index < 0){
				index = m_CubePlaceUsed.Count - 1;
			}
			if(m_CubePlaceUsed[index]){
				return m_Cubes[index];
			}
			--index;
		}
		return null;
	}
	
	public GameObject getNextCube(ref int index){
		if(m_CubePlaceUsed.Count == 1){
			index = 0;
			return m_Cubes[0];
		}
		else if(m_CubePlaceUsed.Count == 0){
			return null;
		}
		
		while(true){
			if(index >= m_CubePlaceUsed.Count){
				index = 0;
			}
			if(m_CubePlaceUsed[index]){
				return m_Cubes[index];
			}
			++index;
		}
		return null;
	}
	
	public GameObject getFirstCube(){
		if(m_CubePlaceUsed.Count == 1){
			return m_Cubes[0];
		}
		else if(m_CubePlaceUsed.Count == 0){
			return null;
		}
		
		int index = 0;
		while(true){
			if(index >= m_CubePlaceUsed.Count){
				index = 0;
			}
			if(m_CubePlaceUsed[index]){
				return m_Cubes[index];
			}
			++index;
		}
		return null;
	}
	
	IEnumerator inputLogic(){
		int currentIndex = 0;
		r_ObjectInFocus = getFirstCube();
		r_ObjectInFocus.renderer.material.color = Color.black;
		
		bool puzzleActive = true;
		while(puzzleActive){
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				--currentIndex;
				GameObject previousObject = getPreviousCube(ref currentIndex);
				if(previousObject != r_ObjectInFocus){
					r_ObjectInFocus.renderer.material.color = Color.white;
					previousObject.renderer.material.color = Color.black;
					r_ObjectInFocus = previousObject;
				}
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)){
				++currentIndex;
				GameObject nextObject = getNextCube(ref currentIndex);
				if(nextObject != r_ObjectInFocus){
					r_ObjectInFocus.renderer.material.color = Color.white;
					nextObject.renderer.material.color = Color.black;
					r_ObjectInFocus = nextObject;
				}
			}
			yield return null;
		}
	}
}
