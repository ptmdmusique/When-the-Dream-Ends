using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public bool willRespawn = false;
    public bool hasBackup = false;

    private List<Transform> enemyList = new List<Transform>();
    private List<Transform> backupList = new List<Transform>();
    
    Transform backupObject;

    private void Start() {
        backupObject = transform.Find("BackupList");

        if (hasBackup == true) {
            foreach (Transform child in transform) {
                if (child.tag == "Enemy") {
                    enemyList.Add(child);
                    backupList.Add(Instantiate(child, backupObject));         //Make a backup of all the enemies on the child list...
                    backupList[backupList.Count - 1].gameObject.SetActive(false);
                }
            }
        }

    }

    public void ResetEnemy() {

        if (hasBackup == false || willRespawn == false) {
            Debug.Log("Can't respawn!");
            return;
        }

        for (int indx = 0; indx < enemyList.Count; indx++) {
            if (enemyList[indx] == null) {                  //If that enemy is destroyed, instantiate that enemy from the backup
                enemyList[indx] = Instantiate(backupList[indx], transform);
                enemyList[indx].gameObject.SetActive(true);
            } else {                                        //If not, reset the enemy health
                foreach (Transform child in enemyList[indx]) {
                    GeneralObject enemyScript = child.GetComponent<GeneralObject>();
                    if (enemyScript != null) {
                        enemyScript.ResetObject();
                        break;
                    }
                }         
            }
            
        }
    }
}
