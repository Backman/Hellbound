using UnityEngine;
using System.Collections;

public abstract class PreviewItem : MonoBehaviour {
	protected virtual void Awake(){}
	public abstract void show();
	public abstract void hide();
}
