using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretZero : StaticEnemy {

    private GameObject AlarmStatus;

	protected enum AnimState {
        Sleep,              //0
        Awake,              //1
        LookLeft,           //2
        LookRight,          //3
        Hurt                //4
    }

    //References
    public GameObject bullet;
    public Transform target;
    public GameObject shootPointLeft;
    public GameObject shootPointRight;
    public GameObject alarmStatus;
    private AlarmStatus alarmStatusScript;
    public GameObject shootingCone;
    public Coroutine attackCoroutine;

    //Floats
    public float wakeRange = 10;
    public float distance;
    public float shootInterval = 5;
    public float bulletSpeed = 5;
    public float bulletPerSecond = 3;
    public float bulletTimer = 0;
    public float bulletInterval = 0.5f;

    //Info
    public bool isFullyAwake = false;
    public GameObject gun;
    public bool canFire = false;
    public bool isFiring = false;

    protected override void Start() {
        base.Start();
        curHealth = maxHealth;
        AlarmStatus = transform.GetChild(0).gameObject;
        
        myAnimator = gameObject.GetComponent<Animator>();
        alarmStatusScript = alarmStatus.GetComponent<AlarmStatus>();

        gun = shootPointLeft;   //Left is the default state of the gun
        
    }

    private void Update() {
        myAnimator.SetInteger("State", myState);

        //Check if player is in range
        RangeCheck();

        //Change direction
        if (isFullyAwake == true) {
           
            if (myPlayer.transform.position.x < transform.position.x) {
                myState = (int)AnimState.LookLeft;
                gun = shootPointLeft;
            }
            else {
                myState = (int)AnimState.LookRight;
                gun = shootPointRight;
            }
        }

        if (canFire == false && myState != (int)AnimState.Sleep) {
            alarmStatus.GetComponent<AlarmStatus>().Detect();
        }

        //Shoot
        if (canFire == true) {
            if (Time.time - bulletTimer >= shootInterval) {
                if (attackCoroutine != null && isFiring == false) {
                    StopCoroutine(attackCoroutine);
                }
                if (attackCoroutine == null) { 
                    attackCoroutine = StartCoroutine(Attack());
                }
            } 
        } else {
            //bulletTimer = 0;                  //Use this if you want to reset the cooldown everytime the player gets out of range          
            if (attackCoroutine != null) { 
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            isFiring = false;
        }

        //Die?
        if (curHealth <= 0) {
            Destroy(transform.parent.gameObject);
        }
    }

    void RangeCheck() {
        distance = Vector3.Distance(transform.position, myPlayer.transform.position);

        if (distance > wakeRange) {
            myState = (int)AnimState.Sleep;
            alarmStatus.GetComponent<AlarmStatus>().OutOfRange();
            isFullyAwake = false;


        } else if (distance <= wakeRange && myState == (int)AnimState.Sleep) {
            myState = (int)AnimState.Awake;
            //alarmStatus.SetActive(true);
            alarmStatusScript.Detect();
            shootingCone.GetComponent<TurretAttackCone>().isFirstTime = true;
        }
    }

    public IEnumerator Attack() {
        
        //Start shooting
        isFiring = true;
        for (int indx = 0; indx < bulletPerSecond; indx++) {
            Vector2 direction = (new Vector3(target.transform.position.x, target.transform.position.y + 0.5f) - transform.position).normalized;
            GameObject bulletClone = Instantiate(bullet, gun.transform.position, Quaternion.identity);
            bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            yield return new WaitForSeconds(bulletInterval);
        }

        //Stop shooting
        isFiring = false;
        bulletTimer = Time.time;
        attackCoroutine = null;
    }


    
}
