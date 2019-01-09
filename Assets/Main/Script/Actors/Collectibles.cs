using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : GeneralObject {

    public bool canBeCollected = true;
    public bool ready = false;
    public Color splashColor = Color.white;
    public Transform mySplash;

    protected Rigidbody2D myRB;


    public void Start() {
        myAnimator = GetComponent<Animator>();
        myRB = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        Player playerScript = collision.GetComponent<Player>();
        if (playerScript != null) {

            Vector3 spawnPoint = GetComponent<Collider2D>().bounds.center;
            Transform theSplash = Instantiate(mySplash, spawnPoint, Quaternion.identity);
            ParticleSystem.MainModule theMain = theSplash.GetComponent<ParticleSystem>().main;
            theMain.startColor = splashColor;
        }
    }
}
