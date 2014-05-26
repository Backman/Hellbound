using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Logic for the cube combination puzzle
/// to open the door in the famine crypt
/// By Arvid Backman and Aleksi Lindeman
/// </summary>

public class CubeKeyPuzzle : MonoBehaviour {
	private List<Behaviour_PickUp> m_Cubes = new List<Behaviour_PickUp>();
	private int m_CubesPlaced = 0;

	public FMODAsset m_DoorOpenSound = null;
	public FMODAsset m_UseCubeSound = null;

	public Color m_HighlightColor = Color.black;
	public List<GameObject> availableCubesToPlace = new List<GameObject>();
	public GameObject inspectCubesDummy;
	
	private List<Vector3> m_PlacedCubePositions = new List<Vector3>();
	private List<bool> m_CubePlaceUsed = new List<bool>();

	private Dictionary<int, GameObject> m_PositionDictionary = new Dictionary<int, GameObject>();
	private int nrCubes;
	private bool m_StopInputLogic = false;
	
	private bool m_CubesSwitching = false;

	private float m_MovementCooldown = 0.5f;

	FreeLookCamera r_FreeLookCamera;

	Interactable r_Interactable;

	GameObject r_ObjectInFocus, r_SecondObjectInFocus;
	Vector3 m_ChosenOrigin;
	// Use this for initialization
	void Start () {
		Messenger.AddListener<GameObject, bool>("onPickupCubeKey", onPickupCubeKey);
		Messenger.AddListener<GameObject, bool>("onCubeKeyDoorUse", onCubeKeyDoorUse);
		Messenger.AddListener<GameObject, bool>("onRequestOpenCubeDoor", onRequestOpenCubeDoor);
		
		foreach(GameObject obj in availableCubesToPlace){
			m_PlacedCubePositions.Add(obj.transform.localPosition);
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
			pCube.transform.localPosition = m_PlacedCubePositions[freeSpace];
			m_CubePlaceUsed[freeSpace] = true;
			pCube.GetComponent<InteractablePuzzle>().setPuzzleState((freeSpace + 1).ToString());
			pCube.SetActive(true);

			//TweenPosition tweenPos = pCube.GetComponent<TweenPosition>();
			//tweenPos.from = pCube.transform.localPosition;
			//tweenPos.to = pCube.transform.localPosition + Vector3.back * 0.3f;

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
				//++m_CubesPlaced;
				placeCube(inter.name);
			}
		}

		StartCoroutine( "inputLogic" );
		Messenger.Broadcast("clear focus");

		PuzzleEvent.cancel("onUseOnly");
		
		PuzzleEvent.trigger("onCubesSwitchedPlaces", gameObject, true);
	}

	public void zoomOut(GameObject obj, bool tr) {
		Interactable inter = obj.GetComponent<Interactable>();
		inter.setPuzzleState("zoomed out");

		r_FreeLookCamera.resetCameraTransform();
		r_FreeLookCamera.setFreeCameraEnabled(false);
		Messenger.Broadcast<bool>("lock player input", false);

		m_StopInputLogic = true;
		
		if(tr) {
			Messenger.Broadcast<Interactable>("add focus", GetComponent<Interactable>());
		}
	}

	private int focusOnFirstCubeObject(out GameObject obj) {
		int index = 0;
		bool lookForObject = m_PositionDictionary.TryGetValue (index, out obj);
		while( !lookForObject && index < availableCubesToPlace.Count - 1 ) {
			index++;
			m_PositionDictionary.TryGetValue (index, out obj);
		}
		return index;
	}

	private int goToPreviousCube(int startIndex, out GameObject obj){
		int i = startIndex;
		int count = nrCubes;
		bool found = false;

		while( count > 0 && !found ){
			i = (( i - 1 ) + nrCubes ) % nrCubes;
			found = m_PositionDictionary.TryGetValue( i, out obj);
			if(found){
				return i;
			}
			count--;
		}

		obj = null;
		return i;
	}

	private int goToNextCube( int startIndex, out GameObject obj){
		int i = startIndex;
		int count = nrCubes;
		bool found = false;
		
		while( count > 0 && !found ){
			i = (( i + 1 ) + nrCubes ) % nrCubes;
			found = m_PositionDictionary.TryGetValue( i, out obj);
			if(found){
				return i;
			}
			count--;
		}
		
		obj = null;
		return i;
	}

	

	IEnumerator inputLogic() {
		bool objectSelected = false;
		int index = focusOnFirstCubeObject(out r_ObjectInFocus);
		TweenPosition tweenPos = null;

		bool init = false;
		bool waitForRelease = InputManager.getButtonDown(InputManager.Button.Use);
		Color col = Color.white;

		m_StopInputLogic = false;

		//This while statement checks if there are any cubes in the wall or not
		while( r_ObjectInFocus != null && !m_StopInputLogic){
			if( InputManager.getButtonDown(InputManager.Button.Run) && init) {
				zoomOut(gameObject, true);
			}
			
			if( !init ){
				//focuseObjectColor = r_ObjectInFocus.renderer.sharedMaterial.color;
				col = r_ObjectInFocus.renderer.sharedMaterial.color;
				r_ObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.15f);
				init = true;
			}

			if( InputManager.getButtonDown(InputManager.Button.Use) && !waitForRelease){
				if( !objectSelected ) {
					Debug.Log ("Activating object: " + r_ObjectInFocus.name);
					Vector3 newPos = r_ObjectInFocus.transform.localPosition;
					newPos.z -= 0.3f;
					tweenPos = TweenPosition.Begin(r_ObjectInFocus, 0.1f, newPos);
					//tweenPos.PlayForward();
					objectSelected = true;
					if(m_UseCubeSound != null) {
						FMOD_StudioSystem.instance.PlayOneShot(m_UseCubeSound, r_ObjectInFocus.transform.position);
					}
				}
				else if( objectSelected ){
					Vector3 newPos = r_ObjectInFocus.transform.localPosition;
					newPos.z = m_PlacedCubePositions[0].z;
					tweenPos = TweenPosition.Begin(r_ObjectInFocus, 0.1f, newPos);
					//tweenPos.PlayReverse();
					objectSelected = false;
					if(m_UseCubeSound != null) {
						FMOD_StudioSystem.instance.PlayOneShot(m_UseCubeSound, r_ObjectInFocus.transform.position);
					}
				}
			}
			else if(InputManager.getButtonUp(InputManager.Button.Use)){
				if( objectSelected ) {
					//r_ObjectInFocus.renderer.sharedMaterial.color = col;
					r_ObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
					yield return StartCoroutine("pickNextCube", index);
					objectSelected = false;
					index = focusOnFirstCubeObject(out r_ObjectInFocus);
					init = false;
				}
				waitForRelease = false;
			}

			if( InputManager.getButtonDown(InputManager.Button.Left, true) && !objectSelected){
				//r_ObjectInFocus.renderer.sharedMaterial.color = col;
				r_ObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
				init = false;
				index = goToPreviousCube(index, out r_ObjectInFocus);
			} 
			else if ( InputManager.getButtonDown(InputManager.Button.Right, true) && !objectSelected) {
				//r_ObjectInFocus.renderer.sharedMaterial.color = col;
				r_ObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
				init = false;
				index = goToNextCube(index, out r_ObjectInFocus);
			}
			
			yield return null;
		}
		if(r_ObjectInFocus != null){
			//r_ObjectInFocus.renderer.sharedMaterial.color = col;
			r_ObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
		}
		else if(!init){
			bool released = false;
			while(!m_StopInputLogic){
				if(InputManager.getButtonUp(InputManager.Button.Run) || released) {
					if(InputManager.getButtonDown(InputManager.Button.Run)){
						zoomOut(gameObject, true);
					}
					released = true;
				}
				yield return null;
			}
		}
	}

	IEnumerator pickNextCube(int currentIndex) {
		bool objectSelected = false;
		int index = focusOnFirstCubeObject(out r_SecondObjectInFocus);
		TweenPosition tweenPos = null;
		
		bool init = false;
		Color col = Color.white;
		
		m_StopInputLogic = false;
		
		//This while statement checks if there are any cubes in the wall or not
		while( r_SecondObjectInFocus != null && !m_StopInputLogic){
			if( InputManager.getButtonDown(InputManager.Button.Run) && init && !m_CubesSwitching ) {
				//m_StopInputLogic = true;
				Vector3 newPos = r_ObjectInFocus.transform.localPosition;
				newPos.z = m_PlacedCubePositions[0].z;
				tweenPos = TweenPosition.Begin(r_ObjectInFocus, 0.1f, newPos);
				zoomOut(gameObject, true);
			}
			
			if( !init ){
				//focuseObjectColor = r_ObjectInFocus.renderer.sharedMaterial.color;
				col = r_SecondObjectInFocus.renderer.sharedMaterial.color;
				//r_SecondObjectInFocus.renderer.sharedMaterial.color = m_HighlightColor;
				r_SecondObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.15f);
				init = true;
			}
			
			if( InputManager.getButtonDown(InputManager.Button.Use) ){
				if(r_SecondObjectInFocus == r_ObjectInFocus){
					Vector3 newPos = r_ObjectInFocus.transform.localPosition;
					newPos.z = m_PlacedCubePositions[0].z;
					tweenPos = TweenPosition.Begin(r_ObjectInFocus, 0.1f, newPos);
					//tweenPos.PlayReverse();
					objectSelected = false;
					if(m_UseCubeSound != null) {
						FMOD_StudioSystem.instance.PlayOneShot(m_UseCubeSound, r_ObjectInFocus.transform.position);
					}
					break;
				}
				else if( !objectSelected ) {
					Debug.Log ("Activating object: " + r_SecondObjectInFocus.name);
					Vector3 newPos = r_SecondObjectInFocus.transform.localPosition;
					newPos.z -= 0.3f;
					tweenPos = TweenPosition.Begin(r_SecondObjectInFocus, 0.1f, newPos);
					//tweenPos.PlayForward();
					objectSelected = true;
					if(m_UseCubeSound != null) {
						FMOD_StudioSystem.instance.PlayOneShot(m_UseCubeSound, r_ObjectInFocus.transform.position);
					}
					tweenPos.AddOnFinished(onMovedOut);
					m_CubesSwitching = true;
					while(m_CubesSwitching){
						yield return null;
					}
					//r_SecondObjectInFocus.renderer.sharedMaterial.color = col;
					r_SecondObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
					break;
					
					//yield return StartCoroutine("pickNextCube", index);
				}
			}
			
			if( InputManager.getButtonDown(InputManager.Button.Left)  && !objectSelected ){
				//r_SecondObjectInFocus.renderer.sharedMaterial.color = col;
				r_SecondObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
				init = false;
				index = goToPreviousCube(index, out r_SecondObjectInFocus);
			} 
			else if ( InputManager.getButtonDown(InputManager.Button.Right)  && !objectSelected ) {
				//r_SecondObjectInFocus.renderer.sharedMaterial.color = col;
				r_SecondObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
				init = false;
				index = goToNextCube(index, out r_SecondObjectInFocus);
			}
			
			yield return null;
		}
		
		//r_SecondObjectInFocus.renderer.sharedMaterial.color = col;
		r_SecondObjectInFocus.renderer.material.SetFloat("_EmissionLM", 0.0f);
	}

	void onMovedOut(){
		//Debug.Log("Tween finished!");
		r_SecondObjectInFocus.GetComponent<TweenPosition>().RemoveOnFinished(new EventDelegate(onMovedOut));
		Vector3 newPos = r_SecondObjectInFocus.transform.localPosition;
		newPos.z -= 0.5f;
		TweenPosition tweenPos = TweenPosition.Begin(r_SecondObjectInFocus, 0.2f, newPos);
		tweenPos.AddOnFinished(onMovedOutFurther);
	}
	
	void onMovedOutFurther(){
		//Debug.Log("Tween finished2!");
		r_SecondObjectInFocus.GetComponent<TweenPosition>().RemoveOnFinished(new EventDelegate(onMovedOutFurther));
		
		Vector3 pos = r_ObjectInFocus.transform.localPosition;
		Vector3 pos2 = r_SecondObjectInFocus.transform.localPosition;
		
		float tempX = pos2.x;
		float tempY = pos2.y;
		pos2.x = pos.x;
		pos2.y = pos.y;
		pos.x = tempX;
		pos.y = tempY;
		
		TweenPosition.Begin(r_ObjectInFocus, 0.3f, pos);
		TweenPosition tweenPos = TweenPosition.Begin(r_SecondObjectInFocus, 0.3f, pos2);
		tweenPos.AddOnFinished(onCubesSwitchedPos);
	}
	
	void onCubesSwitchedPos(){
		//Debug.Log("Tween finished3!");
		r_SecondObjectInFocus.GetComponent<TweenPosition>().RemoveOnFinished(new EventDelegate(onCubesSwitchedPos));
		
		Vector3 pos = r_ObjectInFocus.transform.localPosition;
		Vector3 pos2 = r_SecondObjectInFocus.transform.localPosition;
		
		pos.z += 0.3f;
		pos2.z = pos.z;
		TweenPosition.Begin(r_ObjectInFocus, 0.3f, pos);
		TweenPosition tweenPos = TweenPosition.Begin(r_SecondObjectInFocus, 0.3f, pos2);
		tweenPos.AddOnFinished(onCubesFinishedSwitchingPos);
	}
	
	void onCubesFinishedSwitchingPos(){
		//Debug.Log("Tween finished4!");
		int i = 0;
		int index = 0;
		int index2 = 0;
		foreach(GameObject obj in m_PositionDictionary.Values){
			if(obj == r_ObjectInFocus){
				Debug.Log("Obj1 index: "+i);
				index = i;
			}
			else if(obj == r_SecondObjectInFocus){
				Debug.Log("Obj2 index: "+i);
				index2 = i;
			}
			++i;
		}
		m_PositionDictionary[index] = r_SecondObjectInFocus;
		m_PositionDictionary[index2] = r_ObjectInFocus;
		Debug.Log("new obj1 index: "+index);
		Debug.Log("new obj2 index: "+index2);
		
		r_SecondObjectInFocus.GetComponent<TweenPosition>().RemoveOnFinished(new EventDelegate(onCubesFinishedSwitchingPos));
		m_CubesSwitching = false;
		
		r_ObjectInFocus.GetComponent<InteractablePuzzle>().setPuzzleState((index2 + 1).ToString());
		r_SecondObjectInFocus.GetComponent<InteractablePuzzle>().setPuzzleState((index + 1).ToString());
		PuzzleEvent.trigger("onCubesSwitchedPlaces", gameObject, true);
	}
	
	public void onRequestOpenCubeDoor(GameObject obj, bool tr){
		if(m_DoorOpenSound != null) {
			FMOD_StudioSystem.instance.PlayOneShot(m_DoorOpenSound, obj.transform.position);
		}
		obj.transform.parent.gameObject.GetComponent<UIPlayTween>().Play(true);
		obj.GetComponent<Interactable>().enabled = false;

		zoomOut(gameObject, false);

		foreach(GameObject go in availableCubesToPlace) {
			go.SetActive(false);
		}

		foreach(Behaviour_PickUp inter in m_Cubes) {
			InventoryLogic.Instance.addItem (inter.m_ItemName, inter.m_ItemThumbnail);
		}
		Debug.Log("Open door!");
	}
}
