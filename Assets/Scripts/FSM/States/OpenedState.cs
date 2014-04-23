using UnityEngine;
using System.Collections;

[System.Serializable]
public class OpenedState : State<LockedInteractable> {

	public override void enter (LockedInteractable entity)
	{
		GameObject.Destroy(entity.gameObject);
	}

	public override void activate (LockedInteractable entity)
	{
		Debug.Log(this.ToString() +" is opened");
	}
}
