using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEndHide : MonoBehaviour {

    MeshRenderer myMR;

	// Use this for initialization
	void Start () {
        myMR = GetComponent<MeshRenderer>();
        myMR.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
