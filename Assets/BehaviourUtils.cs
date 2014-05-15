
using UnityEngine;
using System.Collections;

public class BehaviorUtils {
	public static void MakeVisible(GameObject obj, bool visible) {
		if (obj.renderer != null && obj.renderer.enabled != visible) {
			obj.renderer.enabled = visible;
		}
		int childCount = obj.transform.childCount;
		int i;
		for (i = 0; i < childCount; i++) {
			GameObject child = obj.transform.GetChild(i).gameObject;
			MakeVisible(child, visible);
		}
	}
}