using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DFollowNormal : MonoBehaviour {

    private Vector2 velocity;

    [Header("Some Parameter")]
    public float smoothTimeY;
    public float smoothTimeX;
    public float offsetY;

    [Header("Camera Position")]
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    [Header("Game Object")]
    public GameObject target;

	// Use this for initialization
	void Start () {
        offsetY = (transform.position.y - target.transform.position.y);

    }
	
	// Update is called once per frame
	void Update () {
        float posX = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, target.transform.position.y + offsetY, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }

    public void SetMinCamPosition() {
        minCameraPos = gameObject.transform.position;
    }

    public void SetMaxCamPosition() {
        maxCameraPos = gameObject.transform.position;
    }
}
