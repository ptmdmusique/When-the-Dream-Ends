using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification_UI : MonoBehaviour {

    public bool canMoveOn;

    private float originalTimeScale;

    // Use this for initialization
    void Start() { 
        originalTimeScale = Time.timeScale;

        //Find a camera
        if (GetComponent<Canvas>().worldCamera == null) { 
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        //Save the original timescale 
        if (originalTimeScale == 0) {
            originalTimeScale = 1;
        }

        Time.timeScale = 0;
        
    }

    // Update is called once per frame
    void Update() {
        
        if (Input.GetKeyDown(KeyCode.Return) == true && canMoveOn == true) {
            GetComponent<Animator>().SetTrigger("MoveOn");
        }
    }

    public void DestroyObject() {
        Time.timeScale = originalTimeScale;
        Destroy(gameObject);
    }
}
