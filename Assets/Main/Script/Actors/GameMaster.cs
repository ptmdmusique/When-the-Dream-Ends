using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    [Header("Canvas - Hud - Camera")]
    public Transform myCanvas;
    public StatManager statManagerScript;
    public Transform myMainCamera;
    public bool atImportantPoint = false;

    [Header("Checkpoints")]
    public GameObject checkpointParent;
    public Transform currentCheckpoint;
    public List<Transform> checkpointList;

    [Header("Enemies")]
    public List<Transform> enemyManagers;

    [Header("Backgrounds")]
    public GameObject backgroundParent;

    [Header("Collectibles")]
    public GameObject collectibleParent;

    [Header("Player")]
    public GameObject myPlayer;
    public GameObject playerPrefab;
    public float spawnDelay = 2;
    public Transform spawnPrefab;

    [Header("GameMaster")]
    public static GameObject myGM;

    [Header("Start and End Level Text")]
    public string levelName = "";
    public int levelNumber = 0;
    public Transform myLevelStart;

    [Header("Dead Notification")]
    public Transform myDeadNotification;
    private GameObject prevTarget;

    //Personal
    private Coroutine respawnPlayer;
    //Debug
    int runningCoroutine = 0;

	// Use this for initialization
	void Start () {
        if (myGM == null) {
            myGM = GameObject.Find("GameMaster");
        }

        //Canvas
        myCanvas = transform.parent.transform.Find("Canvas");
        statManagerScript = myCanvas.Find("StatManager").GetComponent<StatManager>();

        GameObject[] managerList = GameObject.FindGameObjectsWithTag("EnemyManager");
        foreach (GameObject manager in managerList) {
            enemyManagers.Add(manager.transform);
        }

        checkpointParent = GameObject.Find("Checkpoints");
        foreach(Transform child in checkpointParent.transform) {
            checkpointList.Add(child);
        }

        if (currentCheckpoint == null) {
            currentCheckpoint = checkpointList[0];
        }

        backgroundParent = GameObject.Find("Background");
        collectibleParent = GameObject.Find("Collectibles");
        myPlayer = GameObject.Find("Player");

        //Start Level
        Transform levelNoti = Instantiate(myLevelStart, transform.position, Quaternion.identity);
        levelNoti.Find("LevelText").GetComponent<Text>().text = "Chapter " + levelNumber;
        levelNoti.Find("LevelName").GetComponent<Text>().text = levelName;

        //Camera
        if (myMainCamera == null) { 
            myMainCamera = transform.parent.transform.Find("Main Camera").transform;
        }
    }
	
    public IEnumerator RespawnPlayer() {
        runningCoroutine++;
        //Return the player to the old place, then wait a little bit for the smoke to go out and "respawn" him
        myPlayer.GetComponent<Player>().ResetObject();
        myMainCamera.GetComponent<CameraFollow2>().myTarget = prevTarget;
        prevTarget = null;
        myPlayer.GetComponent<Player>().isReseting = false;

        GameObject clone = Instantiate(spawnPrefab, currentCheckpoint.position, spawnPrefab.transform.rotation).gameObject;
        float waitTime = clone.transform.Find("SpawnParticles").GetComponent<ParticleSystem>().main.duration;
        Destroy(clone, spawnDelay);

        yield return new WaitForSeconds(spawnDelay);
        //myPlayer.GetComponent<SpriteRenderer>().enabled = true;
        myPlayer.GetComponent<Player>().canMove = true;       //Then let the player have the control again

        StopRespawning();
    }

    public void StopRespawning() {
        if (respawnPlayer != null) {
            StopCoroutine(respawnPlayer);
            respawnPlayer = null;
        }
    }

    public void StartRespawning() {
        if (respawnPlayer == null) { 
            respawnPlayer = StartCoroutine(RespawnPlayer());
        }
    }

    public void ShowDeadNotification() {
        Instantiate(myDeadNotification, transform.position, Quaternion.identity);
        myPlayer.GetComponent<Player>().canMove = false;
        myPlayer.transform.position = currentCheckpoint.position;
        myPlayer.GetComponent<Player>().isReseting = true;

        prevTarget = myMainCamera.GetComponent<CameraFollow2>().myTarget;
        myMainCamera.GetComponent<CameraFollow2>().myTarget = null;
    }

    public void KillPlayer() {
        
        ShowDeadNotification();
        //StartCoroutine(RespawnPlayer());

        foreach (Transform enemyManager in enemyManagers) {     //Reset the enemy list and their health
            if (enemyManager.GetComponent<EnemyManager>().willRespawn == true) { 
                enemyManager.GetComponent<EnemyManager>().ResetEnemy();
            }
        }
    }

	// Update is called once per frame
	void Update () {
	}

    public void UpdateCoin() {
        statManagerScript.UpdateCoin();
    }
}
