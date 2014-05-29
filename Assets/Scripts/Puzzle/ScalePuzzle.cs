using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Copied example from: http://stackoverflow.com/questions/166089/what-is-c-sharp-analog-of-c-stdpair
public class Pair<T, U> {
	public Pair() {}
	
	public Pair(T first, U second) {
		this.first = first;
		this.second = second;
	}
	
	public T first { get; set; }
	public U second { get; set; }
};

/// <summary>
/// The logic behind the scale puzzle in the famine crypt.
/// The puzzle will be completed when correct cubes are on the
/// correct scales
/// By Arvid Backman and Aleksi Lindeman
/// </summary>

public class ScalePuzzle : MonoBehaviour {
	public FMOD_StudioEventEmitter m_MusicEmitter = null;
	public string m_Parameter;
	public float m_DeathParameter;
	public float m_CompleteParameter;
	public FMODAsset m_CubePlaceSound = null;
	public List<GameObject> m_Cubes = new List<GameObject>();
	public List<GameObject> m_EvilScalePlacedCubes = new List<GameObject>();
	public List<GameObject> m_GoodScalePlacedCubes = new List<GameObject>();
	private List<Pair<GameObject, int>> m_EvilCubes = new List<Pair<GameObject, int>>();
	private List<Pair<GameObject, int>> m_GoodCubes = new List<Pair<GameObject, int>>();

	public Color m_HighlightColor = Color.black;
	public GameObject m_LeftScale;
	public GameObject m_RightScale;
	public GameObject inspectCubesDummy;
	public float m_RotationSpeed = 3.0f;
	[Multiline]
	public string m_HelpText;

	private List<Vector3> m_PlacedCubePositions = new List<Vector3>();
	private List<bool> m_CubePlaceUsed = new List<bool>();
	private GameObject r_ObjectInFocus;
	private FMOD.Studio.ParameterInstance m_ParameterInstance;
	FreeLookCamera r_FreeLookCamera;

	private bool m_ExaminatingCube = false;
	private int m_CurrentIndex = 0;
	private int m_Balance = 0;
	private Animator m_ScaleAnimator;
	private float m_CurrentBlend = 0.0f;

	private bool m_LeftScaleIsFull = false;
	private bool m_RightScaleIsFull = false;

	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onRequestStartScalePuzzle", onRequestStartScalePuzzle);
		Messenger.AddListener<GameObject, bool>("openLockedDoor", openLockedDoor);
		Messenger.AddListener("onScalePuzzleDeath", onScalePuzzleDeath);
		if(m_MusicEmitter != null) {
			m_ParameterInstance = m_MusicEmitter.getParameter(m_Parameter);
		}
		foreach(GameObject cube in m_Cubes){
			m_PlacedCubePositions.Add(cube.transform.localPosition);
			m_CubePlaceUsed.Add(true);
		}
		m_ScaleAnimator = GetComponentInChildren<Animator>();
		r_FreeLookCamera = Camera.main.transform.parent.transform.parent.gameObject.GetComponent<FreeLookCamera>();
		Debug.Log ("Free look camera: " + r_FreeLookCamera);
	}
	
	public void onScalePuzzleDeath() {
		Messenger.Broadcast<bool> ("enable help text", false);
		if (m_ParameterInstance != null) {
			m_ParameterInstance.setValue (m_DeathParameter);
		}
	}
	public void onRequestStartScalePuzzle(GameObject obj, bool tr){
		foreach(GameObject cube in m_Cubes){
			cube.SetActive(true);
		}
		for(int i = 0; i < m_Cubes.Count; ++i){
			//InventoryLogic.Instance.removeItem("cube key");
		}

		Messenger.Broadcast("clear focus");
		if(m_MusicEmitter != null) {
			m_MusicEmitter.Play();
		}
		r_FreeLookCamera.setFreeCameraPosition(inspectCubesDummy.transform.position, inspectCubesDummy.transform.localRotation.eulerAngles);
		r_FreeLookCamera.setFreeCameraEnabled(true);
		StartCoroutine("inputLogic");
		StartCoroutine("updateAnimator");
		Messenger.Broadcast<bool> ("enable help text", true);
		Messenger.Broadcast<string> ("set help text", m_HelpText);
	}

	public void openLockedDoor(GameObject go, bool tr) {
		Behaviour_DoorSimple door = go.GetComponent<Behaviour_DoorSimple>();
		if(door != null) {
			door.unlockAndOpen();
		}
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

	IEnumerator updateAnimator() {
		while(true) {
			m_ScaleAnimator.SetFloat("Balance", m_CurrentBlend, 0.3f, Time.deltaTime);

			yield return null;
		}
	}

	public void placeOnEvilScale(GameObject cube, int cubeIndex){
		m_EvilCubes.Add(new Pair<GameObject, int>(cube, cubeIndex));
		Mesh cubeMesh = cube.GetComponent<MeshFilter>().mesh;
		int idx = 0;
		foreach(GameObject obj in m_EvilScalePlacedCubes){
			if(!obj.activeSelf){
				m_CurrentBlend -= 1.0f / 3.0f;
				obj.SetActive(true);
				obj.GetComponent<MeshFilter>().mesh = cubeMesh;
				if(m_CubePlaceSound != null) {
					FMOD_StudioSystem.instance.PlayOneShot(m_CubePlaceSound, obj.transform.position);
				}
				break;
			}
			idx++;
		}
		if(idx > 1) {
			m_LeftScaleIsFull = true;
		} else {
			m_LeftScaleIsFull = false;
		}
	}

	public void removeFromEvilScale(){
		for(int i = m_EvilScalePlacedCubes.Count - 1; i >= 0; --i){
			GameObject obj = m_EvilScalePlacedCubes[i];
			if(obj.activeSelf){
				m_CurrentBlend += 1.0f / 3.0f;
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
		m_LeftScaleIsFull = false;
	}

	public void placeOnGoodScale(GameObject cube, int cubeIndex){
		m_GoodCubes.Add(new Pair<GameObject, int>(cube, cubeIndex));
		Mesh cubeMesh = cube.GetComponent<MeshFilter>().mesh;
		int idx = 0;
		foreach(GameObject obj in m_GoodScalePlacedCubes){
			if(!obj.activeSelf){
				m_CurrentBlend += 1.0f / 3.0f;
				
				obj.SetActive(true);
				obj.GetComponent<MeshFilter>().mesh = cubeMesh;
				if(m_CubePlaceSound != null) {
					FMOD_StudioSystem.instance.PlayOneShot(m_CubePlaceSound, obj.transform.position);
				}
				break;
			}
			idx++;
		}
		if(idx > 1) {
			m_RightScaleIsFull = true;
		} else {
			m_RightScaleIsFull = false;
		}
	}

	public void removeFromGoodScale(){
		for(int i = m_GoodScalePlacedCubes.Count - 1; i >= 0; --i){
			GameObject obj = m_GoodScalePlacedCubes[i];
			if(obj.activeSelf){
				m_CurrentBlend -= 1.0f / 3.0f;
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
		m_RightScaleIsFull = false;
	}

	IEnumerator inputLogic(){
		m_CurrentIndex = 0;
		r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
		Color originColor = r_ObjectInFocus.renderer.material.color;
		r_ObjectInFocus.renderer.material.color = m_HighlightColor;
		bool puzzleActive = true;
		bool firstLoop = true;
		while(puzzleActive){
			if(m_ExaminatingCube){
				yield return StartCoroutine("examineCube");
				m_ExaminatingCube = false;
				r_ObjectInFocus.transform.localPosition = m_PlacedCubePositions[m_CurrentIndex];
				r_ObjectInFocus.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
				r_ObjectInFocus.SetActive(false);
				r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
				if(r_ObjectInFocus != null){
					r_ObjectInFocus.renderer.material.color = m_HighlightColor;
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
						Messenger.Broadcast<bool>("lock player input", false);
						StopCoroutine("updateAnimator");
						r_FreeLookCamera.resetCameraTransform();
						r_FreeLookCamera.setFreeCameraEnabled(false);

						puzzleActive = false;
						GetComponent<Interactable>().enabled = false;
						Messenger.Broadcast<bool> ("enable help text", false);

						if(m_ParameterInstance != null) {
							m_ParameterInstance.setValue(m_CompleteParameter);
						}
						break;
					}
					else{
						Debug.Log("You failed noob!");
						yield return StartCoroutine("removeFromScale");
						r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
						r_ObjectInFocus.renderer.material.color = m_HighlightColor;
					}
				}
			}
			else {
				if(InputManager.getButtonDown(InputManager.Button.Left, true)){
					--m_CurrentIndex;
					GameObject previousObject = getPreviousCube(ref m_CurrentIndex);
					if(previousObject != r_ObjectInFocus){
						r_ObjectInFocus.renderer.material.color = originColor;
						previousObject.renderer.material.color = m_HighlightColor;
						r_ObjectInFocus = previousObject;
						//originColor = r_ObjectInFocus.renderer.material.color;
					}
				}
				else if(InputManager.getButtonDown(InputManager.Button.Right, true)){
					++m_CurrentIndex;
					GameObject nextObject = getNextCube(ref m_CurrentIndex);
					if(nextObject != r_ObjectInFocus){
						r_ObjectInFocus.renderer.material.color = originColor;
						nextObject.renderer.material.color = m_HighlightColor;
						r_ObjectInFocus = nextObject;
						//originColor = r_ObjectInFocus.renderer.material.color;
					}
				}
				else if(InputManager.getButtonDown(InputManager.Button.Use) && !firstLoop){
					// Cube is no longer placed on the table, jump over object when navigating
					m_CubePlaceUsed[m_CurrentIndex] = false;
					r_ObjectInFocus.transform.position = inspectCubesDummy.transform.position +  inspectCubesDummy.transform.forward * 0.5f;
					r_ObjectInFocus.renderer.material.color = originColor;
					m_ExaminatingCube = true;
				}
				else if(InputManager.getButtonDown(InputManager.Button.Forward) && (m_GoodCubes.Count > 0 || m_EvilCubes.Count > 0)){
					r_ObjectInFocus.renderer.material.color = originColor;
					yield return StartCoroutine("removeFromScale");
					r_ObjectInFocus = getFirstCube(ref m_CurrentIndex);
					originColor = r_ObjectInFocus.renderer.material.color;
					r_ObjectInFocus.renderer.material.color = m_HighlightColor;
				}
			}
			firstLoop = false;
			yield return null;
		}
	}

	IEnumerator examineCube() {
		Bounds bound = r_ObjectInFocus.transform.collider.bounds;
		Transform transform = r_ObjectInFocus.transform;
		GameObject r_ScaleInFocus = m_LeftScale;
		if(m_LeftScaleIsFull) {
			r_ScaleInFocus = m_RightScale;
		}
		Color originColor = r_ScaleInFocus.renderer.material.color;
		r_ScaleInFocus.renderer.material.color = m_HighlightColor;
		while(m_ExaminatingCube) {
			if(InputManager.getButton(InputManager.Button.Rotate)) {
				float x = Input.GetAxis("Mouse X") * m_RotationSpeed;
				float y = Input.GetAxis("Mouse Y") * m_RotationSpeed;

				Vector3 pivot = bound.center;

				transform.RotateAround(pivot, Vector3.up, -x);
				transform.RotateAround(pivot, Vector3.right, -y);
			}
			
			float x1 = Input.GetAxis("HorizontalAxis") * m_RotationSpeed * 2.0f;
			float y1 = Input.GetAxis("VerticalAxis") * m_RotationSpeed * 2.0f;
			if(Mathf.Abs(x1) > 0.1f || Mathf.Abs(y1) > 0.1f){
				Vector3 pivot = bound.center;
				
				transform.RotateAround(pivot, Vector3.up, -x1);
				transform.RotateAround(pivot, Vector3.right, -y1);
			}
			
			if(InputManager.getButtonDown(InputManager.Button.Left) && !m_LeftScaleIsFull) {
				r_ScaleInFocus.renderer.material.color = originColor;
				r_ScaleInFocus = m_LeftScale;
				originColor = r_ScaleInFocus.renderer.material.color;
				r_ScaleInFocus.renderer.material.color = m_HighlightColor;
			}
			else if(InputManager.getButtonDown(InputManager.Button.Right) && !m_RightScaleIsFull) {
				r_ScaleInFocus.renderer.material.color = originColor;
				r_ScaleInFocus = m_RightScale;
				originColor = r_ScaleInFocus.renderer.material.color;
				r_ScaleInFocus.renderer.material.color = m_HighlightColor;
			}
			
			if(InputManager.getButtonDown(InputManager.Button.Use)) {
				if(r_ScaleInFocus == m_LeftScale) {
					placeOnEvilScale(r_ObjectInFocus, m_CurrentIndex);
				}
				else if(r_ScaleInFocus == m_RightScale) {
					placeOnGoodScale(r_ObjectInFocus, m_CurrentIndex);
				}
				r_ScaleInFocus.renderer.material.color = originColor;
				break;
			}
			yield return null;
		}
	}

	IEnumerator removeFromScale(){
		bool use = InputManager.getButtonDown(InputManager.Button.Use);
		GameObject r_ScaleInFocus = m_LeftScale;
		Color originColor = r_ScaleInFocus.renderer.material.color;
		r_ScaleInFocus.renderer.material.color = m_HighlightColor;
		while(true){
			if(InputManager.getButtonDown(InputManager.Button.Left, true)){
				r_ScaleInFocus.renderer.material.color = originColor;
				r_ScaleInFocus = m_LeftScale;
				originColor = r_ScaleInFocus.renderer.material.color;
				r_ScaleInFocus.renderer.material.color = m_HighlightColor;
			}
			else if(InputManager.getButtonDown(InputManager.Button.Right, true)){
				r_ScaleInFocus.renderer.material.color = originColor;
				r_ScaleInFocus = m_RightScale;
				originColor = r_ScaleInFocus.renderer.material.color;
				r_ScaleInFocus.renderer.material.color = m_HighlightColor;
			}
			else if(InputManager.getButtonDown(InputManager.Button.Use) && !use){
				if(r_ScaleInFocus == m_LeftScale) {
					removeFromEvilScale();
				}
				else if(r_ScaleInFocus == m_RightScale) {
					removeFromGoodScale();
				}

				if(m_GoodCubes.Count == 0 && m_EvilCubes.Count == 0){
					r_ScaleInFocus.renderer.material.color = originColor;
					break;
				}
			}
			else if(InputManager.getButtonDown(InputManager.Button.Use)){
				use = false;
			}
			else if(InputManager.getButtonDown(InputManager.Button.Backward)){
				bool cubes = false;
				int idx = 0;
				foreach(bool v in m_CubePlaceUsed){
					cubes = v;

					if(cubes) {
						idx++;
					}
				}
				Debug.Log ("available cubes: " + idx);
				if(cubes) {
					r_ScaleInFocus.renderer.material.color = originColor;
					break;
				}
			}
			yield return null;
		}
	}
}
