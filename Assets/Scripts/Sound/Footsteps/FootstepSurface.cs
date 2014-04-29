using UnityEngine;
using System.Collections;

public class FootstepSurface : MonoBehaviour {
	/// <summary>
	/// FootstepSurface script will need to be placed on any surface the player can walk on
	/// to change the sound the "footsteps" makes.
	/// Anton Thorsell
	/// </summary>

	public bool m_UseFootstepSurface = false;
	public float m_Surface = 0f;
	public int m_Priority = 0;
}