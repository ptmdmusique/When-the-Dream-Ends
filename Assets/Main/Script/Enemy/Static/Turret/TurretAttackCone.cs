using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttackCone : MonoBehaviour {

    public GameObject turret;
    public AlarmStatus alarmStatus;
    public bool isFirstTime = true;
    

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") == true) {
            if (turret.GetComponent<TurretZero>().isFullyAwake == true) {
                turret.GetComponent<TurretZero>().canFire = true;
                if (isFirstTime == true) {
                    alarmStatus.GetComponent<AlarmStatus>().InRange();
                    isFirstTime = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player") == true) {
            turret.GetComponent<TurretZero>().canFire = false;
            isFirstTime = true;
        }
    }

}
