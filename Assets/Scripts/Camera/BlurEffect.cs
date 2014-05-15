using UnityEngine;
using System.Collections;

public class BlurEffect : MonoBehaviour {

	[TooltipAttribute("The amount of blur imposed on the screen when this script is active")]
	public float m_BlurAmount = 0.5f;
	public Shader m_BlurShader = null;


}
