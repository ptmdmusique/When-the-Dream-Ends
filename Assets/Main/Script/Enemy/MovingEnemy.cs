using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingEnemy : MovingObject {

    [Header("Status + Info")]
    public bool isDeath = false;
    public GameObject myPlayer;

    [Header("Attack Stuff")]
    public float detectRange = 0;
    public float attackRange = 0;
    public float distanceToPlayer;
    public float maxDetectionElevation;
    public GameObject attackCollider;

    [Header("Boundaries")]
    public EnemyBoundary myBoundary;
    public BoxCollider2D myBoundaryCollider;
    public Vector2 xBoundary;
    public Vector2 yBoundary;

    protected Transform playerTarget;
    protected float attackTimer = 0;
    protected float attackCounter = 0;
    protected bool canHitPlayer = false;
    protected bool canSeePlayer = false;

    protected float flipTime = 5f;
    protected float nextFlipChance = 0f;
    protected bool canFlip = true;

    [Header("Collectibles")]
    public GameObject[] collectibleList;

    [Header("Organizers")]
    private GameObject collectibles;

    [Header("Misc")]
    public Color deathParticleColor = Color.red;
    public Transform deathParticle;
    public Transform mySpawnParticle;

    new private void Awake() {
        base.Awake();

        //Organizers
        collectibles = GameObject.Find("Collectibles");

        myPlayer = GameObject.Find("Player");    //Find the player
        if (myPlayer != null) {
            playerTarget = myPlayer.transform.Find("Target");
        }
        
        myBoundary = transform.parent.Find("Boundary").GetComponent<EnemyBoundary>();
        myBoundaryCollider = transform.parent.Find("Boundary").GetComponent<BoxCollider2D>();

        //X and Y boundaries
        xBoundary.x = myBoundaryCollider.bounds.center.x - myBoundaryCollider.bounds.extents.x;
        xBoundary.y = myBoundaryCollider.bounds.center.x + myBoundaryCollider.bounds.extents.x;

        yBoundary.x = myBoundaryCollider.bounds.center.y - myBoundaryCollider.bounds.extents.y;
        yBoundary.y = myBoundaryCollider.bounds.center.y + myBoundaryCollider.bounds.extents.y;

        //Status Indicator
        myStatusIndicator = transform.parent.transform.Find("StatusIndicator");
        statusIndicatorScript = myStatusIndicator.GetComponent<StatusIndicator>();
        statusIndicatorScript.offset = myStatusIndicator.position - transform.position;
        statusIndicatorScript.offset.x = 0;
        myStatusIndicator.gameObject.SetActive(false);

        //Spawn Particle
        Vector3 spawnPos = myCollider.bounds.center;
        Transform something = Instantiate(mySpawnParticle, spawnPos, mySpawnParticle.rotation);
        ParticleSystem.MainModule settings = something.GetComponent<ParticleSystem>().main;
        settings.startColor = deathParticleColor;

    }

    public override bool IsHitWall() {
        Vector2 colliderCenter = new Vector2(transform.position.x + myCollider.offset.x * direction, transform.position.y + myCollider.offset.y - myCollider.bounds.extents.y);

        //Left ray
        Vector3 collisionRayOrigin = new Vector3(colliderCenter.x - myCollider.bounds.extents.x - 0.05f, colliderCenter.y, transform.position.z);
        RaycastHit2D[] allHitLeft = Physics2D.RaycastAll(collisionRayOrigin, Vector3.up, myCollider.size.y);
        Debug.DrawRay(collisionRayOrigin, new Vector3(0, myCollider.size.y, 0), Color.cyan);

        foreach (RaycastHit2D ifHit in allHitLeft) {
            //Debug.Log(ifHit.transform.GetComponent<GeneralObject>().myTag.Contains("PathEnd"));
            GeneralObject ifHitScript = ifHit.transform.GetComponent<GeneralObject>();
            bool isAGround = false;
            bool isPathEnd = false;
            if (ifHitScript != null) {
                isAGround = ifHitScript.myTag.Contains("Obstacle");
                isPathEnd = ifHitScript.myTag.Contains("PathEnd");
            }
            if (ifHit.transform.name != transform.name && isAGround == true && isPathEnd == true) {
                return true;
            }
        }

        return false;
    }

    public virtual bool CanAttack() {
        //Cast an upward ray from the center of the object
        Vector2 colliderCenter = (Vector2)transform.position + myCollider.offset;

        Vector3 collisionRayOrigin = new Vector3(colliderCenter.x, colliderCenter.y, transform.position.z);
        RaycastHit2D[] allHit = Physics2D.RaycastAll(collisionRayOrigin, Vector3.left * -direction, attackRange);
        Debug.DrawRay(collisionRayOrigin, Vector3.left * attackRange * -direction);

        foreach (RaycastHit2D ifHit in allHit) {
            //Debug.Log(ifHit.transform.GetComponent<GeneralObject>().myTag.Contains("PathEnd"));
            GeneralObject ifHitScript = ifHit.transform.GetComponent<GeneralObject>();

            if (ifHitScript != null) {
                if (ifHitScript.myTag.Contains("Player") == true) {
                    return true;
                }
            }
        }

        return false;
    }

    public virtual bool CanSeePlayer() {
        //Only "see" the player if the players is not roughly on the same elevation level
        if (maxDetectionElevation > 0) {
            if (Mathf.Abs(myPlayer.transform.position.y - transform.position.y) > maxDetectionElevation) {
                return false;
            }
        }

        //Cast an upward ray from the center of the object
        Vector2 colliderCenter = (Vector2) transform.position + myCollider.offset;

        Vector3 collisionRayOrigin = new Vector3(colliderCenter.x, colliderCenter.y, transform.position.z);
        RaycastHit2D[] allHit = Physics2D.RaycastAll(collisionRayOrigin, myPlayer.transform.position - transform.position, detectRange, groundLayer);
        
        foreach (RaycastHit2D ifHit in allHit) {
            //Debug.Log(ifHit.transform.GetComponent<GeneralObject>().myTag.Contains("PathEnd"));
            GeneralObject ifHitScript = ifHit.transform.GetComponent<GeneralObject>();

            if (ifHitScript != null) {
                    if (ifHitScript.myTag.Contains("SeeThrough") == false) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool PlayerInRange() {
        if (myBoundary.playerInRange == true && 
            (Mathf.Abs(myPlayer.transform.position.y - transform.position.y) < maxDetectionElevation 
            || maxDetectionElevation == -1)) {
            return true;
        }

        return false;
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        myAnimator.SetTrigger("IsHurt");
    }
   
    new public void HealthStatusCheck() {
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);

        if (statusIndicatorScript != null) {
            if (isEngaged == true) {
                myStatusIndicator.gameObject.SetActive(true);
                UpdateHealthBar();
            } 
        }

        if (curHealth <= 0) {
            if (myState != (int)AnimState.Death) {
                //Set state to death and start the animation
                myState = (int)AnimState.Death;

                myAnimator.Play("Death");
                Destroy(rigidBody);

                //Spawn blood
                SpawnDeathParticle();

                //Destroy health bar
                myStatusIndicator.gameObject.SetActive(false);
                isEngaged = false;
            }
        }

        if (isDeath == true) {
            //Spawn collectibles
            Vector3 spawnPoint = myCollider.bounds.center;
            if (collectibleList.Length > 0) {
                GameObject healthOrb = Instantiate(collectibleList[Random.Range(0, collectibleList.Length)], spawnPoint, Quaternion.identity);
                healthOrb.transform.parent = collectibles.transform;
            }

            Destroy(transform.parent.gameObject);
        }
    }


    public void SpawnDeathParticle() {
        //Spawn blood
        Vector3 spawnPoint = myCollider.bounds.center;
        spawnPoint.z -= 5;
        Transform myDeathParticle = Instantiate(deathParticle, spawnPoint, Quaternion.identity);
        ParticleSystem.MainModule settings = myDeathParticle.GetComponent<ParticleSystem>().main;
        settings.startColor = deathParticleColor;

        //Child color
        settings = myDeathParticle.GetChild(0).GetComponent<ParticleSystem>().main;
        settings.startColor = deathParticleColor;
    }

    public void FixedUpdate() {
        distanceToPlayer = Vector3.Distance(myPlayer.transform.position, transform.position);
        HealthStatusCheck();
    }

    public void XClampEnemy() {
        //If the enemy get out of bounds then flip
        float xPos = transform.position.x;
        if (xPos < xBoundary.x || xPos > xBoundary.y) {
            Flip();
        }
    }

    public void YClampEnemy() {
        float yPos = transform.position.y;
        if (yPos < xBoundary.x || yPos > xBoundary.y) {
            Flip();
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == transform.tag) {
            Physics2D.IgnoreCollision(collision.collider, myCollider);
        }
    }

    new void Flip() {
        if (canFlip == false) {
            return;
        }

        base.Flip();
    }

    public void FaceTheTarget(Transform myMovingTarget) {
        if (direction == 1) {
            if (myMovingTarget.transform.position.x < transform.position.x) {
                Flip();
            }
        }
        else {
            if (myMovingTarget.transform.position.x > transform.position.x) {
                Flip();
            }
        }
    }

    public void FaceTheTarget(Vector3 myMovingTarget) {
        if (direction == 1) {
            if (myMovingTarget.x < transform.position.x) {
                Flip();
            }
        }
        else {
            if (myMovingTarget.x > transform.position.x) {
                Flip();
            }
        }
    }

    //For debugging purposes
    private void OnDrawGizmosSelected() {
        //Gizmos.DrawSphere(transform.position, detectRange);
        //Gizmos.DrawSphere(transform.position, attackRange);
    }
}
