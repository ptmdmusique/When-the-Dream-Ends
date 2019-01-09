using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadNotification : MonoBehaviour {

	public void SpawnPlayer() {
        GameMaster myGM = GameObject.Find("GameMaster").GetComponent<GameMaster>().transform.GetComponent<GameMaster>();
        myGM.StartRespawning();
    }
}
