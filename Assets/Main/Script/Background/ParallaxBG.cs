using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //For list stuff!

public class ParallaxBG : MonoBehaviour {
    ///Player
    private int myPlayerDirection;

    /// Scrolling speed
    public Vector2 speed = new Vector2(10, 10);

    /// Moving direction
    public Vector2 direction = new Vector2(-1, 0);

    /// Movement should be applied to camera
    public bool isLinkedToCamera = false;

    /// 1 - Background is infinite
    public bool isLooping = false;

    /// 2 - List of children with a renderer.
    private List<SpriteRenderer> backgroundPart;

    //Camera initial position
    private float camPosX;

    bool firstTime = true;

    // 3 - Get all the children
    void Awake() {
        

        if (isLooping) {
            // Get all the children of the layer with a renderer
            backgroundPart = new List<SpriteRenderer>();

            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                SpriteRenderer r = child.GetComponent<SpriteRenderer>();

                // Add only the visible children
                if (r != null) {
                    backgroundPart.Add(r);
                }
            }

            // Sort by position.
            // Note: Get the children from left to right.
            // We would need to add a few conditions to handle
            // all the possible scrolling directions.
            backgroundPart = backgroundPart.OrderBy(
              t => t.transform.position.x
            ).ToList();
        }

        camPosX = Camera.main.transform.position.x;
    }

    void Update() {
        
        if (firstTime == false) {
            //Get the direction of the player
            myPlayerDirection = GameObject.Find("Player").GetComponent<Player>().GetDirection();
        } else {
            myPlayerDirection = -1;
            firstTime = false;
        }
        if (isLinkedToCamera == true) { 
            transform.position = new Vector3((Camera.main.transform.position.x - camPosX) / speed.x + 5, transform.position.y, transform.position.z) ;
        } else {
            transform.position = new Vector3((-Camera.main.transform.position.x + camPosX) / speed.x + 5, transform.position.y, transform.position.z);
        }

        // 4 - Loop
        if (isLooping) {
            if (myPlayerDirection == 1) {
                // Get the first object.
                // The list is ordered from left (x position) to right.
                SpriteRenderer firstChild = backgroundPart.FirstOrDefault();
                
                if (firstChild != null) {
                    // Check if the child is already (partly) before the camera.
                    // We test the position first because the IsVisibleFrom
                    // method is a bit heavier to execute.
                    if (firstChild.transform.position.x < Camera.main.transform.position.x) {
                        // If the child is already on the left of the camera,
                        // we test if it's completely outside and needs to be
                        // recycled.
                        if (firstChild.IsVisibleFrom(Camera.main) == false) {
                            // Get the last child position.
                            SpriteRenderer lastChild = backgroundPart.LastOrDefault();

                            Vector3 lastPosition = lastChild.transform.position;
                            Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                            // Set the position of the recyled one to be AFTER
                            // the last child.
                            // Note: Only work for horizontal scrolling currently.
                            firstChild.transform.position = new Vector3(lastPosition.x + lastSize.x, firstChild.transform.position.y, firstChild.transform.position.z);

                            // Set the recycled child to the last position
                            // of the backgroundPart list.
                            //backgroundPart.Remove(firstChild);
                            //backgroundPart.Add(firstChild);

                            backgroundPart = backgroundPart.OrderBy(
                                t => t.transform.position.x
                            ).ToList();
                        }
                    }
                }
            }
            else {
                // Get the first object.
                // The list is ordered from left (x position) to right.
                SpriteRenderer firstChild = backgroundPart.LastOrDefault();
                if (firstChild != null) {
                    // Check if the child is already (partly) before the camera.
                    // We test the position first because the IsVisibleFrom
                    // method is a bit heavier to execute.
                    if (firstChild.transform.position.x > Camera.main.transform.position.x) {
                        // If the child is already on the left of the camera,
                        // we test if it's completely outside and needs to be
                        // recycled.
                        if (firstChild.IsVisibleFrom(Camera.main) == false) {
                            // Get the last child position.
                           
                            SpriteRenderer lastChild = backgroundPart.FirstOrDefault();

                            Vector3 lastPosition = lastChild.transform.position;
                            Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                            // Set the position of the recyled one to be AFTER
                            // the last child.
                            // Note: Only work for horizontal scrolling currently.
                            firstChild.transform.position = new Vector3(lastPosition.x - lastSize.x, firstChild.transform.position.y, firstChild.transform.position.z);

                            // Set the recycled child to the last position
                            // of the backgroundPart list.
                            //backgroundPart.Remove(firstChild);
                            //backgroundPart.Add(firstChild);
                            backgroundPart = backgroundPart.OrderBy(
                                t => t.transform.position.x
                            ).ToList();
                        }
                    }
                }
            }
        }
    }
}
