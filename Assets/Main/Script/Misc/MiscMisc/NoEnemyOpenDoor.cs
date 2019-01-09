using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEnemyOpenDoor : MonoBehaviour {

    public List<Transform> doorList;
    public bool readyToCheck = false;

    List<Transform> enemyList = new List<Transform>();

    private void Start() {
        //Add enemies into the list
        foreach (Transform child in transform){ 
            if (child.tag == "Enemy") {
                enemyList.Add(child);
            }
        }    
        if (enemyList.Count > 0) {
            readyToCheck = true;
        }

        StartCoroutine(RemoveMissingEntry());
    }

    private void Update() {

        if (readyToCheck == true && enemyList.Count <= 0) {
            //If there is no enemy left, then open all the door and turn off this function
            foreach(Transform door in doorList) {
                door.GetComponent<Door>().UnlockDoor();
            }
            readyToCheck = false;
        }
    }

    IEnumerator RemoveMissingEntry() {
        enemyList.RemoveAll(item => item == null);

        yield return new WaitForEndOfFrame();

        StartCoroutine(RemoveMissingEntry());
    }
}
