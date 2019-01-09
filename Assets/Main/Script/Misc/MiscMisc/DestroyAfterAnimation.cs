using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour {

    public bool destroy = false;

    public void DestroyObject() {
        Destroy(gameObject);
    }
}
