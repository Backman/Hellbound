using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PauseGameEffect : MonoBehaviour {	
	public float m_BlurSize = 1.0f;	
	public Shader m_Shader = null;
	
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
	
	protected void OnDisable() {
		if( m_Material ) {
			DestroyImmediate( m_Material );
		}
	}

	void Start() {

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

	void Update() {
		material.SetFloat("_BlurSize", m_BlurSize * 0.01f);
	}

	IEnumerator pauseGame(bool pause) {
		if(pause) {
			enabled = true;
			m_BlurSize = 0.35f;
		} else {
			float t = 0.5f;
			while(t > 0.0f) {
				m_BlurSize = t * 0.35f;

				t -= Time.deltaTime;
				yield return null;
			}
			m_BlurSize = 0.0f;
			enabled = false;
		}
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		RenderTexture hBlur = RenderTexture.GetTemporary(source.width, source.height);

		//Graphics.Blit(source, vBlur, material);
		Graphics.Blit(source, hBlur, material, 0);
		Graphics.Blit(hBlur, destination, material, 1);

		RenderTexture.ReleaseTemporary(hBlur);
	}

	/*public BlurEffect blurEffect {
		get; set;
	}

	void Awake() {
		blurEffect = GetComponent<BlurEffect>();
	}

	void OnEnable() {
		if(blurEffect != null) {
			blurEffect.enabled = true;
		}
	}

	void OnDisable() {
		if(blurEffect != null) {
			blurEffect.enabled = false;
		}
	}

	IEnumerator pauseGame(bool pause) {
		if(pause) {
			enabled = true;
			blurEffect.blurSpread = 0.25f;
		} else {
			float t = 0.5f;
			while(t > 0.0f) {
				blurEffect.blurSpread = t * 0.25f;
				
				t -= Time.deltaTime;
				yield return null;
			}
			blurEffect.blurSpread = 0.0f;
			enabled = false;
		}
	}*/
}
