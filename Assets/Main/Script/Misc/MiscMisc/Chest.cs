using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : GeneralObject {

    [Header("Chest")]
    public GameObject myContent;
    public Text myText;
    public bool requireKey = false;

    private Coroutine myRoutine;

    private void Start() {
        myAnimator = GetComponent<Animator>();
        myText.enabled = false;    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            myText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            myText.enabled = false;
        }
    }

    private void Update() {
        if (myText.enabled == true && requireKey == false) {
            if (Input.GetKeyDown(KeyCode.Return) == true) {
                if (myRoutine == null) {
                    Debug.Log("Có èn");
                    myText.text = "Unlocked!";
                    myAnimator.SetTrigger("IsCollected");
                    //myRoutine = StartCoroutine(UIChange());
                }
                //Spawn object
                GameObject spawn = Instantiate(myContent, transform.position, Quaternion.identity);
                spawn.GetComponent<Collectibles>().ready = true;    
            }
        }
    }

}
