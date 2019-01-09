using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestedCoin : Collectibles {

    new private void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        Player playerScript = collision.GetComponent<Player>();
        if (playerScript != null) {

            //Increase Player coin's amount
            playerScript.infestedCoin += (int) myValue;

            //Update HUD
            GameObject.Find("GameMaster").GetComponent<GameMaster>().UpdateCoin();

            myAnimator.SetTrigger("IsCollected");


        }
    }
}
