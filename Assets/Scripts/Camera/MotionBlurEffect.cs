using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera), typeof(CameraInfo))]
public class MotionBlurEffect : PostProcessEffectBase {
	
	protected static HashSet<ObjectBlur> BlurObjects {
		get {
			if(m_BlurObjects == null) {
				m_BlurObjects = new HashSet<ObjectBlur>();
			}
			return m_BlurObjects;
		}
	}
	protected static HashSet<ObjectBlur> m_BlurObjects;


	protected Camera m_VelocityCamera;

	public static void RegisterBlurObject(ObjectBlur obj) {
		BlurObjects.Add(obj);
	}

	public static void RemoveBlurObject(ObjectBlur obj) {
		BlurObjects.Remove(obj);
	}
 	
	virtual protected void Awake() {
		GameObject velocityCamera = new GameObject("Velocity Camera (Auto-Generated)", typeof(Camera));
		velocityCamera.transform.parent = transform;
		m_VelocityCamera = velocityCamera.camera;
		velocityCamera.SetActive (false);
	}

	virtual protected void OnRenderImage(RenderTexture source, RenderTexture dest) {
		foreach(ObjectBlur obj in BlurObjects) {
			obj.PreMotionRender();
		}

		RenderTexture velocityTexture = RenderTexture.GetTemporary(source.width, source.height, 24);

		m_VelocityCamera.CopyFrom (camera);
		m_VelocityCamera.backgroundColor = new Color(0.4980392f, 0.5f, 0.4980392f, 0.5f);
		m_VelocityCamera.targetTexture = velocityTexture;
		m_VelocityCamera.RenderWithShader(MotionVectorMaterialFactory.MotionVectorShader, "RenderType");
		m_VelocityCamera.targetTexture = null;

		foreach(ObjectBlur obj in BlurObjects) {
			obj.PostMotionRender();
		}

		material.SetTexture ("_VelocityTex", velocityTexture);

		Graphics.Blit(source, dest, material);

		RenderTexture.ReleaseTemporary(velocityTexture);
	}
}

