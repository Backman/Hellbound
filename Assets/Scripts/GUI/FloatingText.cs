using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class FloatingText : MonoBehaviour {

	private UILabel r_Label;
	private bool m_Active;
	private Vector3 m_Position;

	public GameObject m_Target;
	public Vector3 m_Offset;
	private Camera m_WorldCamera;
	private Camera m_GUICamera;

	private TweenPosition r_Tweener;
	private Transform r_Transform;

	public Color TextColor {
		get { return r_Label.color; }
		set { r_Label.color = value; }
	}

	public string Text {
		get { return r_Label.text; }
		set { r_Label.text = value; }
	}

	public bool Active {
		get { return m_Active; }
		set { m_Active = value; }
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
		r_Tweener = GetComponent<TweenPosition>();
		r_Label = GetComponent<UILabel>();
	}

	void Start() {
		m_GUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
	}

	public void Initiate(string text, Color color, GameObject target) {
		Text = text;
		TextColor = color;
		Target = target;
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

	public void notExaminable() {

	}

	public void notPickable() {

	}
}
