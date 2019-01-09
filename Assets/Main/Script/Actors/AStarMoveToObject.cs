using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStarMoveToObject : MonoBehaviour {

    //Object to chase
    public Transform target;
    public bool staticTarget = false;   
    public Vector3 staticPos;

    //How many times each second we will update our path
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D myRB;

    //The calculated path
    private Coroutine currentUpdatePath;
    public Path path;

    //The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;
    public bool standBy = false;

    //The max distance from AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //Index of the current waypoint that we are trying to move towards
    private int currentWaypoint = 0;

    protected void Start() {
        seeker = GetComponent<Seeker>();
        myRB = GetComponent<Rigidbody2D>();

        if (target == null) {
            GameObject.Find("Player");
        }

        currentUpdatePath = StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath() {
        if (target == null && staticTarget == false) {
            target = GameObject.Find("Player").transform.Find("Target").transform;
        }

        //Start a new path to the target position and return the result to the OnPathComplete method
        if (standBy == false) {
            if (staticTarget == false) {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
            }
            else {
                seeker.StartPath(transform.position, staticPos, OnPathComplete);
            }
        }

        yield return new WaitForSeconds(1f / updateRate);

        if (standBy == false && staticTarget == false) { 
            currentUpdatePath = StartCoroutine(UpdatePath());
        }
    }

    public void StartPath() {
        if (currentUpdatePath != null) {
            StopCoroutine(currentUpdatePath);
        }

        currentUpdatePath = StartCoroutine(UpdatePath());
    }

    public void StandBy() {
        target = null;
        standBy = true;
        staticTarget = false;
    }

    public void OnPathComplete(Path myPath) {
        if (myPath.error == false) {
            path = myPath;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate() {       
        if (path == null || myRB == null) {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            if (pathIsEnded == true) {
                return;
            }
            return;
        }

        float finalMaxDistance = nextWaypointDistance;
        if (standBy == false) {
            if (staticTarget == false) {
                if (Vector3.Distance(transform.position, target.position) <= nextWaypointDistance) {
                    finalMaxDistance = 0.1f;
                }
            }
            else {
                if (Vector3.Distance(staticPos, transform.position) <= nextWaypointDistance) {
                    finalMaxDistance = 0.1f;
                }
            }
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        //Move the AI
        myRB.AddForce(dir, fMode);

        //Increase to the next waypoint if the distance to the next point is smaller than the desired distance
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance) {
            currentWaypoint++;
        }

    }
}
