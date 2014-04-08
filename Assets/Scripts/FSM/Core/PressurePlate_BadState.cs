using UnityEngine;
using System.Collections;

public class PressurePlate_BadState : FSMState<PressurePlate> {

	public PressurePlate_BadState(Executions.Type type) {
		setUpExecutionType(type);
	}

	public override void enter (PressurePlate entity)
	{
		entity.renderer.material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	}

	public override void execute (PressurePlate entity)
	{
		Debug.Log("Is executing!!");
	}

	public override void exit (PressurePlate entity)
	{
		entity.renderer.material.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
	}
}

