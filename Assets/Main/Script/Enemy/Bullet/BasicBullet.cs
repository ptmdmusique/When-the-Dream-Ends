using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : Projectile {

	// Use this for initialization
	void Start () {
        if (myLifeTime != -1) { 
            Destroy(this.gameObject, myLifeTime);
        }

        if (myTarget != null) {
            if (toTarget == true) {
                //Rotate to that direction
                Quaternion rotation = Quaternion.LookRotation(myTarget.transform.position - transform.position, Vector3.up);
                rotation.y = transform.rotation.y;
                transform.rotation = rotation;

                direction = (myTarget.transform.position - transform.position).normalized;  //Re-calculate direction
            }

            //myRigidbody.AddForce(direction * mySpeed);  //Add force to that direction
            myRigidbody.velocity = direction * mySpeed;
        }
    }
	
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") == true) {

            myTarget.GetComponent<Player>().TakeDamage(myBaseDamage);
            myTarget.GetComponent<Player>().StartKnockback(1.5f, transform);
            Destroy(this.gameObject);

        } else if (collision.GetComponent<GeneralObject>() != null && collision.GetComponent<GeneralObject>().myTag.Contains("Obstacle") == true && collision.transform.tag != transform.tag){
            Destroy(this.gameObject);
        }
    }

    private void Update() {
        Debug.Log(myRigidbody.velocity);
    }
}
