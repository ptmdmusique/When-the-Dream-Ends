using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUp : Collectibles {

    [Header("HealthUp")]
    public float modifierValue = 50;
    public Transform myText;
    public int option = 1;

    new private void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            //Modify value then automatically animate
            myPlayerScript.MaxHealthModifier(modifierValue, option);
            myAnimator.SetTrigger("IsUnlocked");
            Destroy(GetComponent<Collider>());

            //Pop up Text
            Text popup = Instantiate(myText, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<Text>();
            popup.text = "Max Health Up!";
            popup.color = Color.white;

            //Destroy if there is no animation
            if (GetComponent<DestroyAfterAnimation>() == null) {
                Destroy(gameObject);
            }
        }
    }

    private void Update() {
        if (ready == true) {
            Player myPlayerScript = GameObject.Find("Player").GetComponent<Player>();
            if (myPlayerScript != null) {
                myPlayerScript.MaxHealthModifier(modifierValue, option);
                myAnimator.SetTrigger("IsUnlocked");
                Destroy(GetComponent<Collider>());

                //Pop up Text
                Text popup = Instantiate(myText, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<Text>();
                popup.text = "Max Health Up!";
                popup.color = Color.white;

                if (GetComponent<DestroyAfterAnimation>() == null) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
