using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Copied example from: http://stackoverflow.com/questions/166089/what-is-c-sharp-analog-of-c-stdpair
public class Pair<T, U> {
	public Pair() {
	}
	
	public Pair(T first, U second) {
		this.first = first;
		this.second = second;
	}
	
	public T first { get; set; }
	public U second { get; set; }
};

public class ScalePuzzle : MonoBehaviour {
	public List<GameObject> m_Cubes = new List<GameObject>();
	public List<GameObject> m_EvilScalePlacedCubes = new List<GameObject>();
	public List<GameObject> m_GoodScalePlacedCubes = new List<GameObject>();
	private List<Pair<GameObject, int>> m_EvilCubes = new List<Pair<GameObject, int>>();
	private List<Pair<GameObject, int>> m_GoodCubes = new List<Pair<GameObject, int>>();


	public GameObject m_LeftScale;
	public GameObject m_RightScale;
	public GameObject inspectCubesDummy;
	public float m_RotationSpeed = 3.0f;

	private List<Vector3> m_PlacedCubePositions = new List<Vector3>();
	private List<bool> m_CubePlaceUsed = new List<bool>();
	private GameObject r_ObjectInFocus;

	FreeLookCamera r_FreeLookCamera;

	private bool m_ExaminatingCube = false;

	private int m_CurrentIndex = 0;
	
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onRequestStartScalePuzzle", onRequestStartScalePuzzle);
		foreach(GameObject cube in m_Cubes){
			m_PlacedCubePositions.Add(cube.transform.localPosition);
			m_CubePlaceUsed.Add(true);
		}

		r_FreeLookCamera = Camera.main.transform.parent.transform.parent.gameObject.GetComponent<FreeLookCamera>();
	}
	
	// Update is called once per frame
	void Update(){
	
	}
	
	public void onRequestStartScalePuzzle(GameObject obj, bool tr){
		foreach(GameObject cube in m_Cubes){
			cube.SetActive(true);
		}
		for(int i = 0; i < m_Cubes.Count; ++i){
			//InventoryLogic.Instance.removeItem("cube key");
		}
		r_FreeLookCamera.setFreeCameraPosition(inspectCubesDummy.transform.position, inspectCubesDummy.transform.localRotation.eulerAngles);
		r_FreeLookCamera.setFreeCameraEnabled(true);
		StartCoroutine("inputLogic");
	}
	
	public GameObject getPreviousCube(ref int index){
		bool availableCube = false;
		foreach(bool cubePlaced in m_CubePlaceUsed){
			if(cubePlaced){
				availableCube = true;
				break;
			}
		}
		if(!availableCube){
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
		bool availableCube = false;
		foreach(bool cubePlaced in m_CubePlaceUsed){
			if(cubePlaced){
				availableCube = true;
				break;
			}
		}
		if(!availableCube){
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
	
	public GameObject getFirstCube(ref int index){
		bool availableCube = false;
		foreach(bool cubePlaced in m_CubePlaceUsed){
			if(cubePlaced){
				availableCube = true;
				break;
			}
		}
		if(!availableCube){
			return null;
		}

		index = 0;
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

	public void placeOnEvilScale(GameObject cube, int cubeIndex){
		m_EvilCubes.Add(new Pair<GameObject, int>(cube, cubeIndex));
		foreach(GameObject obj in m_EvilScalePlacedCubes){
			if(!obj.activeSelf){
				obj.SetActive(true);
				break;
			}
		}
	}

	public void removeFromEvilScale(){
		for(int i = m_EvilScalePlacedCubes.Count - 1; i >= 0; --i){
			GameObject obj = m_EvilScalePlacedCubes[i];
			if(obj.activeSelf){
				obj.SetActive(false);
				break;
			}
		}

		if(m_EvilCubes.Count > 0){
			GameObject cube = m_EvilCubes[m_EvilCubes.Count - 1].first;
			m_CubePlaceUsed[m_EvilCubes[m_EvilCubes.Count - 1].second] = true;
			m_EvilCubes.RemoveAt(m_EvilCubes.Count - 1);
			cube.SetActive(true);
		}
	}

	public void placeOnGoodScale(GameObject cube, int cubeIndex){
		m_GoodCubes.Add(new Pair<GameObject, int>(cube, cubeIndex));
		foreach(GameObject obj in m_GoodScalePlacedCubes){
			if(!obj.activeSelf){
				obj.SetActive(true);
				break;
			}
		}
	}

	public void removeFromGoodScale(){
		for(int i = m_GoodScalePlacedCubes.Count - 1; i >= 0; --i){
			GameObject obj = m_GoodScalePlacedCubes[i];
			if(obj.activeSelf){
				obj.SetActive(false);
				break;
			}
		}

		if(m_GoodCubes.Count > 0){
			GameObject cube = m_GoodCubes[m_GoodCubes.Count - 1].first;
			m_CubePlaceUsed[m_GoodCubes[m_GoodCubes.Count - 1].second] = true;
			m_GoodCubes.RemoveAt(m_GoodCubes.Count - 1);
			cube.SetActive(true);
		}
	}

	IEnumerator inputLogic(){
		m_CurrentIndex = 0;
		r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
		r_ObjectInFocus.renderer.material.color = Color.black;
		
		bool puzzleActive = true;
		while(puzzleActive){
			if(m_ExaminatingCube){
				yield return StartCoroutine("examineCube");
				m_ExaminatingCube = false;
				r_ObjectInFocus.transform.localPosition = m_PlacedCubePositions[m_CurrentIndex];
				r_ObjectInFocus.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
				r_ObjectInFocus.SetActive(false);
				r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
				if(r_ObjectInFocus != null){
					r_ObjectInFocus.renderer.material.color = Color.black;
				}
				else{
					// All cubes placed, now check if they were placed correctly
					bool clearedPuzzle = true;
					foreach(Pair<GameObject, int> cube in m_EvilCubes){
						InteractablePuzzle puzz = cube.first.GetComponent<InteractablePuzzle>();
						if(puzz.getPuzzleState() != "bad"){
							clearedPuzzle = false;
							break;
						}
					}
					if(clearedPuzzle){
						foreach(Pair<GameObject, int> cube in m_GoodCubes){
							InteractablePuzzle puzz = cube.first.GetComponent<InteractablePuzzle>();
							if(puzz.getPuzzleState() != "good"){
								clearedPuzzle = false;
								break;
							}
						}
					}
					if(clearedPuzzle){
						PuzzleEvent.trigger("onScalePuzzleCleared", gameObject, false);
						Debug.Log("Cleared puzzle!");
						puzzleActive = false;
						break;
					}
					else{
						Debug.Log("You failed noob!");
						yield return StartCoroutine("removeFromScale");
						r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
						r_ObjectInFocus.renderer.material.color = Color.black;
					}
				}
			}
			else {
				if(Input.GetKeyDown(KeyCode.LeftArrow)){
					--m_CurrentIndex;
					GameObject previousObject = getPreviousCube(ref m_CurrentIndex);
					if(previousObject != r_ObjectInFocus){
						r_ObjectInFocus.renderer.material.color = Color.white;
						previousObject.renderer.material.color = Color.black;
						r_ObjectInFocus = previousObject;
					}
				}
				else if(Input.GetKeyDown(KeyCode.RightArrow)){
					++m_CurrentIndex;
					GameObject nextObject = getNextCube(ref m_CurrentIndex);
					if(nextObject != r_ObjectInFocus){
						r_ObjectInFocus.renderer.material.color = Color.white;
						nextObject.renderer.material.color = Color.black;
						r_ObjectInFocus = nextObject;
					}
				}
				else if(Input.GetButtonDown("Jump")){
					// Cube is no longer placed on the table, jump over object when navigating
					m_CubePlaceUsed[m_CurrentIndex] = false;
					r_ObjectInFocus.transform.position = inspectCubesDummy.transform.position +  inspectCubesDummy.transform.forward * 0.5f;
					r_ObjectInFocus.renderer.material.color = Color.white;
					m_ExaminatingCube = true;
				}
				else if(Input.GetKeyDown(KeyCode.UpArrow) && (m_GoodCubes.Count > 0 || m_EvilCubes.Count > 0)){
					r_ObjectInFocus.renderer.material.color = Color.white;
					yield return StartCoroutine("removeFromScale");
					r_ObjectInFocus.renderer.material.color = Color.black;
				}
			}
			yield return null;
		}
	}

	IEnumerator examineCube() {
		Bounds bound = r_ObjectInFocus.transform.collider.bounds;
		Transform transform = r_ObjectInFocus.transform;
		GameObject r_ScaleInFocus = m_LeftScale;
		r_ScaleInFocus.renderer.material.color = Color.black;
		while(m_ExaminatingCube) {
			if(Input.GetButton("Fire1")) {
				float x = Input.GetAxis("Mouse X") * m_RotationSpeed;
				float y = Input.GetAxis("Mouse Y") * m_RotationSpeed;

				Vector3 pivot = bound.center;

				transform.RotateAround(pivot, Vector3.up, -x);
				transform.RotateAround(pivot, Vector3.right, y);
			}
			if(Input.GetButtonDown("Jump")) {
				if(r_ScaleInFocus == m_LeftScale) {
					placeOnEvilScale(r_ObjectInFocus, m_CurrentIndex);
				}
				else if(r_ScaleInFocus == m_RightScale) {
					placeOnGoodScale(r_ObjectInFocus, m_CurrentIndex);
				}
				r_ScaleInFocus.renderer.material.color = Color.white;
				break;
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				r_ScaleInFocus.renderer.material.color = Color.white;
				r_ScaleInFocus = m_LeftScale;
				r_ScaleInFocus.renderer.material.color = Color.black;
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)) {
				r_ScaleInFocus.renderer.material.color = Color.white;
				r_ScaleInFocus = m_RightScale;
				r_ScaleInFocus.renderer.material.color = Color.black;
			}
			yield return null;
		}
	}

	IEnumerator removeFromScale(){
		bool jump = Input.GetButtonDown("Jump");
		GameObject r_ScaleInFocus = m_LeftScale;
		r_ScaleInFocus.renderer.material.color = Color.black;
		while(true){
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				r_ScaleInFocus.renderer.material.color = Color.white;
				r_ScaleInFocus = m_LeftScale;
				r_ScaleInFocus.renderer.material.color = Color.black;
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)){
				r_ScaleInFocus.renderer.material.color = Color.white;
				r_ScaleInFocus = m_RightScale;
				r_ScaleInFocus.renderer.material.color = Color.black;
			}
			else if(Input.GetButtonDown("Jump") && !jump){
				if(r_ScaleInFocus == m_LeftScale) {
					removeFromEvilScale();
				}
				else if(r_ScaleInFocus == m_RightScale) {
					removeFromGoodScale();
				}

				if(m_GoodCubes.Count == 0 && m_EvilCubes.Count == 0){
					r_ScaleInFocus.renderer.material.color = Color.white;
					break;
				}
			}
			else if(Input.GetButtonUp("Jump")){
				jump = false;
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow)){
				r_ScaleInFocus.renderer.material.color = Color.white;
				break;
			}
			yield return null;
		}
	}
}
