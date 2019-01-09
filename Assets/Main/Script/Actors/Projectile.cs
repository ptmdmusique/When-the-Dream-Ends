using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GeneralObject {

    //Info
    [Header("Projectile")]
    public Rigidbody2D myRigidbody = null;
    public float mySpeed = 3;
    public GameObject myTarget = null;
    public float myLifeTime = 0;
    public Vector3 direction = Vector3.right;   //Default direction
    public bool toTarget = false; //Fire to target? or just 1 direction

    new protected virtual void Awake() {
        base.Awake();
        myRigidbody = GetComponent<Rigidbody2D>();
        if (myTarget == null) {
            if (myTag.Contains("Enemy") == true) { 
                myTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().gameObject;
            }
        }
    }
}
