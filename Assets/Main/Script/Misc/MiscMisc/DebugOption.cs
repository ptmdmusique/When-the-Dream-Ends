using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOption : MonoBehaviour {

    public float customTimeScale = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Time.timeScale = customTimeScale;
	}
}
