using Pathfinding;
using UnityEngine;
using System.Collections;

public class AutoCollectCollectibles : Collectibles {

    public bool startChasePlayer = false;
    public AStarMoveToObject moveToScript;

    new protected void Start() {
        base.Start();
        moveToScript = GetComponent<AStarMoveToObject>();
        canBeCollected = true;
    }

    protected void Update() {
        if (startChasePlayer == true && canBeCollected == true) {
            if (moveToScript == null) {
                gameObject.AddComponent<AStarMoveToObject>();
                moveToScript = GetComponent<AStarMoveToObject>();
                moveToScript.target = GameObject.Find("Player").transform.Find("Target").transform;
                moveToScript.staticTarget = false;
                moveToScript.standBy = false;
                moveToScript.speed = 50000;
            }
        }
    }

    private void OnAnimatorMove() {
        if (myRB != null) {
            myRB.velocity = myAnimator.deltaPosition / Time.deltaTime;
        }
    }
}
