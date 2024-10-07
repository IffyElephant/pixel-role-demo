using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script for getting back to menu from Win Scene

public class BackToMenu : MonoBehaviour
{
    public Loading LoadingManager;

    public void LoadScene(string levelName) {
        PlayerPrefs.SetInt("Saved", 0);
        LoadingManager.AsyncLoad(levelName);
    }
}
