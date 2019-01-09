using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject myMainCamera;
    public CameraShake cameraShake;
    public bool isOpened = false;
    public Animator myAnimator;

    private void Start() {
        myMainCamera = Camera.main.gameObject;
        cameraShake = myMainCamera.GetComponent<CameraShake>();
        myAnimator = GetComponent<Animator>();
    }

    public void UnlockDoor() {
        isOpened = true;
        myAnimator.SetBool("Open", isOpened);
    }

    public void LockDoor() {
        isOpened = false;
        myAnimator.SetBool("Open", isOpened);
    }

    public void StartCameraShake() {
        cameraShake.StartShaking();
    }

    public void StopCameraShake() {
        cameraShake.StopShaking();
    }
}
