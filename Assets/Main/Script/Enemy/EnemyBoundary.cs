using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoundary : MonoBehaviour {

    public bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            playerInRange = false;
        }
    }
}
