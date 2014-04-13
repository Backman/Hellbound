using UnityEngine;
using UnityEditor;


public class AddObjectBlurComponent {
	[MenuItem("Custom/Add Blur To Object")]
	static void AddBlur() {
		foreach(Transform t in Selection.transforms) {
			AddBlurComponent(t);
		}
	}

	static void AddBlurComponent(Transform t) {
		if((t.GetComponent<MeshRenderer>() || t.GetComponent<SkinnedMeshRenderer>()) && !t.GetComponent<ObjectBlur>()) {
			t.gameObject.AddComponent<ObjectBlur>();
		}
		foreach(Transform child in t) {
			AddBlurComponent(child);
		}
	}

	[MenuItem("Custom/Add Blur To Object", true)]
	static bool ValidateAddBlur() {
		return Selection.activeTransform;
	}
}

