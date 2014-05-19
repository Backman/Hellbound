using UnityEngine;
using System.Collections;
/// <summary>
/// Blur effect.
/// 
/// An image effect on the camera. Inspiered by Arvids PauseGameEffect.
/// 
/// Created by Simon
/// </summary>
[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour {
	public Shader m_Shader = null;
	[Range(0.0f, 1.0f)]
	public float  m_BlurAmount = 0.3f;

	private float m_AppliedBlurAmount = 0.0f;	//This is the value acctually applied to the blur shader.

	static Material m_Material = null;
	protected Material material {
		get {
			if (m_Material == null) {
				m_Material = new Material(m_Shader);
				m_Material.hideFlags = HideFlags.DontSave;
			}
			return m_Material;
		} 
	}

	private void Start() {
		m_AppliedBlurAmount = m_BlurAmount;
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		// Disable if the shader can't run on the users graphics card
		if (!m_Shader || !material.shader.isSupported) {
			enabled = false;
			return;
		}
	}

	void Update(){
#if UNITY_EDITOR
		m_AppliedBlurAmount = m_BlurAmount;
#endif
		material.SetFloat ("_BlurSize", m_AppliedBlurAmount * 0.01f);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination) {
		RenderTexture hBlur = RenderTexture.GetTemporary(source.width, source.height);
		
		//Graphics.Blit(source, vBlur, material);
		Graphics.Blit(source, hBlur, material, 0);
		Graphics.Blit(hBlur, destination, material, 1);
		
		RenderTexture.ReleaseTemporary(hBlur);
	}


	public void setBlurPercentage(float percentage){
		float val = Mathf.Clamp (percentage, 0.0f, 1.0f);
		m_AppliedBlurAmount = m_BlurAmount * val;
	}

	public void setNewBlurAmount( float amount ){
		m_BlurAmount = amount;
	}
}
