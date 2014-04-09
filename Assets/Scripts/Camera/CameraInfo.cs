using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraInfo : MonoBehaviour {

	public static Matrix4x4 ViewMatrix { get; private set; }
	public static Matrix4x4 ProjectionMatrix { get; private set; }
	public static Matrix4x4 ViewProjectionMatrix { get; private set; }
	public static Matrix4x4 PrevViewMatrix { get; private set; }
	public static Matrix4x4 PrevProjectionMatrix { get; private set; }
	public static Matrix4x4 PrevViewProjectionMatrix { get; private set; }

	bool m_D3D;
	// Use this for initialization
	protected void Awake () {
		m_D3D = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;
	
		initializeMatrices();
		updateMatrices();
	}

	void initializeMatrices() {
		ViewMatrix = Matrix4x4.identity;
		ProjectionMatrix = Matrix4x4.identity;
		ViewProjectionMatrix = Matrix4x4.identity;
		PrevViewMatrix = Matrix4x4.identity;
		PrevProjectionMatrix = Matrix4x4.identity;
		PrevViewProjectionMatrix = Matrix4x4.identity;
	}

	protected void Update() {
		PrevViewMatrix = ViewMatrix;
		PrevProjectionMatrix = ProjectionMatrix;
		PrevViewProjectionMatrix = ViewProjectionMatrix;

		updateMatrices();
	}

	void updateMatrices() {
		ViewMatrix = camera.worldToCameraMatrix;
		Matrix4x4 proj = camera.projectionMatrix;

		if(m_D3D) {
			for(int i = 0; i < 4; ++i) {
				proj[1, i] = -proj[1, i];
			}

			for(int i = 0; i < 4; ++i) {
				proj[2, i] = proj[2, i] * 0.5f + proj[3, i] * 0.5f;
			}
		}

		ProjectionMatrix = proj;
		ViewProjectionMatrix = ProjectionMatrix * ViewMatrix;
	}
}
