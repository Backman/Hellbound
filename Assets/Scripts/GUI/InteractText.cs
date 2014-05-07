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
	private bool m_HasPickup = false;
	private bool m_CanBeUsed = true;

	public bool HasExamine {
		get { return m_HasExamine; }
	}
	public bool HasPickup {
		get { return m_HasPickup; }
	}
	public bool CanBeUsed {
		get { return m_CanBeUsed; }
	}
	public GameObject Target {
		get { return m_Target; }
		set {
			Debug.Log( "Used" );
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

	public void setupInteractionTexts( string examineText, string useText ){
		reposition();

		m_HasExamine = ( examineText != "" );
		m_CanBeUsed  = ( useText  	 != "" );

		m_ExamineText.gameObject.SetActive( m_HasExamine );
		m_UseText.gameObject.SetActive( m_CanBeUsed );
		m_UseText.text =  "Press E to " + useText;

		reposition();
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
