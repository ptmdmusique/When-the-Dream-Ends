using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJumpUnlocker : Collectibles {
    [Header("Double Jump Unlocker")]
    public Transform myText;
    public Transform myTextDescription;

    new private void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);

        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {

            //Pop up Text
            Transform notification = Instantiate(myText, transform.position, Quaternion.identity).transform;
            Text popup = notification.Find("Text").GetComponent<Text>();
            popup.text = "Double Jump Unlocked!";
            popup.color = Color.white;

            popup = notification.Find("Description").GetComponent<Text>();
            popup.text = "Press Jump after the first jump from the ground to execute";

            myPlayerScript.doubleJump.unlocked = true;
            myAnimator.SetTrigger("IsUnlocked");
            if (GetComponent<DestroyAfterAnimation>() == null) {
                Destroy(gameObject);
            }
        }
    }

    private void Update() {
        if (ready == true) {
            Player myPlayerScript = GameObject.Find("Player").GetComponent<Player>();
            if (myPlayerScript != null) {

                //Pop up Text
                Text popup = Instantiate(myText, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<Text>();
                popup.text = "Double Jump Unlocked!";
                popup.color = Color.white;

                myPlayerScript.doubleJump.unlocked = true;
                myAnimator.SetTrigger("IsUnlocked");
                if (GetComponent<DestroyAfterAnimation>() == null) { 
                    Destroy(gameObject);
                }
            }
        }
    }
}
