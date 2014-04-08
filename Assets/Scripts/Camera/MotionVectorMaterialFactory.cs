using UnityEngine;
using System.Collections;

public class MotionVectorMaterialFactory {
	static Shader m_MotionVectorShader;

	public static Shader MotionVectorShader {
		get {
			if(!m_MotionVectorShader) {
				m_MotionVectorShader = Shader.Find("Hidden/Motion Vectors");
			}
			return m_MotionVectorShader;
		}
	}

	public static Material NewMaterial() {
		return new Material(MotionVectorShader);
	}
}

