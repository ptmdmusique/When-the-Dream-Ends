using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : GeneralObject {
    public bool canFlip = false;
    public int  direction = 1;

    protected void Flip() {
        localScale.x *= -1;
        direction *= -1;
        transform.localScale = localScale;
    }
}
