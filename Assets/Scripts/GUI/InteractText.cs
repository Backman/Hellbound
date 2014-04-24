using UnityEngine;
using System.Collections;

public class InteractText : MonoBehaviour {

	public UILabel m_ExamineText;
	public UILabel m_PickupText;
	public UILabel m_UseText;
	public UIGrid r_Grid;

	public GameObject m_Target;
	public Vector3 m_Offset = Vector3.zero;

	private Camera m_WorldCamera;
	private Camera m_GUICamera;
	private Transform r_Transform;
	private Vector3 m_Position;
	private UIPlayTween r_Tweener;

	private bool m_HasExamine = true;
	private bool m_HasPickup = true;
	private bool m_CanBeUsed = true;

	public bool HasExamine {
		get { return m_HasExamine; }
		set {
			m_HasExamine = value;
			if(!m_HasExamine){
				m_ExamineText.gameObject.SetActive(false);
			} else {
				m_ExamineText.gameObject.SetActive(true);
			}
		}
	}

	public bool HasPickup {
		get { return m_HasPickup; }
		set {
			m_HasPickup = value;
			if(!m_HasPickup){
				m_PickupText.gameObject.SetActive(false);
			} else {
				m_PickupText.gameObject.SetActive(true);
			}
		}
	}

	public bool CanBeUsed {
		get { return m_CanBeUsed; }
		set {
			m_CanBeUsed = value;
			if(!m_CanBeUsed){
				m_UseText.gameObject.SetActive(false);
			} else {
				m_UseText.gameObject.SetActive(true);
			}
		}
	}

	public GameObject Target {
		get { return m_Target; }
		set {
			m_Target = value;
			m_WorldCamera =  NGUITools.FindCameraForLayer(m_Target.layer);
		}
	}

	void Awake(){
		r_Transform = transform;
		r_Tweener = GetComponent<UIPlayTween>();
	}
	
	void Start() {
		m_GUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
	}

	public void active(bool value) {
		r_Tweener.Play(value);
	}

	public void reposition() {
		if(r_Grid == null) {
			return;
		}
		r_Grid.Reposition ();
	}

	public void LateUpdate() {
		if(!enabled || m_Target == null){
			return;
		}
		
		m_WorldCamera =  NGUITools.FindCameraForLayer(m_Target.layer);
		m_Position = m_WorldCamera.WorldToViewportPoint(m_Target.transform.position);
		
		m_Position = m_GUICamera.ViewportToWorldPoint(m_Position);
		m_Position.z = 0.0f;
		
		r_Transform.position = m_Position + m_Offset;
	}
}
