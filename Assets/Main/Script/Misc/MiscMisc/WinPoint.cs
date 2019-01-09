using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPoint : MonoBehaviour {

    [Header("WinPoint")]
    public Transform winText;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            //Destroy(GetComponent<Collider>());

            //Pop up Text
            Text popup = Instantiate(winText, transform.position, Quaternion.identity).transform.Find("GratzText").GetComponent<Text>();
            popup.text = "Well Done";
            //popup.color = Color.white;

            //Destroy if there is no animation
            if (GetComponent<DestroyAfterAnimation>() == null) {
                Destroy(gameObject);
            }
        }
    }
}
