using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MovingEnemy {

    private void Start() {
        attackTimer = 2;
        attackCounter = 0;
        direction = -1;         //Default direction
    }

    void Decide() {

        if (myState != (int) AnimState.Death && myState != (int)AnimState.Attacking && myState != (int)AnimState.Hurt && canMove == true) {
            if (isMoving == false) {
                int MoveOrIdle = Random.Range(0, 10);    //Stay idle or move

                if (MoveOrIdle <= 1) {                  //Idle
                    myState = (int)AnimState.Idle;
                    rigidBody.velocity = Vector3.zero;
                }
                else {
                    myState = (int)AnimState.Move;
                    
                }
            }
        }
    }

    void MoveToPlayer() {
        
        myState = (int)AnimState.Move;
        FaceTheTarget(myPlayer.transform);

    }

    void Attack() {
        
        myState = (int)AnimState.Attacking;
        attackCounter = attackTimer;
    }

    new public void FixedUpdate() {
        base.FixedUpdate();

        if (myState == (int)AnimState.Death) {
            return;
        }

        XClampEnemy();

        bool playerInRange = PlayerInRange();
        if (playerInRange == true) {
            canHitPlayer = CanAttack();

            if (canHitPlayer == false) {
                MoveToPlayer();
            }
            else {
                FaceTheTarget(myPlayer.transform);
                if (attackCounter <= 0) {
                    Attack();
                }
                else {
                    attackCounter -= Time.deltaTime;
                }
            }
        } else {
            Decide();
        }

        //if (distanceToPlayer < detectRange) { 
        //    canSeePlayer = CanSeePlayer(); //Can the enemy see the player?
        //} else {
        //    canSeePlayer = false;
        //}
        ////Debug.Log(canSeePlayer);

        //if (canSeePlayer == false) {   //If no, then starts the normal routine
        //    Decide();
        //} else {                       //If yes, we check whether the player is in attack range
        //    canHitPlayer = CanAttack();

        //    if (canHitPlayer == false) {               
        //        MoveToPlayer();
        //    } else {
        //        FaceTheTarget(myPlayer.transform);
        //        if (attackCounter <= 0) {
        //            Attack();
        //        } else {
        //            attackCounter -= Time.deltaTime;
        //        }
        //    }
        //    //Debug.Log(myState);
        //}


        myAnimator.SetInteger("State", myState);
    }

    private void Update() {
        if (Time.time > nextFlipChance && isMoving == false) {
            if (Random.Range(0, 10) >= 5) {
                Flip();
            }
            nextFlipChance = Time.time + flipTime;
        }
    }

    private void OnAnimatorMove() {
        if (rigidBody != null) { 
            rigidBody.velocity = myAnimator.deltaPosition / Time.deltaTime;
        }
    }
}
