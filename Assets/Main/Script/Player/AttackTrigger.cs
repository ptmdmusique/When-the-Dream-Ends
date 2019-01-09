using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTrigger : GeneralObject {

    [Header("Personal")]
    public Transform myDamageText;


    private void OnTriggerEnter2D(Collider2D collision) {
        GeneralObject generalScript = collision.GetComponent<GeneralObject>();
        if (generalScript != null) {
            if (generalScript.myTag.Contains("Enemy") == true) {
                if (generalScript.myTag.Contains("CanBeDamaged") == true) {
                    generalScript.TakeDamage(myBaseDamage);

                    //Knock the other objects back a little bit
                    if (collision.GetComponent<MovingObject>() != null) {
                        collision.GetComponent<MovingObject>().Knockback(0.005f, transform);
                    }

                    //Create damage pop up
                    Vector3 spawnLoc = collision.transform.position;
                    spawnLoc.y += 2;
                    Text popup = Instantiate(myDamageText, spawnLoc, Quaternion.identity).transform.GetChild(0).GetComponent<Text>();
                    popup.text = myBaseDamage.ToString();

                    collision.GetComponent<GeneralObject>().isEngaged = true;

                }

                //Camera shake
                Player myPlayer = transform.parent.GetComponent<Player>();
                if (myPlayer != null) {
                    CameraShake myCameraShake = myPlayer.myMainCamera.transform.GetComponent<CameraShake>();
                    myCameraShake.shakeAmount = 0.5f;
                    myCameraShake.shakeTimer = 0.2f;
                }
            }
        }
    }
}
