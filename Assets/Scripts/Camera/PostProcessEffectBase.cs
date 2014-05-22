using UnityEngine;

/// <summary>
/// Base class for all image effects
/// 
/// By Arvid Backman
/// </summary>

[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class PostProcessEffectBase : MonoBehaviour {
	public Shader m_Shader;
	private Material m_Material;

	protected virtual void Start() {
		if(!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}

		if(!m_Shader || !m_Shader.isSupported) {
			enabled = false;
		}
	}

	protected Material material {
		get {
			if(m_Material == null) {
				m_Material = new Material(m_Shader);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
			}

			return m_Material;
		}
	}

	protected virtual void OnDisable() {
		if(m_Material) {
			DestroyImmediate(m_Material);
		}
	}

}

