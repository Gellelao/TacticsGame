using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartResourceManager : MonoBehaviour {
	void Awake () {
		ResourceManager.Initialize();
	}
}
