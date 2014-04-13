using UnityEngine;
using System.Collections;

[AddComponentMenu("Material/Draw Vector Field")]
public class ObjectBlur : MonoBehaviour {

	private Material m_RegularMaterial;
	private Material m_StretchMaterial;

	private Matrix4x4 m_PreviousModelMatrix;

	protected void Start() {
		m_StretchMaterial = MotionVectorMaterialFactory.NewMaterial();
		m_RegularMaterial = renderer.material;

		m_PreviousModelMatrix = transform.localToWorldMatrix;
	}

	protected void OnEnable() {
		MotionBlurEffect.RegisterBlurObject(this);
	}

	protected void OnDisable() {
		MotionBlurEffect.RemoveBlurObject(this);
	}

	// Set up for motion rendering
	public void PreMotionRender() {
		m_RegularMaterial = renderer.material;
		renderer.material = m_StretchMaterial;
	}

	// Restore to normal rendering
	public void PostMotionRender() {
		renderer.material = m_RegularMaterial;
	}

	protected void LateUpdate() {
		Vector4 currentPos = transform.position;
		currentPos.w = 1.0f;

		Matrix4x4 modelViewMatrix = CameraInfo.ViewMatrix * transform.localToWorldMatrix;
		Matrix4x4 previousModelViewMatrix = CameraInfo.PrevViewMatrix*m_PreviousModelMatrix;

		m_StretchMaterial.SetMatrix("_ModelView", modelViewMatrix);
		m_StretchMaterial.SetMatrix("_prevModelView", previousModelViewMatrix);
		m_StretchMaterial.SetMatrix("_ModelViewInverseTranspose", modelViewMatrix.transpose.inverse);
		m_StretchMaterial.SetMatrix("_prevModelViewProjection", CameraInfo.PrevViewProjectionMatrix*m_PreviousModelMatrix);

		m_PreviousModelMatrix = transform.localToWorldMatrix;
	}
}

