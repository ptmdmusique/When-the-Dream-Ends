using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour {
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private Coroutine findTargetCoroutine;
    private BoxCollider2D myCollider;
    private Transform gameBorders;

    public GameObject myTarget;
    public Vector2 xRestriction;
    public Vector2 yRestriction;
    //public float xThreshold = 3;
    //public float yThreshold = 3;
    public bool restrictCameraToGame = true;
    public Transform curGameBorder;
    public float smooth;
    public Vector2 colliderOffset;

    private void Start() {
        findTargetCoroutine = StartCoroutine(FindTarget("Player"));
        myCollider = GetComponent<BoxCollider2D>();
        gameBorders = GameObject.Find("GameBorders").transform;

        CalculateCollider();
        CalculateBound(gameBorders.GetChild(0));

        if (restrictCameraToGame == true) { 
            float posX = Mathf.Clamp(transform.position.x, xRestriction.x, xRestriction.y);
            float posY = Mathf.Clamp(transform.position.x, yRestriction.x, yRestriction.y);

            transform.position = new Vector3(posX, posY, transform.position.z);
        }
        
    }

    private void LateUpdate() {
        if (myTarget == null) {
            return;
        }

        //float posX = myPlayer.transform.position.x + offset.x * myPlayer.GetComponent<Player>().GetDirection();
        //float posY = myPlayer.transform.position.y + offset.y;
        float posX = myTarget.transform.position.x;
        float posY = myTarget.transform.position.y + offset.y;
        float finalSmooth = smooth;

        if (restrictCameraToGame == true) {
            posX = Mathf.Clamp(posX, xRestriction.x, xRestriction.y);
            posY = Mathf.Clamp(posY, yRestriction.x, yRestriction.y);
        }

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, new Vector3(posX, posY, transform.position.z), ref velocity, finalSmooth);
        transform.position = smoothedPosition;
    }

    IEnumerator FindTarget(string name) {
        if (myTarget == null) {
            myTarget = GameObject.Find(name);
            offset = transform.position - myTarget.transform.position;
        }
        yield return new WaitForSeconds(0.5f);

        findTargetCoroutine = StartCoroutine(FindTarget(name));
    }

    public void CalculateBound(Transform parmBorder) {
        if (myTarget == null) {
            return;
        }

        //Camera Restriction
        curGameBorder = parmBorder;
        BoxCollider2D border = curGameBorder.GetComponent<BoxCollider2D>();
        Vector2 worldPos = (Vector2)(curGameBorder.transform.position) + border.offset; //Border center
        Vector2 size = border.size / 2f;    
        Camera camera = GetComponent<Camera>();
        Vector2 displacement = new Vector2(size.x - camera.orthographicSize * Screen.width / Screen.height, size.y - camera.orthographicSize);


        xRestriction.x = worldPos.x - displacement.x;
        xRestriction.y = worldPos.x + displacement.x;

        yRestriction.x = worldPos.y - displacement.y;
        yRestriction.y = worldPos.y + displacement.y;

    }

    void CalculateCollider() {
        Camera camera = GetComponent<Camera>();

        myCollider.size = new Vector2(camera.orthographicSize * Screen.width / Screen.height + colliderOffset.x, camera.orthographicSize + +colliderOffset.y) * 2;
    }

}
