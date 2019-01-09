using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterNoChild : MonoBehaviour {

    private void Update() {
        if (transform.childCount <= 0) {
            Destroy(gameObject);
        }
    }
}
