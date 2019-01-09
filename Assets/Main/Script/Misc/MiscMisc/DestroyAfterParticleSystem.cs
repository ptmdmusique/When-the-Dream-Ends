using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterParticleSystem : MonoBehaviour {

    private ParticleSystem thisPS;

    private void Start() {
        thisPS = GetComponent<ParticleSystem>();

    }

    private void Update() {
        if (thisPS.isPlaying == true) {
            return;
        }

        Destroy(gameObject);
    }
}
