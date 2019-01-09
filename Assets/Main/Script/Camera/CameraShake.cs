using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Player myPlayerScript;

    public bool loopShaking = false;
    public float shakeTimer = 0;
    public float shakeAmount = 0;
    public float prevShakeAmount;
    
	void Start () {
        myPlayerScript = GameObject.Find("Player").GetComponent<Player>();
    }

	// Update is called once per frame
	void Update () {
        if (shakeTimer > 0 || loopShaking == true) {
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);

            shakeTimer -= Time.deltaTime;
        }		
	}

    public void StartShaking() {
        loopShaking = true;
        prevShakeAmount = shakeAmount;
        shakeAmount = 0.1f;
    }

    public void StopShaking() {
        loopShaking = false;
        shakeAmount = prevShakeAmount;
    }
}
