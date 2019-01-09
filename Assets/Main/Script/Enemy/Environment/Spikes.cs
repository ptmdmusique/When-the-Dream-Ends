using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : StaticEnemy {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") == true) {

            myPlayer.TakeDamage(myBaseDamage);
            
            myPlayer.StartKnockback(1.5f, transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") == true) {

            myPlayer.TakeDamage(myBaseDamage);

            myPlayer.StartKnockback(1.5f, transform);
        }
    }
}
