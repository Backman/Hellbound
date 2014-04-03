using UnityEngine;
using System.Collections;

public class ThirdPersonCharacter : MonoBehaviour {
	[Range(0.1f, 3.0f)] public float m_MoveSpeedMultiplier;
	[Range(0.1f, 3.0f)] public float m_AnimationSpeedMultiplier;

	Vector3 m_LookDirection;
	Animator m_Animator;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
