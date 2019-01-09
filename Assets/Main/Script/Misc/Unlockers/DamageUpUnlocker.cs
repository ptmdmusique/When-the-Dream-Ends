using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUpUnlocker : Collectibles {
    [Header("DamageUpModifier")]
    public float modifierValue = 0.1f;
    public int option = 1;
    public Transform myText;

    new private void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);

        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            //Modify value then automatically animate
            myPlayerScript.ModifyDamageModifier(modifierValue, option);
            myAnimator.SetTrigger("IsUnlocked");
            Destroy(GetComponent<Collider>());
            
            //Pop up Text
            Text popup = Instantiate(myText, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<Text>();
            popup.text = "Damage Up!";
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
                myPlayerScript.ModifyDamageModifier(modifierValue, option);
                myAnimator.SetTrigger("IsUnlocked");
                Destroy(GetComponent<Collider>());

                //Pop up Text
                Text popup = Instantiate(myText, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<Text>();
                popup.text = "Damage Up!";
                popup.color = Color.white;

                if (GetComponent<DestroyAfterAnimation>() == null) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
