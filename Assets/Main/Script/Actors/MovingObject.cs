using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : GeneralObject {

    [Header("GameObjectComponents")]
    public Rigidbody2D rigidBody;
    protected BoxCollider2D myCollider;

    [Header("Raycast")]
    protected Vector2 raycastOrigin;
    protected Vector2 rayLength;

    [Header("Status")]
    protected bool onGround = true;
    protected bool canJump = true;
    protected bool isMovingX = false;
    public bool canMove = true;
    protected int direction = 0; // 1 = Face right ; -1 = Face left.
    //WallCheck
    public float wallCheckRadius = 0.1f;
    protected bool completelyHitWall = false;
    //GroundCheck
    public bool grounded = true;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    protected bool wallStuck = false;
    

    //protected int jumpingDirection = 0;

    [Header("Move")]
    public float speed = 8;
    public Vector2 maxVelocity = new Vector2(8, 8);
    protected float initialVelocity;
    protected float initialSpeed;
    public bool isMoving = false;

    [Header("Jump")]
    public float jumpForce = 15, forwardForce = 0f;

    protected enum AnimState {
        Idle = 0,       //0
        Move = 1,       //1
        Hurt = 3,       //3
        Jump = 4,       //4
        Death = 5,        //5
        Attacking = 6,   //6
        WallSlide = 7
    };

    public override void Awake() {
        base.Awake();

        rigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();

        rayLength.y = myCollider.size.y / 2;
        
        localScale = transform.localScale;
        curHealth = maxHealth;

        //Time.timeScale = 0.1f;
    }

    //Ray cast / Collision Detection
    public virtual bool IsGrounded(){
        //Circle collider...
        bool result = false;

        //Raycast
        Vector3 center = myCollider.bounds.center;
        center.y -= (myCollider.bounds.extents.y + 0.1f);
        float distance = myCollider.bounds.extents.x - 0.1f;
        Vector3 origin = center;
        origin.x -= distance;
        Vector3 dest = center;
        dest.x += distance;
        Debug.DrawRay(origin, dest - origin, Color.red);

        result = Physics2D.Raycast(origin, dest - origin, Vector3.Distance(dest, origin), groundLayer);

        return result;
    }

    public virtual bool IsHitWall() {
        //if (grounded == true) {
        //    wallStuck = false;
        //    return false;
        //}

        Vector3 center = myCollider.bounds.center;
        Vector2 offset = new Vector2(myCollider.bounds.extents.x + 0.03f, myCollider.bounds.extents.y - 0.01f);
        Vector3 origin = new Vector3(center.x + offset.x * direction, center.y - offset.y, 0);
        Vector3 midPoiint = new Vector3(center.x + offset.x * direction, center.y);
        Vector3 dest = new Vector3(center.x + offset.x * direction, center.y + offset.y, 0);
        wallStuck = Physics2D.Raycast(origin, dest - origin, Vector3.Distance(dest, origin), groundLayer);
        Debug.DrawRay(origin, dest - origin, Color.cyan);

        bool hitUpperWall = Physics2D.OverlapCircle(origin, wallCheckRadius, groundLayer);
        bool hitLowerWall = Physics2D.OverlapCircle(dest, wallCheckRadius, groundLayer);
        bool hitMiddleWall = Physics2D.OverlapCircle(midPoiint, wallCheckRadius, groundLayer);

        if ((hitUpperWall == true || hitLowerWall == true) && hitMiddleWall == true) { 
            return true;
        }

        return false;
    }

    //Effect
    public void StartKnockback(float knockDur, Transform knockObj) {
        StartCoroutine(Knockback(knockDur, knockObj));
    }

    public IEnumerator Knockback (float knockDur, Transform knockObj) {
        isInvincible = true;                            //Invincible frame \m/
        Vector2 knockBackForce = new Vector3();

        Vector2 direction = (-knockObj.position + myCollider.bounds.center).normalized;

        //Cal the knockback direction
        knockBackForce = direction;
        //Debug.DrawRay(myCollider.bounds.center, knockBackForce, Color.cyan, 3);
        knockBackForce.x *= 150;
        knockBackForce.y *= 150;


        myAnimator.SetTrigger("IsHurt");

        //rigidBody.AddForce(knockBackForce);

        rigidBody.velocity = Vector2.zero;

        rigidBody.AddForce(knockBackForce);

        yield return new WaitForSeconds(knockDur);
        isInvincible = false;                       //Stop being invicible :'(

        if (IsGrounded() == false) {
            myState = (int)AnimState.Jump;
        }
        else {
            myState = (int)AnimState.Idle;
        }
    }

    protected void Flip() {
        localScale.x *= -1;
        direction *= -1;
        transform.localScale = localScale;
    }

    //Getter Setter
    public int GetDirection() {
        return direction;
    }

    public Rigidbody2D GetRigidBody() {
        return rigidBody;
    }

    //Debug

}
