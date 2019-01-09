using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour {

    private Player myPlayerScript;

    [Header("InfestedCoin")]
    public Transform infestedPointParent;
    public Animator infestedPointAnimator;
    public Text infestedPoint;

    private void Start() {
        myPlayerScript = GameObject.Find("Player").GetComponent<Player>();

        //Coin
        infestedPointParent = transform.Find("InfestedCoinParent");
        infestedPoint = infestedPointParent.Find("InfestedCoinText").GetComponent<Text>();
        infestedPointAnimator = infestedPointParent.Find("InfestedCoinImage").GetComponent<Animator>();
    }

    private void Update() {
        
    }

    public void UpdateCoin() {
        infestedPoint.text = myPlayerScript.infestedCoin.ToString();
        infestedPointAnimator.SetInteger("NumberOfCoin", myPlayerScript.infestedCoin);
        infestedPointAnimator.SetTrigger("IsCollected");
    }
}
