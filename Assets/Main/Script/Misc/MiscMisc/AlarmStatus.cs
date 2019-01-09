using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmStatus : SpriteObject {

    private Animator myAnimator;

    public enum AnimState {
        OutOfRange, //0
        Detect,     //1
        InRange     //2
    }
    public int myState = 0;

    private void Start() {
        myAnimator = GetComponent<Animator>();
    }

    private void Update() {
        myAnimator.SetInteger("State", myState);
    }

    public void OutOfRange() {
        myState = 0;
    }

    public void Detect() {
        myState = 1;
    }

    public void InRange() {
        myState = 2;
    }

    
}
