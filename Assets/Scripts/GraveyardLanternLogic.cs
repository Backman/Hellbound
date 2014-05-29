using UnityEngine;
using System.Collections;

/// <summary>
/// Logic to pick up and use the player lantern
/// By Arvid Backman
/// </summary>

 public class GraveyardLanternLogic : MonoBehaviour {
	[HideInInspector] [SerializeField] GameObject m_Lantern;
	public float m_Intensity = 5.0f;
	void Start() {
		Messenger.AddListener<GameObject, bool>("pickUpLantern", pickUpLantern);
		if (Application.loadedLevelName.Contains ("Graveyard_part1")) {
			m_Lantern = GameObject.Find("lantern_belt");
			if(m_Lantern != null) {
				m_Lantern.SetActive(false);
			}
		}
	}

	public void pickUpLantern(GameObject obj, bool tr){
		Interactable inter = obj.GetComponent<Interactable>();
		if(inter != null){
			Destroy (obj);
			if(m_Lantern != null) {
				m_Lantern.SetActive(true);
			}
			GetComponent<Light>().intensity = m_Intensity;
			Messenger.Broadcast("clear focus");
		}
	}
}
