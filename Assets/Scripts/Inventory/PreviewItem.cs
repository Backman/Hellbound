using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract class for handling preview of object in inventory
/// </summary>
public abstract class PreviewItem : MonoBehaviour {
	protected virtual void Awake(){}
	public abstract void show();
	public abstract void hide();
}
