using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellHound : MovingEnemy {

    void Movement() {

        if (myState != (int)AnimState.Attacking && myState != (int)AnimState.Hurt && canMove == true) {
            int MoveOrIdle = Random.Range(0, 10);    //Stay idle or move

            if (MoveOrIdle <= 1) {                  //Idle
                myState = (int)AnimState.Idle;
                rigidBody.velocity = Vector3.zero;
            }
            else {
                myState = (int)AnimState.Move;
                float force = speed * direction;
                //rigidBody.AddForce(new Vector2(force, 0));
            }

        }
    }

    

    new void FixedUpdate() {
        base.FixedUpdate();
        Movement();
        myAnimator.SetInteger("State", myState);
    }
}
