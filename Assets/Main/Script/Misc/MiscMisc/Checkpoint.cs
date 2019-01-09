using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour {

    private GameMaster myGMScript;
    public Text checkpointText;
    
	// Use this for initialization
	void Start () {
        myGMScript = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        if (checkpointText == null) { 
            checkpointText = transform.Find("Text").GetComponent<Text>();
        }
        checkpointText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            checkpointText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Player myPlayerScript = collision.GetComponent<Player>();
        if (myPlayerScript != null) {
            checkpointText.enabled = false;
        }
    }

    private void Update() {
        if (checkpointText.enabled == true) {
            if (Input.GetKeyDown(KeyCode.Return) == true) {
                myGMScript.currentCheckpoint = transform;
                StartCoroutine(UIChange());
            }
        }
    }

    IEnumerator UIChange() {
        checkpointText.text = "Checkpoint changed!";
        yield return new WaitForSeconds(2);
        checkpointText.text = "Press Enter to change checkpoint";
    }
}
