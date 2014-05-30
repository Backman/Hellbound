using UnityEngine;
using System.Collections;

public class FootPlacement : MonoBehaviour {
	private Animator m_Animator;
	void Start(){
		m_Animator = GetComponent<Animator>();
	}
	
	void OnAnimatorIK(int layerIndex){
		Animator animator = gameObject.GetComponent<Animator>();
		//animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftFootTarget.getPosition());
		Vector3 leftFootPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
		//animator.SetIKPosition(AvatarIKGoal.LeftFoot, new Vector3(0.0f, 10.0f, 0.0f));
	}
}
