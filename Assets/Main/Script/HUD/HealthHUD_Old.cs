using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD_Old : MonoBehaviour {

    [Header("Player")]
    private Player playerManager;

    [Header("ChildHeart")]
    public List<GameObject> childHeart;
    public GameObject myHeart;

    [Header("Canvas Stuff")]
    private Canvas canvas;

    private void Awake() {  
        playerManager = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update() {
        UpdateHeartList();
    }

    void RemoveHeart() {
        if (childHeart.Count <= 1) {
            return;
        }
        GameObject removeChild = childHeart[childHeart.Count - 1];
        childHeart.Remove(removeChild);

        Destroy(removeChild.gameObject);
    }

    void AddHeart() {
        //Add the new heart 3 unit away from the last heart

        if (childHeart.Count > 0) {
            GameObject lastHeart = childHeart[childHeart.Count - 1];
            Vector3 newPosition = new Vector3(lastHeart.transform.localPosition.x + 30, lastHeart.transform.localPosition.y, 0);
            GameObject curHeart = Instantiate(myHeart);
            curHeart.transform.SetParent(transform);
            curHeart.transform.localPosition = newPosition;
            curHeart.transform.localScale = lastHeart.transform.localScale;

            childHeart.Add(curHeart);
        }        
    }

    void UpdateHeart(int heartIndx, int indx) {
        if (heartIndx >= childHeart.Count) {
            //Safe guard
            return;
        }

        //2 = full health - 1 = half health - 0 = zero health
        if (heartIndx > childHeart.Count - 1) {
            return;
        }
        GameObject curHeart = childHeart[heartIndx];
        curHeart.GetComponent<Image>().sprite = curHeart.GetComponent<SpriteObject>().spriteList[indx];
    }

    void UpdateHeartList() {
        if (playerManager.GetCurHealth() < 0) {
            return;
        }

        int expectedHeart = (int)playerManager.GetMaxHealth() / 100;
        if (((int)playerManager.GetMaxHealth() + 50) % 100 == 0) {
            expectedHeart++;
        }

        //Check if we have just enough heart
        //1 heart worths 100 health => 4 hearts worth 400 health and so on
        int numberToAdd = expectedHeart - childHeart.Count;
        if(numberToAdd > 0) {
            for (int indx = 0; indx < numberToAdd; indx++) {
                AddHeart();
            }
        }
        else if (numberToAdd < 0) {
            for (int indx = 0; indx < -numberToAdd; indx++) {
                RemoveHeart();
                if (childHeart.Count <= 1) {    //Break if there is only 1 heart left
                    break;
                }
            }
        }

        //Update the heart sprite
        int curMaxHeart = (int)playerManager.GetCurHealth() / 100;
        int curHealthStatus = (int)playerManager.GetCurHealth() % 100;

        if (curMaxHeart == 0 && curHealthStatus == 0) {
            //If our current heart is 0, displayed all hearts as 0
            for (int indx = 0; indx < expectedHeart; indx++) {
                UpdateHeart(indx, 0);
            }
            return;
        }

        if (curHealthStatus == 50) {
            //All the previous hearts are displayed as full
            for (int indx = 0; indx < curMaxHeart; indx++) {
                UpdateHeart(indx, 2);
            }
            //Update the current heart as half
            UpdateHeart(curMaxHeart, 1);

            //All the rest are displayed as zero
            for (int indx = curMaxHeart + 1; indx < expectedHeart; indx++) {
                UpdateHeart(indx, 0);
            }
            return;
        }
        else {
            //All the previous heart and current heart are displayed as full
            for (int indx = 0; indx < curMaxHeart; indx ++) {
                UpdateHeart(indx, 2);
            }
            //Make the rest display as 0
            for (int indx = curMaxHeart; indx < expectedHeart; indx++) {
                UpdateHeart(indx, 0);
            }
            return;
        }
    }

}
