using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// General UI controlls for ESC menu / Death menu

public class LevelUIContorls : MonoBehaviour
{
    public Loading LoadingManager;
    public GameObject EscMenu;
    private bool paused = false;

    public void LoadScene(string sceneName){
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("Player").layer = 0;
        EscMenu.SetActive(false);
        LoadingManager.AsyncLoad(sceneName);
    }

    public void Quit() {
        Application.Quit();
    }

    public void Continue() {
        Time.timeScale = 1;
        EscMenu.SetActive(false);
        paused = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            EscMenu.SetActive(!EscMenu.activeSelf);
            Time.timeScale = EscMenu.activeSelf ? 0 : 1;
            paused = EscMenu.activeSelf;
        }

        if(paused && Input.GetKeyDown(KeyCode.E))
            Continue();
    }
}
