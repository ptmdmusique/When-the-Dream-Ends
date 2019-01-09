using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour {

    public GameObject myTarget;
    public float myDamage;

    public void Start() {
        myTarget = GameObject.Find("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") == true) {

            myTarget.GetComponent<Player>().TakeDamage(myDamage);
            myTarget.GetComponent<Player>().StartKnockback(0.5f, transform);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player") == true) {

            myTarget.GetComponent<Player>().TakeDamage(myDamage);
            myTarget.GetComponent<Player>().StartKnockback(0.5f, transform);

        }
    }
}
