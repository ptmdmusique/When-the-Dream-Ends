using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_Unlocker : MonoBehaviour {

    public bool canDestroy = false;

    private float originalTimeScale;
   
	// Use this for initialization
	void Start () {
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) == true && canDestroy == true) {
            Time.timeScale = originalTimeScale;
            Destroy(gameObject);
        }
	}
}
