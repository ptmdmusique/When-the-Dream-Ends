using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : StaticObject {

    [Header("Enemy Stuff")]
    public bool playerInRange = false;
    public Vector2 attackRange = new Vector2(0,0);
    public bool attacking = false;
    public EnemyBoundary myBoundaryScript;
    public bool isDeath = false;
    public BoxCollider2D myCollider;
    //Collectibles Spawning
    public GameObject[] collectibleList;
    //Effect
    public Transform deathParticle;
    public Color deathParticleColor;
    private GameObject collectibles;

    [Header("VFX")]
    public Transform explosion;

    protected Player myPlayer;

    protected virtual void Start() {
        curHealth = maxHealth;
        myStatusIndicator.gameObject.SetActive(false);
        collectibles = GameObject.Find("Collectibles");
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        myBoundaryScript = transform.Find("Boundary").GetComponent<EnemyBoundary>();
    }

    public override void Die() {
        base.Die();
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        myAnimator.SetTrigger("IsHurt");
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

    new void Flip() {
        if (canFlip == false) {
            return;
        }

        base.Flip();
    }

    public bool PlayerInRange() {
        playerInRange = myBoundaryScript.playerInRange;
        return playerInRange;
    }

    public bool CanDetectPlayer() {
       Vector3 playerPos = myPlayer.transform.position;

        if (Mathf.Abs(playerPos.x - transform.position.x) < attackRange.x || Mathf.Abs(playerPos.y - transform.position.y) < attackRange.y) {
            return true;
        }

        return false;
    }

    public bool CanAttack() {
        PlayerInRange();
        if (playerInRange == true) {
            if (CanDetectPlayer() == true) {
                if (direction == 1) {
                    if (myPlayer.transform.position.x > transform.position.x) {
                        return true;
                    }
                }
                else {
                    if (myPlayer.transform.position.x < transform.position.x) {
                        return true;
                    }
                }
            }
        }

        return false;
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
            //Spawn blood
            SpawnDeathParticle();
            isDeath = true;
            //Destroy health bar
            myStatusIndicator.gameObject.SetActive(false);
            isEngaged = false;

            Vector3 spawnPoint = myCollider.bounds.center;
            if (collectibleList.Length > 0) {
                GameObject healthOrb = Instantiate(collectibleList[Random.Range(0, collectibleList.Length)], spawnPoint, Quaternion.identity);
                healthOrb.transform.parent = collectibles.transform;
            }
            Destroy(gameObject);
        }
    }


    public void SpawnDeathParticle() {
        //Spawn blood
        Vector3 spawnPoint = myCollider.bounds.center;
        spawnPoint.z -= 5;
        Transform myDeathParticle = Instantiate(deathParticle, spawnPoint, Quaternion.identity);
        ParticleSystem deathParticleSystem = myDeathParticle.GetComponent<ParticleSystem>();

        if (deathParticleSystem != null) {
            ParticleSystem.MainModule settings = deathParticleSystem.main;
            settings.startColor = deathParticleColor;
            //Child color
            settings = myDeathParticle.GetChild(0).GetComponent<ParticleSystem>().main;
            settings.startColor = deathParticleColor;
        }
    }

    public void FixedUpdate() {
        HealthStatusCheck();
    }
}
