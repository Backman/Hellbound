using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeKeyPuzzle : MonoBehaviour {
	private List<Behaviour_PickUp> m_Cubes = new List<Behaviour_PickUp>();
	private List<GameObject> m_PlacedCubes = new List<GameObject>();
	private int m_CubesPlaced = 0;
	
	public List<GameObject> availableCubesToPlace = new List<GameObject>();
	public GameObject inspectCubesDummy;
	
	private List<Vector3> m_PlacedCubePositions = new List<Vector3>();
	private List<bool> m_CubePlaceUsed = new List<bool>();
	
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onPickupCubeKey", onPickupCubeKey);
		Messenger.AddListener<GameObject, bool>("onCubeKeyDoorUse", onCubeKeyDoorUse);
		
		foreach(GameObject obj in availableCubesToPlace){
			m_PlacedCubePositions.Add(obj.transform.position);
			m_CubePlaceUsed.Add(false);
		}
	}
	
	public void onPickupCubeKey(GameObject obj, bool tr){
		Behaviour_PickUp inter = obj.GetComponent<Behaviour_PickUp>();
		inter.setPuzzleState("readyToUse");
		m_Cubes.Add(inter);
	}
	
	public void placeCube(string name){
		// Get cube to place by getting object by tag
		GameObject pCube = null;
		foreach(GameObject obj in availableCubesToPlace){
			if(obj.tag == name){
				pCube = obj;
				break;
			}
		}
		
		if(pCube != null){
			int freeSpace = 0;
			int index = 0;
			foreach(bool used in m_CubePlaceUsed){
				if(!used){
					freeSpace = index;
					break;
				}
				++index;
			}
			
			pCube.transform.position = m_PlacedCubePositions[freeSpace];
			m_CubePlaceUsed[freeSpace] = true;
			pCube.SetActive(true);
		}
		FreeLookCamera camera = Camera.main.transform.parent.transform.parent.gameObject.GetComponent<FreeLookCamera>();
		camera.setFreeCameraPosition(inspectCubesDummy.transform.position, inspectCubesDummy.transform.localRotation.eulerAngles);
		camera.setFreeCameraEnabled(true);
	}
	
	public void onCubeKeyDoorUse(GameObject obj, bool tr){
		foreach(Behaviour_PickUp inter in m_Cubes){
			if(inter.getPuzzleState() == "readyToUse"){
				InventoryLogic.Instance.removeItem(inter.m_ItemName);
				inter.setPuzzleState("used");
				++m_CubesPlaced;
				placeCube(inter.name);
			}
		}
		if(m_CubesPlaced == 6){
			Debug.Log("Open door!");
		}
	}
}
