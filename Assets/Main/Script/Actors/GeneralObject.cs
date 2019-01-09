using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObject : MonoBehaviour {

    [Header("Info")]
    //Basic Info
    public string myNickname = "";
    public string myName = "";
    public string myTag = "";
    public bool isEngaged = false;
    //Damage
    public float myBaseDamage = 0;
    public float myDamageModifier = 1;
    //Temp buffs
    public float tempDamageModifier = 0;
    public float tempSpeedModifier = 0;
    public float tempHealthModifier = 0;
    //Score and value
    public float myScore = 0;
    public float myValue = 0;               //Health-up, Coin, etc
    //Health stuff
    public bool isInvincible = false;
    public StatusIndicator statusIndicatorScript;
    public Transform myStatusIndicator;
    protected Vector3 localScale;
    public float curHealth = 100;
    public float maxHealth = 100;

    [Header("Animation")]
    protected Animator myAnimator;
    protected Animation curAnimation;
    public int myState;

    public virtual void Awake() {
        if (myTag.Contains("Invincible") == true) {
            isInvincible = true;
        }
    }

    //HealthStuff
    public void SetCurHealth(float newValue) {
        curHealth = newValue;
    }

    public void ModifyCurHealth(float value) {
        curHealth += value;
    }

    public float GetCurHealth() {
        return curHealth;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }

    public void SetMaxHealth(float amount) {
        maxHealth = amount;
    }

    public virtual void TakeDamage(float damage) {
        if (isInvincible == false) {
            ModifyCurHealth(-damage);
        }
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    //Damage stuff
    public void SetDamage(float value) {
        myBaseDamage = value;
    }

    public bool HealthStatusCheck() {
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
        if (curHealth <= 0) {
            Die();
            return false;
        }
        return true;
    }

    public void UpdateHealthBar() {
        statusIndicatorScript.SetHealth(curHealth, maxHealth);
    }

    public void ResetObject() {
        curHealth = maxHealth;
        if (transform.tag == "Enemy") {
            isEngaged = false;
            myStatusIndicator.gameObject.SetActive(false);
        }
    }
}
