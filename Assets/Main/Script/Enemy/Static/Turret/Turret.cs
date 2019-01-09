using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : StaticEnemy {

    [Header("Bullet")]
    public Transform[] bulletList;
    public int curBullet = 0;
    public float[] bulletSpeed = { 3 };
    public float[] bulletLifeTime = { 5 };
    public int bulletPerWave = 3;
    public float waveWait = 1.5f;

    [Header("Turret")]
    public bool regularAttack = true;       //Just shoot left and right, not any other direction?
    public bool isDynamic = false;          //Do we actively face the player and shoot at him/her?
    public Transform shootingPoint;
    public float shootingRate;          //Number of bullet(s) shot per seconds
    public Transform metalClash;

    new public void Start() {
        base.Start();
        shootingPoint = transform.Find("ShootingPoint");

        StartCoroutine(Shoot());
    }

    IEnumerator Shoot() {

        bool canAttack = CanAttack();

        if (canAttack == true) {
            //Regular attack
            if (regularAttack == true) {
                for (int indx = 0; indx < bulletPerWave; indx++) {
                    Transform myBullet = Instantiate(bulletList[curBullet], shootingPoint.position, Quaternion.identity);
                    Projectile bulletScript = myBullet.GetComponent<Projectile>();
                    bulletScript.direction = new Vector3(direction, 0, 0);
                    bulletScript.mySpeed = bulletSpeed[curBullet];
                    bulletScript.myLifeTime = bulletLifeTime[curBullet];
                    myAnimator.SetTrigger("Attack");
                    yield return new WaitForSeconds(1 / shootingRate);
                }
            }
        }

        yield return new WaitForSeconds(waveWait);

        StartCoroutine(Shoot());
    }

    private void Update() {
        if (isDynamic == true) {
            FaceTheTarget(myPlayer.transform);
        }

        PlayerInRange();
        myAnimator.SetBool("Active", playerInRange);

    }

    public override void TakeDamage(float value) {
        base.TakeDamage(value);
        Vector3 spawnLoc = myCollider.bounds.center;
        Transform myMetalClash = Instantiate(metalClash, spawnLoc, Quaternion.identity);
        myMetalClash.parent = transform;
    }
}
