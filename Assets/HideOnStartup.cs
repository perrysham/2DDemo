
using UnityEngine;
using System.Collections;

public class HideOnStartup : MonoBehaviour {
	void Start() {
		BehaviorUtils.MakeVisible(gameObject, false);
	}
}
