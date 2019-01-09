using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MovingEnemy {

    private Vector3 initialPosition;            //The instantiated position
    bool movingToIdle = false;                  //Are we moving to the idle position?
    bool needToMove = false;                    //Do we need to move or just stand still?
    float decideTimer = 5f;                     //Interval between decisions
    float decideCounter = 0;
    public bool isAttacking = false;                 
    float chargingTimer = 1;                    //Charging timer

    public AStarMoveToObject pathFindingScript;  

	void Start () {
        initialPosition = transform.position;

        attackTimer = 2;
        attackCounter = 0;

        direction = -1;         //Default direction

        if (pathFindingScript == false) {
            pathFindingScript = gameObject.AddComponent<AStarMoveToObject>();
        }
    }

    //void Decide() {
    //    decideCounter = decideTimer;            //Reset the counter

    //    if (myState != (int)AnimState.Death && (myState != (int)AnimState.Attacking || canSeePlayer == false) && myState != (int)AnimState.Hurt && canMove == true) {

    //        int MoveOrIdle = Random.Range(0, 10);    //Stay idle or move

    //        if (MoveOrIdle <= 3 || movingToIdle == true) {    //If we decide to be idle, then try to move to the initial position and stay idle              
    //            if (transform.position != initialPosition) {
    //                myState = (int)AnimState.Move;
    //                myTarget = initialPosition;
    //                needToMove = true;
    //            }
    //            else {                          //If we are at the right position, become idle
    //                movingToIdle = false;
    //                needToMove = false;
    //                myState = (int)AnimState.Idle;
    //            }
    //        }
    //        else {                              //Move somewhere between the boundary
    //            Vector3 newTarget = new Vector3(Random.Range(boundary_1.transform.position.x + 3, boundary_2.transform.position.x - 3), initialPosition.y, transform.position.z);
    //            myTarget = newTarget;
    //            myState = (int)AnimState.Move;
    //            needToMove = true;
    //        }
    //    }

    //}

    //new private void FixedUpdate() {
    //    base.FixedUpdate();

    //    if (distanceToPlayer < detectRange) {
    //        canSeePlayer = CanSeePlayer();
    //    } else {
    //        canSeePlayer = false;
    //    }

    //    if (canSeePlayer == true) {           //Move closer to the player to attack
    //        decideCounter = 0;                //Reset the counter if we see the player so that when the player is out of range, we imediately decide what to do next
    //        myTarget = myPlayer.transform.Find("Target").transform.position;
    //        myTarget.x += -1.5f * direction;
    //        if (distanceToPlayer > attackRange) {   //If still can't attack then move closer
    //            myState = (int)AnimState.Move;
    //            movingToIdle = false;
    //            needToMove = true;

    //        } else {                           //Now attack
    //            needToMove = false;
    //            if (attackCounter <= 0) {
    //                if (myState != (int)AnimState.Attacking) {      //Have we charged?
    //                    myState = (int)AnimState.Attacking;
    //                    attackCounter = attackTimer;                //Reset the counter
    //                }
    //            }
    //        }
    //    } else {                               //If we can't see the player, then do the idling stuff

    //        if (decideCounter <= 0) {          //Only decide if we can't see the player and it's time to decide
    //            Decide();
    //        }
    //        else {
    //            decideCounter -= Time.deltaTime;
    //        }

    //        if (transform.position == myTarget) {    //If we arrive, then we don't need to move anymore
    //            needToMove = false;
    //        }
    //    }

    //    attackCounter -= Time.deltaTime;            //Reduce the attack counter no matter if we are attacking or not

    //    if (needToMove == true) {                   //We move to the decided position
    //        transform.position = Vector3.MoveTowards(transform.position, myTarget, speed * Time.deltaTime);
    //    } else if (isAttacking == true) {
    //        transform.position = Vector3.MoveTowards(transform.position, myTarget, speed * 5 * Time.deltaTime); //Move with breakneck speed to the player
    //        if (transform.position == myTarget) {
    //            isAttacking = false;
    //        }
    //    }

    //    FaceTheTarget(myTarget);
    //    myAnimator.SetInteger("State", myState);
    //}

    //private void OnAnimatorMove() {
    //    if (rigidBody != null) {
    //        rigidBody.velocity = myAnimator.deltaPosition / Time.deltaTime;
    //    }
    //}

    //new private void OnCollisionEnter2D(Collision2D collision) {    //Do damage stuff
    //    base.OnCollisionEnter2D(collision);
    //    if (collision.collider.CompareTag("Player") == true) {

    //        myState = (int)AnimState.Move;                      //Bounce back a little bit
    //        isAttacking = false;
    //        myPlayer.GetComponent<Player>().TakeDamage(myBaseDamage);
    //        myPlayer.GetComponent<Player>().StartKnockback(0.5f, transform);

    //    }
    //}     //Old stuff


        
    //New stuff
    new public void FixedUpdate() {
        base.FixedUpdate();

        if (distanceToPlayer <= detectRange) {
            canSeePlayer = CanSeePlayer();
        } else {
            canSeePlayer = false;
        }

        if (myState == (int)AnimState.Death) {
            return;
        }

        XClampEnemy();

        bool playerInRange = PlayerInRange();
        if (playerInRange == true && distanceToPlayer <= detectRange) {
            if (isAttacking == false) {
                //Move to player
                pathFindingScript.target = playerTarget.transform;
                pathFindingScript.staticTarget = false;
                pathFindingScript.standBy = false;
                pathFindingScript.speed = 25000;

                pathFindingScript.StartPath();
                isAttacking = true;
                movingToIdle = false;

                myState = (int)AnimState.Move;
            }
            FaceTheTarget(myPlayer.transform);
        } else {
            if (Vector3.Distance(transform.position, initialPosition) >= 0.1f) {
                if (movingToIdle == false) {
                    //Move back
                    pathFindingScript.staticPos = initialPosition;
                    pathFindingScript.staticTarget = true;
                    pathFindingScript.standBy = false;
                    pathFindingScript.speed = 5000;

                    pathFindingScript.StartPath();
                    isAttacking = false;
                    movingToIdle = true;

                    myState = (int)AnimState.Move;
                }
                FaceTheTarget(initialPosition);
            } else {
                //Standby
                pathFindingScript.StandBy();
                movingToIdle = false;
                isAttacking = false;
                myState = (int)AnimState.Idle;
            }
        }
        myAnimator.SetInteger("State", myState);
    }

}
