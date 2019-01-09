using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MovingObject {

    [Header("Camera + GameMaster")]
    public GameObject myMainCamera;
    public GameObject myGameMaster;
    public GameMaster gameMasterScript;

    [Header("Info")]
    //Jump
    protected float stamina = 0;
    protected float initialGravityScale = 10;       //This is for wall slide only! Since friction is a ***ch, we need to adjust gravity scale directly
    public float yRestriction = -1;
    public bool isReseting = false;
    public bool canMoveX = true;

    [Header("General Attack")]
    public bool attacking = false;
    public float attackTimer = 0;          //Attack animation time
    public Collider2D attackTrigger;        //Weapon Collider
    public int myAttackState;

    [Header("Attack Info")]
    AttackType normalAttack;
    AttackType jumpAttack;
    AttackType runAttack;
    AttackType curAttack;
    public int currentComboState = 0;
    public bool canDoNextCombo = false;

    [Header("Status Info")]
    public Status poisoned;
    public Status damageBuff;
    public Status speedBuff;

    [Header("Abiliies Info")]
    public Ability doubleJump;

    [Header("VFX")]
    public float waitTime_VFX = 0.0025f;
    public GameObject myRunningTrail;            //Run
    public GameObject runningTrail;
    //public bool spawnRunningTrail = false;
    public GameObject myJumpingTrail;            //Jump
    public GameObject jumpingTrail;
    public GameObject wallSlideTrail_1;          //Wall Slide
    public GameObject wallSlideTrail_2;
    public GameObject myWallSlideTrail;
    //public bool spawnWallSlideTrail = false;

    [Header("Stats")]
    public int infestedCoin = 0;

    public class AttackType {

        public float time;
        public float baseDmg;
        public float damage;
        public bool unlocked;

        public int maxCombo;

        public AttackType(float myTime, float myDmg, bool isUnlocked, int combo) {
            time = myTime;
            damage = myDmg;
            baseDmg = myDmg;
            unlocked = isUnlocked;
            maxCombo = combo;
        }

    }

    public class Ability {
        public bool canUse;
        public bool unlocked;

        public Ability(bool canBeUsed, bool isUnlocked) {
            canUse = canBeUsed;
            unlocked = isUnlocked;
        }
    }

    public class Status {
        public int statusID;
        public bool active = false;

        public int curMultiplier = 0;
        public float valueEachMultiplier;

        public float cooldownTime;
        public float timer = 0;

        public int minMultiplier;
        public int maxMultiplier;

        public Status(StatusType name, float value, float time, int minV, int maxV) {
            statusID = (int) name;
            valueEachMultiplier = value;
            cooldownTime = time;

            minMultiplier = minV;
            maxMultiplier = maxV;
        }
    }

    public enum StatusType {
        Nothing = -1,
        DamageBuff = 0,
        SpeedBuff = 1,
        Poisoned = 2,
    }

    enum AttackState {
        Nothing = -1,
        NormalAttack = 0,
        JumpAttack = 1,
        RunAttack = 2
    }
   
    // Use this for initialization
    void Start () {
        myGameMaster = GameObject.Find("GameMaster");
        myMainCamera = Camera.main.gameObject;

        //Components
        myCollider = GetComponent<BoxCollider2D>();
        //rigidBody.collisionDetectionMode = CollisionDetectionMode2D.
        initialVelocity = maxVelocity.x;
        initialSpeed = speed;
        myAnimator = GetComponent<Animator>();
        curAnimation = GetComponent<Animation>();
        initialGravityScale = rigidBody.gravityScale;

        //Attack
        normalAttack = new AttackType(0.5f, 100, true, 3);
        jumpAttack = new AttackType(0.5f, 100, true, 1);
        runAttack = new AttackType(0.5f, 100, true, 1);

        //Abilities
        doubleJump = new Ability(true, false);

        //Status
        poisoned = new Status(StatusType.Poisoned, 2, 5, 0, 5);
        speedBuff = new Status(StatusType.SpeedBuff, 5, 10, -5, 5);
        damageBuff = new Status(StatusType.DamageBuff, 10, 10, -5, 5);

        //Initial States
        myState = (int)AnimState.Idle;
        myAttackState = (int)AttackState.Nothing;

        //Sprite
        direction = 1;      //1 = right - -1 = left

        //VFX
        StartCoroutine(SpawnVFX());
    }
	
    private void Update() {

        //Force ray
        raycastOrigin = myCollider.offset + new Vector2(transform.position.x, transform.position.y);

        //Draw ray
        Debug.DrawRay(raycastOrigin, new Vector2(rigidBody.velocity.x, 0), Color.blue);  //Horizontal
        Debug.DrawRay(raycastOrigin, new Vector2(0, rigidBody.velocity.y), Color.green);  //Vertical
        Debug.DrawRay(raycastOrigin, rigidBody.velocity, Color.red);    //Whole movement vector

        if (Time.timeScale == 0) {
            return;
        }

        //Status
        bool lose = CheckLoseStatus();
        if (statusIndicatorScript != null) { 
            statusIndicatorScript.SetHealth(curHealth, maxHealth);
        }
        if (lose == true && isReseting == false) {
            myGameMaster.GetComponent<GameMaster>().KillPlayer();
        }

        bool tempGrounded = IsGrounded();
        bool tempHitWall = IsHitWall();
        if (tempHitWall == true && completelyHitWall == false) {    //Just hit the wall
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        }

        if (tempGrounded == true && grounded == false) {    //Just landed
            JustLanded();
        }
        grounded = tempGrounded;
        completelyHitWall = tempHitWall;

        //State 
        myState = StateCheck();
        myAnimator.SetInteger("State", myState);
        myAnimator.SetBool("HitWall", completelyHitWall);
        myAnimator.SetBool("Grounded", grounded);

        //Move and user input
        if (canMove == true) {
            MoveHorizontal();
            MoveVertical();
            WallSlideCheck();
            ClampVelocity();
        }
        //Attack
        myAnimator.SetBool("CanDoNextCombo", canDoNextCombo);
        Attack();
        myAnimator.SetInteger("AtkState", myAttackState);
        myAnimator.SetInteger("ComboState", currentComboState);
        //Test 
        //IsHitWall();
    }

    IEnumerator SpawnVFX() {
        while (myState != (int)AnimState.Death) {
            if (myState == (int)AnimState.WallSlide) {
                SpawnWallSlideDust();
            }

            if (myState == (int)AnimState.Move) {
                SpawnRunningDust();
            }

            yield return new WaitForSeconds(waitTime_VFX);
        }
    }

    void SpawnWallSlideDust() {
        GameObject dust = Instantiate(myWallSlideTrail, wallSlideTrail_1.transform.position, myWallSlideTrail.transform.rotation);
        if (direction == -1) {
            dust.GetComponent<SpriteRenderer>().flipX = true;
        }

        dust = Instantiate(myWallSlideTrail, wallSlideTrail_2.transform.position, myWallSlideTrail.transform.rotation);
        if (direction == -1) {
            dust.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void SpawnRunningDust() {
        GameObject dust = Instantiate(myRunningTrail, runningTrail.transform.position, Quaternion.identity);
        if (direction == -1) {
            dust.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    int StateCheck() {

        if (myState == (int)AnimState.Attacking && (completelyHitWall == false || grounded == true)) {
            return (int)AnimState.Attacking;
        }

        if (myState == (int)AnimState.Hurt) {
            return (int)AnimState.Hurt;
        }

        if (grounded == false) {
            if (myState != (int)AnimState.Hurt) {
                if (grounded == false) {
                    if (completelyHitWall == false) {
                        return (int)AnimState.Jump;
                    }
                    else {

                        return (int)AnimState.WallSlide;
                    }
                }
            }
        } else {
            if (Input.GetAxisRaw("Horizontal") == 0 || (canMove == false && grounded == true)) {
                return (int)AnimState.Idle;
            } else {
                return (int)AnimState.Move;
            }
        }
        
        return (int)AnimState.Idle;
    }

    void WallSlideCheck() {
        if (myState == (int)AnimState.WallSlide) {
            rigidBody.gravityScale = 1;
        } else {
            rigidBody.gravityScale = initialGravityScale;
        }
    }
        
    void JumpStatusCheck() {
        if (grounded == true) {
            canJump = true;
            doubleJump.canUse = true;
        }
        else {
            canJump = false;
        }
    }

    void JustLanded() {
        Instantiate(myJumpingTrail, jumpingTrail.transform.position, Quaternion.identity);
        JumpStatusCheck();
        attackTimer = 0;
    }

    void MoveHorizontal() {
        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX != 0) {

            if (canMoveX == false || Time.timeScale == 0) {
                return;
            }

            if (completelyHitWall == true || wallStuck == true) {
                if (direction * inputX > 0) {
                    return;
                }
            }

            isMovingX = true;
            
            maxVelocity.x = initialVelocity;
            speed = initialSpeed;

            if (inputX > 0 && direction == -1) {
                Flip();
            }
            else if (inputX < 0 && direction == 1) {
                Flip();
            }

            float moveForce = inputX * speed;

            rigidBody.velocity = new Vector2(moveForce, rigidBody.velocity.y);
            
        }
        else {
            isMovingX = false;
            if (grounded == true) {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
    }

    void MoveVertical() {
        float verticalVelocity = 0;
        float horizontalVelocity = 0;

        if (Input.GetKeyDown(KeyCode.Space) == true && Time.timeScale != 0) {
            if (grounded == true) {
                verticalVelocity = jumpForce;
                doubleJump.canUse = true;
                myState = (int)AnimState.Jump;
                //Instantiate(myJumpingTrail, jumpingTrail.transform.position, Quaternion.identity);

                //Reset y-velocity
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, 0);
            }
            else {
                if (completelyHitWall == true) {
                    horizontalVelocity = speed * -direction * 10;
                    verticalVelocity = jumpForce;
                    myState = (int)AnimState.Jump;
                    doubleJump.canUse = false;
                    SpawnWallSlideDust();

                    Flip();
                    //Reset y-velocity
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, 0);

                    //Disable movement in x-axi in certain time
                    StartCoroutine(DisableXMovement(0.1f));
                } else if (doubleJump.unlocked == true && doubleJump.canUse == true) {
                    verticalVelocity = jumpForce;
                    doubleJump.canUse = false;
                    myState = (int)AnimState.Jump;

                    //Reset y-velocity
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, 0);
                }
            }

            Vector3 moveVector = new Vector3(horizontalVelocity, verticalVelocity, 0);
            rigidBody.AddForce(moveVector);
        }   
    }

    void ClampVelocity() {
        Vector2 newVelocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, -maxVelocity.y, 2 * maxVelocity.y));
        rigidBody.velocity = newVelocity;
    }

    void ClampPosition() {

    }
    
    void Attack() {
        if (Input.GetKeyDown(KeyCode.H) == true) {
            if (attackTimer <= 0 || canDoNextCombo == true) {
                if (currentComboState == 0) {           //New attack state
                    if (grounded == true) {             //Check what type of attack
                        if (rigidBody.velocity.x == 0) {
                            myAttackState = (int)AttackState.NormalAttack;
                            curAttack = normalAttack;
                            canMove = false;
                        }
                        else {
                            myAttackState = (int)AttackState.RunAttack;
                            curAttack = runAttack;
                        }
                    }
                    else {
                        myAttackState = (int)AttackState.JumpAttack;
                        curAttack = jumpAttack;
                    }
                }                                //Keep the combo "flowing"

                currentComboState += 1;

                attacking = true;
                canDoNextCombo = false;
                attackTimer = curAttack.time;   //When can we do another attack?

                attackTrigger.enabled = true;
                attackTrigger.GetComponent<GeneralObject>().SetDamage(curAttack.damage);

                myState = (int)AnimState.Attacking;
            }
        }

        if (attacking == true) {
            if (attackTimer > 0) {             //Can we do next attack yet?
                attackTimer -= Time.deltaTime;
            }
            else {                //RESET 'EM ALL!
                ResetAttack();
            }
        }

    }
    
    
        /*
    void Attack() {
        if (Input.GetKeyDown(KeyCode.H) == true) {
            if (attackTimer <= 0 || canDoNextCombo == true) {

                if (currentComboState == 0) {           //New attack state
                    if (grounded == true) {             //Check what type of attack
                        if (rigidBody.velocity.x == 0) {
                            myAttackState = (int)AttackState.NormalAttack;
                            curAttack = normalAttack;
                            canMove = false;
                        }
                        else {
                            myAttackState = (int)AttackState.RunAttack;
                            curAttack = runAttack;
                        }
                    }
                    else {
                        myAttackState = (int)AttackState.JumpAttack;
                        curAttack = jumpAttack;
                    }
                }                                //Keep the combo "flowing"

                currentComboState += 1;
                attacking = true;
                canDoNextCombo = false;
                attackCoolDown = curAttack.time;   //When can we execute the next combo?
                attackTimer = attackCoolDown + attackCoolDown * 0.3f;   //When can we do another attack?

                attackTrigger.enabled = true;
                attackTrigger.GetComponent<GeneralObject>().SetDamage(curAttack.damage);

                myState = (int)AnimState.Attacking;
            }
        }

        if (attackCoolDown > 0) {           //Can we do next combo yet?
            attackCoolDown -= Time.deltaTime;
        }
        else {
            if (attacking == true && currentComboState < curAttack.maxCombo) {
                canDoNextCombo = true;
            }
        }

        if (attacking == true) {
            if (attackTimer > 0) {             //Can we do next attack yet?
                attackTimer -= Time.deltaTime;
            }
            else {                //RESET 'EM ALL!
                Debug.Log("Có nè");
                attacking = false;
                canDoNextCombo = false;
                currentComboState = 0;
                myAttackState = (int)AttackState.Nothing;

                canMove = true;
                if (myState == (int)AnimState.Attacking) {
                    if (grounded == true) {
                        myState = (int)AnimState.Idle;
                    }
                    else {
                        myState = (int)AnimState.Jump;
                    }
                }
            }
        }
    }
    */

    new public bool HealthStatusCheck() {
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
        if (curHealth <= 0) {
            return false;
        }
        return true;
    }

    public override bool IsGrounded() {
        bool result = base.IsGrounded();

        if (result == false) {
            myAnimator.SetFloat("YVelocity", rigidBody.velocity.y);
        } else {
            myAnimator.SetFloat("XVelocity", rigidBody.velocity.x);
        }
        return result;
    }

    public override void TakeDamage(float value) {
        base.TakeDamage(value);
        //myState = (int)AnimState.Hurt;

    }

    public bool CheckLoseStatus() {
        if (HealthStatusCheck() == false) {
            return true;
        }

        if (transform.position.y < yRestriction) {
            return true;
        }

        return false;
    }

    new public void ResetObject() {
        base.ResetObject();
        rigidBody.velocity = Vector3.zero;
    }

    public void ResetAttack() {
        attacking = false;
        canDoNextCombo = false;
        currentComboState = 0;
        attackTimer = 0;
        myAttackState = (int)AttackState.Nothing;

        canMove = true;
        if (myState == (int)AnimState.Attacking) {
            if (grounded == true) {
                myState = (int)AnimState.Idle;
            }
            else {
                myState = (int)AnimState.Jump;
            }
        }

    }

    //Modifier
    public void ModifyDamageModifier(float value, int option) {        
        //option = default => %
        //option = 1 => add
        //option = 2 => change directly
        switch (option) {
            case 1:
                myDamageModifier += value;
                break;
            case 2:
                myDamageModifier = value;
                break;
            default:
                myDamageModifier += myDamageModifier / 100 * value;
                break;
        }

        //Reupdate the damage of all attack
        normalAttack.damage = normalAttack.baseDmg * myDamageModifier;
        jumpAttack.damage = jumpAttack.baseDmg * myDamageModifier;
        runAttack.damage = runAttack.baseDmg * myDamageModifier;
    }
    
    public void MaxHealthModifier(float value, int option) {
        //option = default => %
        //option = 1 => add
        //option = 2 => change directly
        switch (option) {
            case 1:
                maxHealth += value;
                break;
            case 2:
                maxHealth = value;
                break;
            default:
                maxHealth += maxHealth / 100 * value;
                break;
        }


    }

    public void ApplyModifier() {

    }

    //Misc

    //Disable movement on x-axi in certain waitTime
    public IEnumerator DisableXMovement(float waitTime) {
        canMoveX = false;

        yield return new WaitForSeconds(waitTime);

        canMoveX = true;
    }
}
