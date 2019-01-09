using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseUI;
    public GameObject debugMenu;
    public float customTimeScale = 1;
    public bool debugEnable = false;

    private bool isPaused = false;

    private void Start() {
        pauseUI.SetActive(false);
        debugMenu.SetActive(false);
    }


    private void Update() {
        if (Input.GetButtonDown("Pause") == true) {
            isPaused = !isPaused;
            if (isPaused == true) {
                pauseUI.SetActive(true);
                Time.timeScale = 0;
            }
            else {
                pauseUI.SetActive(false);
                if (Time.timeScale == 0) {
                    Time.timeScale = 1;
                } else {
                    Time.timeScale = 0;
                }
                debugMenu.SetActive(false);
            }
        } 

        if (Input.GetKeyDown(KeyCode.P) == true) {
            if (Time.timeScale == 0) {
                Time.timeScale = 1;
                debugMenu.SetActive(false);
            } else {
                Time.timeScale = 0;
                debugMenu.SetActive(true);
            }
        }
        if (debugMenu.activeSelf == true) {
            if (Input.GetKeyDown("r") == true) {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (debugEnable == true) { 
            Time.timeScale = customTimeScale;
        }
    }

    public void Resume() {
        isPaused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;        
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }

}
