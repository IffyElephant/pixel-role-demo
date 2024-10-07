using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script for changing scenes, while in dungeon it activates bossfight before letting player leave the stage

public class NextLevelPortal : MonoBehaviour
{
    public GameObject PressEText;
    public Loading loadingManager;
    public GameObject[] bosses;
    public bool bossEncounter = true;

    [SerializeField]
    private string levelName;
    private bool bossKilled = false;
    private bool bossSpawned = false;

    private void Start()
    {
        PressEText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PressEText.activeSelf) {
            if((bossEncounter == true && bossKilled == true) || bossEncounter == false)
                loadingManager.AsyncLoad(levelName);
            else {
                PressEText.SetActive(false);
                bossSpawned = true;
                Invoke("SpawnBoss", 2);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (bossSpawned == false || bossKilled == true)) {
            PressEText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            PressEText.SetActive(false);
        }
    }

    private void SpawnBoss() {
        PressEText.SetActive(false);
        int bossType = PlayerPrefs.GetInt("Stage", 0);
        GameObject boss = Instantiate(bosses[bossType % bosses.Count()], transform.position - Vector3.one, transform.rotation, transform);
    }

    public void BoosKilled() {
        bossKilled = true;

        int progressStage = PlayerPrefs.GetInt("Stage", 0) + 1;
        
        if (progressStage >= 3) {
            bossKilled = false;
            loadingManager.AsyncLoad("Win");
        }

        PlayerPrefs.SetInt("Stage", progressStage);
        
        int progressSeed = PlayerPrefs.GetInt("Seed", 0) / 2; 
        PlayerPrefs.SetInt("Seed", progressSeed);
    }
}
