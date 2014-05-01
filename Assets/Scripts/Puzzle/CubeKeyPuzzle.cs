using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeKeyPuzzle : MonoBehaviour {
	private List<Behaviour_PickUp> m_Cubes = new List<Behaviour_PickUp>();
	private int m_CubesPlaced = 0;
	
	public List<GameObject> availableCubesToPlace = new List<GameObject>();
	public GameObject inspectCubesDummy;
	
	private List<Vector3> m_PlacedCubePositions = new List<Vector3>();
	private List<bool> m_CubePlaceUsed = new List<bool>();

	private Dictionary<int, GameObject> m_PositionDictionary = new Dictionary<int, GameObject>();
	private int nrCubes;
	private bool m_StopInputLogic = false;

	FreeLookCamera r_FreeLookCamera;

	Interactable r_Interactable;

	GameObject r_ObjectInFocus;
	Vector3 m_ChosenOrigin;
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onPickupCubeKey", onPickupCubeKey);
		Messenger.AddListener<GameObject, bool>("onCubeKeyDoorUse", onCubeKeyDoorUse);
		Messenger.AddListener<GameObject, bool>("zoomOut", zoomOut);
		
		foreach(GameObject obj in availableCubesToPlace){
			m_PlacedCubePositions.Add(obj.transform.position);
			m_CubePlaceUsed.Add(false);
		}
		r_Interactable = GetComponent<Interactable>();
		r_FreeLookCamera = Camera.main.transform.parent.transform.parent.gameObject.GetComponent<FreeLookCamera>();

		nrCubes = availableCubesToPlace.Count;
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

			TweenPosition tweenPos = pCube.GetComponent<TweenPosition>();
			tweenPos.from = pCube.transform.localPosition;
			tweenPos.to = pCube.transform.localPosition + Vector3.back * 0.3f;

			m_PositionDictionary.Add (freeSpace, pCube);
		}
	}
	
	public void onCubeKeyDoorUse(GameObject obj, bool tr){
		Interactable interact = obj.GetComponent<Interactable>();
		interact.setPuzzleState("zoomed in");
		
		r_FreeLookCamera.setFreeCameraPosition(inspectCubesDummy.transform.position, inspectCubesDummy.transform.localRotation.eulerAngles);
		r_FreeLookCamera.setFreeCameraEnabled(true);

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


		StartCoroutine( "inputLogic" );


		PuzzleEvent.cancel("onUseOnly");
	}

	public void zoomOut(GameObject obj, bool tr) {
		Interactable inter = obj.GetComponent<Interactable>();
		inter.setPuzzleState("zoomed out");

		r_FreeLookCamera.resetCameraTransform();
		r_FreeLookCamera.setFreeCameraEnabled(false);

		Messenger.Broadcast<bool>("lock player input", false);

		m_StopInputLogic = true;

		PuzzleEvent.cancel("onUseOnly");
	}

	private int focusOnFirstCubeObject() {
		int index = 0;
		bool lookForObject = m_PositionDictionary.TryGetValue (index, out r_ObjectInFocus);
		while( !lookForObject && index < availableCubesToPlace.Count - 1 ) {
			index++;
			m_PositionDictionary.TryGetValue (index, out r_ObjectInFocus);
		}
		return index;
	}

	private int goToPreviousCube(int startIndex){
		int i = startIndex;
		int count = nrCubes;
		bool found = false;

		while( count > 0 && !found ){
			i = (( i - 1 ) + nrCubes ) % nrCubes;
			found = m_PositionDictionary.TryGetValue( i, out r_ObjectInFocus);
			count--;
		}

		return i;
	}

	private int goToNextCube( int startIndex ){
		int i = startIndex;
		int count = nrCubes;
		bool found = false;
		
		while( count > 0 && !found ){
			i = (( i + 1 ) + nrCubes ) % nrCubes;
			found = m_PositionDictionary.TryGetValue( i, out r_ObjectInFocus);
			count--;
		}
		
		return i;
	}

	

	IEnumerator inputLogic() {
		bool objectSelected = false;
		int index = focusOnFirstCubeObject();
		TweenPosition tweenPos = null;

		bool init = false;
		Color col = Color.white;

		m_StopInputLogic = false;

		//This while statement checks if there are any cubes in the wall or not
		while( r_ObjectInFocus != null && !m_StopInputLogic){
			if( !init ){
				//focuseObjectColor = r_ObjectInFocus.renderer.sharedMaterial.color;
				col = r_ObjectInFocus.renderer.sharedMaterial.color;
				r_ObjectInFocus.renderer.sharedMaterial.color = Color.black;
				init = true;
			}

			if( Input.GetKeyDown (KeyCode.Space) ){
				if( !objectSelected ) {
					Debug.Log ("Activating object: " + r_ObjectInFocus.name);
					tweenPos = r_ObjectInFocus.GetComponent<TweenPosition>() as TweenPosition;
					tweenPos.PlayForward();
					objectSelected = true;
					//yield return StartCoroutine("pickNextCube", index);
				}
				else if( objectSelected ){
					tweenPos.PlayReverse();
					objectSelected = false;
				}

			}

			if( Input.GetKeyDown( KeyCode.LeftArrow )  && !objectSelected ){
				r_ObjectInFocus.renderer.sharedMaterial.color = col;
				init = false;
				index = goToPreviousCube(index);
			} 
			else if ( Input.GetKeyDown( KeyCode.RightArrow )  && !objectSelected ) {
				r_ObjectInFocus.renderer.sharedMaterial.color = col;
				init = false;
				index = goToNextCube(index);
			}

			yield return null;
		}
		
		r_ObjectInFocus.renderer.sharedMaterial.color = col;
	}

	/*IEnumerator pickNextCube(int currentIndex) {

	}*/

	void mouseLogic() {
		/*if(r_ChosenObject != null) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = r_ChosenObject.transform.position.z;
			r_ChosenObject.transform.position = mousePos;
		}
		if(Input.GetButtonDown("Fire1")) {
			
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

			if (hit) {
				Debug.Log("Hit " + hitInfo.transform.gameObject.name);
				if (hitInfo.transform.gameObject.name.Contains("CubePlaced")) {
					r_ChosenObject = hitInfo.transform.gameObject;
					m_ChosenOrigin = r_ChosenObject.transform.position;
					Debug.Log ("It's working!");
				} else {
					r_ChosenObject = null;
					Debug.Log ("nopz");
				}
			} else {
				r_ChosenObject = null;
				Debug.Log("No hit");
			}
		}*/
	}
}
